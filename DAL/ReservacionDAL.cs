namespace Hoteles.DAL
{
    using Hoteles.DEL;
    using Hoteles.UTL;
    using System;
    using System.Collections.Generic;
    using System.Data.OleDb;
    using System.Data.SqlClient;

    public class ReservacionDAL : AccesoDatos
    {
        public ReservacionDAL() : base() { }

        public bool Insertar(Reservacion reservacion)
        {
            double montoPagado = (double)reservacion.MontoTarjeta + (double)reservacion.MontoEfectivo;
            string codHabitacion = reservacion.CodHabit.ToString();
            HabitacionDAL habitacion = new HabitacionDAL();
            double montoHabitacion = (double)habitacion.SeleccionarPorId(codHabitacion).Precio;
            //if (montoHabitacion < montoPagado)
            //{
            //    SimpleLogger.LogWarning($"Ocurrió un error al intentar insertar la reservación. El monto con que se cancela es menor al monto de la habitación.");
            //    return false;
            //}

            if (reservacion.FechaLlegada >= reservacion.FechaSalida)
            {
                SimpleLogger.LogWarning($"La fecha de salida no puede ser anterior a la fecha de ingreso.");
                return false;
            }

            // Generar la base del código de reserva
            string fecha = DateTime.Now.ToString("yyyyMMdd");
            string baseCodReserva = $"{fecha}-{reservacion.CodHabit}-";

            // Consultar el máximo consecutivo actual para esa combinación
            string queryConsecutivo = "SELECT MAX(Cod_Reserva) FROM Reservaciones WHERE Cod_Reserva LIKE @pattern";
            SqlParameter[] paramConsecutivo = {
                new SqlParameter("@pattern", $"{baseCodReserva}%")
            };
            string maxCodReserva = ExecuteScalar(queryConsecutivo, paramConsecutivo) as string;

            int consecutivo = 1; // Iniciar el consecutivo en 1
            if (!string.IsNullOrEmpty(maxCodReserva))
            {
                string lastPart = maxCodReserva.Substring(maxCodReserva.LastIndexOf('-') + 1);
                if (int.TryParse(lastPart, out int lastConsecutivo))
                {
                    consecutivo = lastConsecutivo + 1;
                }
            }

            // Generar el nuevo Cod_Reserva
            string newCodReserva = $"{baseCodReserva}{consecutivo}";

            // Preparar la consulta de inserción
            string query = "INSERT INTO Reservaciones" +
                " (Cod_Reserva, Cod_Habit, ID_Cliente, Fecha_Llegada, Fecha_Salida, " +
                "Forma_Pago, Voucher, MontoTarjeta, MontoEfectivo, Cod_Hotel, Estado) " +
                "VALUES (@CodReserva, @CodHabit, @ID_Cliente, @FechaLlegada, @FechaSalida," +
                " @FormaPago, @Voucher, @MontoTarjeta, @MontoEfectivo, @Cod_Hotel, @Estado)";
            SqlParameter[] parameters = {
                new SqlParameter("@CodReserva", newCodReserva),
                new SqlParameter("@CodHabit", reservacion.CodHabit),
                new SqlParameter("@ID_Cliente", reservacion.IdCliente),
                new SqlParameter("@FechaLlegada", reservacion.FechaLlegada),
                new SqlParameter("@FechaSalida", reservacion.FechaSalida),
                new SqlParameter("@FormaPago", reservacion.FormaPago),
                new SqlParameter("@Voucher", reservacion.Voucher),
                new SqlParameter("@MontoTarjeta", reservacion.MontoTarjeta),
                new SqlParameter("@MontoEfectivo", reservacion.MontoEfectivo),
                new SqlParameter("@Cod_Hotel", reservacion.CodHotel),
                new SqlParameter("@Estado", "Activa")
            };

            try
            {
                int filasAfectadas = ExecuteNonQuery(query, parameters);
                return filasAfectadas > 0; // Retorna true si se afectó al menos una fila, indicando éxito en la inserción.
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Ocurrió un error al intentar insertar una reservación.", 
                    new Exception($"Detalle del error: {ex.Message}", ex));
                return false;
            }
        }

        public bool Actualizar(Reservacion reservacion)
        {
            ClienteDAL existeCliente = new ClienteDAL();
            if (existeCliente.ExistePorId(reservacion.IdCliente))
            {
                string query = "UPDATE Reservaciones SET " +
                    "Cod_Habit = @CodHabit, " +
                    "ID_Cliente = @ID_Cliente, " +
                    "Fecha_Llegada = @FechaLlegada, " +
                    "Fecha_Salida = @FechaSalida, " +
                    "Forma_Pago = @FormaPago, " +
                    "Voucher = @Voucher, " +
                    "MontoTarjeta = @MontoTarjeta, " +
                    "MontoEfectivo = @MontoEfectivo " +
                    "WHERE Cod_Reserva = @CodReserva AND Estado != 'Eliminada'";
                SqlParameter[] parameters = {
                    new SqlParameter("@CodHabit", reservacion.CodHabit),
                    new SqlParameter("@ID_Cliente", reservacion.IdCliente),
                    new SqlParameter("@FechaLlegada", reservacion.FechaLlegada),
                    new SqlParameter("@FechaSalida", reservacion.FechaSalida),
                    new SqlParameter("@FormaPago", reservacion.FormaPago),
                    new SqlParameter("@Voucher", reservacion.Voucher),
                    new SqlParameter("@MontoTarjeta", reservacion.MontoTarjeta),
                    new SqlParameter("@MontoEfectivo", reservacion.MontoEfectivo),
                    new SqlParameter("@CodReserva", reservacion.CodReserva)
                };

                try
                {
                    int filasAfectadas = ExecuteNonQuery(query, parameters);
                    return filasAfectadas > 0; // Retorna true si se afectó al menos una fila, indicando éxito en la inserción.
                }
                catch (Exception ex)
                {
                    SimpleLogger.LogError($"Ocurrió un error al intentar actualizar la reservacion: {reservacion.CodReserva}", 
                        new Exception($"Detalle del error: {ex.Message}", ex));
                    return false; // Indica un fallo en la inserción
                }
            }
            else
            {
                SimpleLogger.LogWarning($"No existe el ID_Cliente para la nueva actualización de reservación: {reservacion.CodReserva}");
                return false; // Indica un fallo en la inserción
            }
        }

        public bool Eliminar(string codReserva)
        {
            //string query = "DELETE FROM Reservaciones WHERE Cod_Reserva = ?";
            //OleDbParameter[] parameters = { new OleDbParameter("@CodReserva", codReserva) };
            //ExecuteNonQuery(query, parameters);

            //No voy a eliminar los registros, les voy a cambiar el estado
            string query = "UPDATE Reservaciones SET Estado = @Estado WHERE ID_Cliente = @CodReserva";
            SqlParameter[] parameters = {
                new SqlParameter("@Estado", "Eliminada"),
                new SqlParameter("@CodReserva", codReserva)
            };

            try
            {
                int filasAfectadas = ExecuteNonQuery(query, parameters);
                return filasAfectadas > 0; // Retorna true si se afectó al menos una fila, indicando éxito en la inserción.
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Ocurrió un error al intentar eliminar la reservación: {codReserva}", 
                    new Exception($"Detalle del error: {ex.Message}", ex));
                return false; // Indica un fallo en la inserción
            }
        }

        // Retorna una sola reserva al buscarla por su código de reserva
        public static Reservacion SeleccionarPorCodigo(string codReserva)
        {
            string query = "SELECT " +
                "Cod_Reserva, " +
                "Cod_Habit, " +
                "ID_Cliente, " +
                "Cod_Hotel, " +
                "Fecha_Llegada, " +
                "Fecha_Salida, " +
                "Forma_Pago, " +
                "Voucher, " +
                "MontoTarjeta, " +
                "MontoEfectivo, " +
                "Estado FROM Reservaciones " +
                "WHERE Cod_Reserva = @CodReserva AND Estado != 'Eliminada'";
            SqlParameter[] parameters = { new SqlParameter("@CodReserva", codReserva) };

            using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
            {
                if (reader.Read())
                {
                    return new Reservacion
                    {
                        CodReserva = reader["Cod_Reserva"].ToString(),
                        CodHabit = reader["Cod_Habit"].ToString(),
                        IdCliente = int.Parse(reader["ID_Cliente"].ToString()),
                        CodHotel = int.Parse(reader["Cod_Hotel"].ToString()),
                        FechaLlegada = DateTime.Parse(reader["Fecha_Llegada"].ToString()),
                        FechaSalida = DateTime.Parse(reader["Fecha_Salida"].ToString()),
                        FormaPago = reader["Forma_Pago"].ToString(),
                        Voucher = reader["Voucher"].ToString(),
                        MontoTarjeta = decimal.Parse(reader["MontoTarjeta"].ToString()),
                        MontoEfectivo = decimal.Parse(reader["MontoEfectivo"].ToString()),
                        Estado = reader["Estado"].ToString()
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        // Obtiene las reservaciones que tenga una habitacion entre un rango de fechas
        public static List<Reservacion> ObtenerReservacionesPorHabitacionYFechas(string codHabit, DateTime fechaLlegada, DateTime fechaSalida)
        {
            List<Reservacion> reservacionesEncontradas = new List<Reservacion>();
            string query = @"SELECT 
                Cod_Reserva, 
                Cod_Habit, 
                Cod_Hotel, 
                ID_Cliente, 
                Fecha_Llegada, 
                Fecha_Salida, 
                Forma_Pago, 
                Voucher, 
                MontoTarjeta, 
                MontoEfectivo, 
                Estado 
                FROM Reservaciones 
                WHERE Cod_Habit = @CodHabit AND 
                (Fecha_Llegada >= @FechaLlegadaInicio AND 
            Fecha_Llegada <= @FechaSalidaFin) AND Estado != 'Eliminada'";
            SqlParameter[] parameters = {
                new SqlParameter("@CodHabit", codHabit),
                new SqlParameter("@FechaLlegadaInicio", fechaLlegada),
                new SqlParameter("@FechaSalidaFin", fechaSalida)
            };

            try
            {
                using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
                {
                    while (reader.Read())
                    {
                        reservacionesEncontradas.Add(new Reservacion
                        {
                            CodReserva = reader["Cod_Reserva"] as string, // Convertir usando 'as' para que devuelva null si falla.
                            CodHabit = reader["Cod_Habit"] as string,
                            CodHotel = reader["Cod_Hotel"] is DBNull ? 0 : int.Parse(reader["Cod_Hotel"].ToString()), // Manejar DBNull y convertir a int.
                            IdCliente = reader["ID_Cliente"] is DBNull ? 0 : int.Parse(reader["ID_Cliente"].ToString()),
                            FechaLlegada = reader["Fecha_Llegada"] is DBNull ? DateTime.MinValue : DateTime.Parse(reader["Fecha_Llegada"].ToString()), // Usar un valor por defecto si es null.
                            FechaSalida = reader["Fecha_Salida"] is DBNull ? DateTime.MinValue : DateTime.Parse(reader["Fecha_Salida"].ToString()),
                            FormaPago = reader["Forma_Pago"] as string,
                            Voucher = reader["Voucher"] as string,
                            MontoTarjeta = reader["MontoTarjeta"] is DBNull ? 0 : decimal.Parse(reader["MontoTarjeta"].ToString()), // Convertir a decimal, manejar DBNull.
                            MontoEfectivo = reader["MontoEfectivo"] is DBNull ? 0 : decimal.Parse(reader["MontoEfectivo"].ToString()),
                            Estado = reader["Estado"] as string
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Error al obtener reservaciones.", new Exception($"Detalle del error: {ex.Message}", ex));
            }

            return reservacionesEncontradas;
        }

        // Apesar de ser un metodo que busca habitaciones disponibles, este se basa en
        // las Reservaciones por consiguiente debe ir en esta clase ReservacionDAL
        public static List<string> ObtenerHabitacionesDisponiblesPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            List<string> habitacionesDisponibles = new List<string>();

            string query = @"
            SELECT DISTINCT Cod_Habit FROM Habitaciones
            WHERE Cod_Habit NOT IN (SELECT DISTINCT Cod_Habit FROM Reservaciones
            WHERE ((Fecha_Salida BETWEEN @fechaInicio AND @fechaFin ) OR 
            (Fecha_Entrada BETWEEN @fechaInicio AND @fechaFin )) AND Estado != 'Eliminada')";

            SqlParameter[] parameters = {
                new SqlParameter("@fechaInicio", fechaInicio),
                new SqlParameter("@fechaFin", fechaFin)
            };

            try
            {
                using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
                {
                    while (reader.Read())
                    {
                        habitacionesDisponibles.Add(reader["Cod_Habit"] as string);
                    }
                }
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Error al obtener habitaciones disponibles.", new Exception($"Detalle del error: {ex.Message}", ex));
            }

            return habitacionesDisponibles;
        }

        // Método para obtener reservaciones activas a partir del ID del cliente
        // Utilizada con la intención de saber si un huesped tiene reservaciones activas para evitar eliminar el huesped.
        public static List<string> ObtenerCodigosReservasActivas(int ID_Cliente)
        {
            List<string> reservasActivas = new List<string>();
            string query = @"
                SELECT Cod_Reserva
                FROM Reservaciones
                WHERE ID_Cliente = @ID_Cliente AND Fecha_Salida >= @FechaActual AND Estado != 'Eliminada'";

            SqlParameter[] parameters = {
            new SqlParameter("@ID_Cliente", ID_Cliente),
            new SqlParameter("@FechaActual", DateTime.Now.Date)
            };

            using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
            {
                while (reader.Read())
                {
                    // Agrega el código de cada reserva activa encontrada a la lista
                    reservasActivas.Add(reader["Cod_Reserva"] as string);
                }
            }

            return reservasActivas; // Retorna la lista de códigos de reservas activas
        }

        //Se utiliza a la hora de eliminar un hotel para saber si este tiene reservaciones
        //activas, en caso de que si, no permitira borrar el hotel.
        public static bool ExistenReservacionesActivasParaHotel(int codHotel)
        {
            string query = @"
            SELECT COUNT(*) 
            FROM Reservaciones 
            WHERE Cod_Hotel = @CodHotel AND Fecha_Salida >= @FechaActual";

            SqlParameter[] parameters = {
            new SqlParameter("@CodHotel", codHotel),
            new SqlParameter("@FechaActual", DateTime.Now)
            };

            int count = 0;

            using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
            {
                while (reader.Read())
                {
                    count = reader.GetInt32(0);
                }
            }
            return count > 0;
        }

        public static List<Reservacion> ObtenerHistorialReservacionesPorCliente(int idCliente)
        {
            List<Reservacion> reservacionesEncontradas = new List<Reservacion>();
            string query = @"SELECT 
                    Cod_Reserva, 
                    Cod_Habit, 
                    Cod_Hotel, 
                    ID_Cliente, 
                    Fecha_Llegada, 
                    Fecha_Salida, 
                    Forma_Pago, 
                    Voucher,    
                    MontoTarjeta, 
                    MontoEfectivo,  
                    Estado 
                 FROM Reservaciones 
                 WHERE ID_Cliente = @ID_Cliente AND Estado != 'Eliminada'";
            SqlParameter[] parameters = {
                new SqlParameter("@ID_Cliente", idCliente)
            };

            try
            {
                using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
                {
                    while (reader.Read())
                    {
                        reservacionesEncontradas.Add(new Reservacion
                        {
                            CodReserva = reader["Cod_Reserva"] as string, // Convertir usando 'as' para que devuelva null si falla.
                            CodHabit = reader["Cod_Habit"] as string,
                            CodHotel = reader["Cod_Hotel"] is DBNull ? 0 : int.Parse(reader["Cod_Hotel"].ToString()), // Manejar DBNull y convertir a int.
                            IdCliente = reader["ID_Cliente"] is DBNull ? 0 : int.Parse(reader["ID_Cliente"].ToString()),
                            FechaLlegada = reader["Fecha_Llegada"] is DBNull ? DateTime.MinValue : DateTime.Parse(reader["Fecha_Llegada"].ToString()), // Usar un valor por defecto si es null.
                            FechaSalida = reader["Fecha_Salida"] is DBNull ? DateTime.MinValue : DateTime.Parse(reader["Fecha_Salida"].ToString()),
                            FormaPago = reader["Forma_Pago"] as string,
                            Voucher = reader["Voucher"] as string,
                            MontoTarjeta = reader["MontoTarjeta"] is DBNull ? 0 : decimal.Parse(reader["MontoTarjeta"].ToString()), // Convertir a decimal, manejar DBNull.
                            MontoEfectivo = reader["MontoEfectivo"] is DBNull ? 0 : decimal.Parse(reader["MontoEfectivo"].ToString()),
                            Estado = reader["Estado"] as string
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Error al obtener reservaciones.", new Exception($"Detalle del error: {ex.Message}", ex));
            }

            return reservacionesEncontradas;
        }

        public static List<Reservacion> ObtenerReservacionesPorFechas(DateTime fechaInicio, DateTime fechaFinal)
        {
            List<Reservacion> reservacionesEncontradas = new List<Reservacion>();
            string query = @"SELECT 
                Cod_Reserva, 
                Cod_Habit, 
                Cod_Hotel, 
                ID_Cliente, 
                Fecha_Llegada, 
                Fecha_Salida, 
                Forma_Pago, 
                Voucher, 
                MontoTarjeta,
                Cod_Hotel, 
                MontoEfectivo, 
                Estado 
            FROM Reservaciones WHERE (Fecha_Llegada BETWEEN @FechaInicio AND @FechaSalidaFin) AND Estado != 'Eliminada'";
            SqlParameter[] parameters = {
                new SqlParameter("@FechaInicio", fechaInicio),
                new SqlParameter("@FechaSalidaFin", fechaFinal)
            };

            try
            {
                using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
                {
                    while (reader.Read())
                    {
                        reservacionesEncontradas.Add(new Reservacion
                        {
                            CodReserva = reader["Cod_Reserva"] as string, // Convertir usando 'as' para que devuelva null si falla.
                            CodHabit = reader["Cod_Habit"] as string,                            
                            IdCliente = reader["ID_Cliente"] is DBNull ? 0 : int.Parse(reader["ID_Cliente"].ToString()),
                            FechaLlegada = reader["Fecha_Llegada"] is DBNull ? DateTime.MinValue : DateTime.Parse(reader["Fecha_Llegada"].ToString()), // Usar un valor por defecto si es null.
                            FechaSalida = reader["Fecha_Salida"] is DBNull ? DateTime.MinValue : DateTime.Parse(reader["Fecha_Salida"].ToString()),
                            FormaPago = reader["Forma_Pago"] as string,
                            Voucher = reader["Voucher"] as string,
                            MontoTarjeta = reader["MontoTarjeta"] is DBNull ? 0 : decimal.Parse(reader["MontoTarjeta"].ToString()), // Convertir a decimal, manejar DBNull.
                            MontoEfectivo = reader["MontoEfectivo"] is DBNull ? 0 : decimal.Parse(reader["MontoEfectivo"].ToString()),
                            CodHotel = reader["Cod_Hotel"] is DBNull ? 0 : int.Parse(reader["Cod_Hotel"].ToString()),
                            Estado = reader["Estado"] as string
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Error al obtener reservaciones.", new Exception($"Detalle del error: {ex.Message}", ex));
            }

            return reservacionesEncontradas;
        }

        public static List<Reservacion> ObtenerReservacionesPorFechas(DateTime fechaInicio, DateTime fechaFinal, int cod_Hotel)
        {
            List<Reservacion> reservacionesEncontradas = new List<Reservacion>();
            string query = @"SELECT 
                Cod_Reserva, 
                Cod_Habit, 
                ID_Cliente, 
                Fecha_Llegada, 
                Fecha_Salida, 
                Forma_Pago, 
                Voucher, 
                MontoTarjeta, 
                MontoEfectivo,
                Cod_Hotel,
                Estado 
            FROM Reservaciones WHERE (Fecha_Llegada BETWEEN @FechaInicio AND @FechaSalidaFin) 
            AND Cod_Habit IN (SELECT Cod_Habit FROM Habitaciones WHERE Cod_Hotel = @cod_Hotel) AND Estado != 'Eliminada'";
            SqlParameter[] parameters = {
                new SqlParameter("@FechaInicio", fechaInicio),
                new SqlParameter("@FechaSalidaFin", fechaFinal),
                new SqlParameter("@cod_Hotel", cod_Hotel)
            };

            try
            {
                using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
                {
                    while (reader.Read())
                    {
                        reservacionesEncontradas.Add(new Reservacion
                        {
                            CodReserva = reader["Cod_Reserva"] as string, // Convertir usando 'as' para que devuelva null si falla.
                            CodHabit = reader["Cod_Habit"] as string,                            
                            IdCliente = reader["ID_Cliente"] is DBNull ? 0 : int.Parse(reader["ID_Cliente"].ToString()),
                            FechaLlegada = reader["Fecha_Llegada"] is DBNull ? DateTime.MinValue : DateTime.Parse(reader["Fecha_Llegada"].ToString()), // Usar un valor por defecto si es null.
                            FechaSalida = reader["Fecha_Salida"] is DBNull ? DateTime.MinValue : DateTime.Parse(reader["Fecha_Salida"].ToString()),
                            FormaPago = reader["Forma_Pago"] as string,
                            Voucher = reader["Voucher"] as string,
                            MontoTarjeta = reader["MontoTarjeta"] is DBNull ? 0 : decimal.Parse(reader["MontoTarjeta"].ToString()), // Convertir a decimal, manejar DBNull.
                            MontoEfectivo = reader["MontoEfectivo"] is DBNull ? 0 : decimal.Parse(reader["MontoEfectivo"].ToString()),
                            CodHotel = reader["Cod_Hotel"] is DBNull ? 0 : int.Parse(reader["Cod_Hotel"].ToString()),
                            Estado = reader["Estado"] as string
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Error al obtener reservaciones.", new Exception($"Detalle del error(Recarga): {ex.Message}", ex));
            }

            return reservacionesEncontradas;
        }


        // Metodo utilizado para mostrar las reservaciones activas por Hotel, en el módulo Mostrar Reservaciones.
        public static List<Reservacion> ReservacionesActivasPorHotel(int cod_Hotel)
        {
            List<Reservacion> reservacionesEncontradas = new List<Reservacion>();
            string query = @"SELECT 
                Cod_Reserva, 
                Cod_Habit, 
                Cod_Hotel, 
                ID_Cliente, 
                Fecha_Llegada, 
                Fecha_Salida, 
                Forma_Pago, 
                Voucher, 
                MontoTarjeta, 
                MontoEfectivo, 
                Estado 
            FROM Reservaciones WHERE Cod_Habit IN (SELECT Cod_Habit WHERE Cod_Hotel = @Cod_Hotel) AND 
            Fecha_Salida >= @FechaActual AND Estado != 'Eliminada'";
            SqlParameter[] parameters = {
                new SqlParameter("@Cod_Hotel", cod_Hotel),
                new SqlParameter("@FechaActual", DateTime.Now.Date)
            };

            try
            {
                using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
                {
                    while (reader.Read())
                    {
                        reservacionesEncontradas.Add(new Reservacion
                        {
                            CodReserva = reader["Cod_Reserva"] as string, // Convertir usando 'as' para que devuelva null si falla.
                            CodHabit = reader["Cod_Habit"] as string,
                            CodHotel = reader["Cod_Hotel"] is DBNull ? 0 : int.Parse(reader["Cod_Hotel"].ToString()), // Manejar DBNull y convertir a int.
                            IdCliente = reader["ID_Cliente"] is DBNull ? 0 : int.Parse(reader["ID_Cliente"].ToString()),
                            FechaLlegada = reader["Fecha_Llegada"] is DBNull ? DateTime.MinValue : DateTime.Parse(reader["Fecha_Llegada"].ToString()), // Usar un valor por defecto si es null.
                            FechaSalida = reader["Fecha_Salida"] is DBNull ? DateTime.MinValue : DateTime.Parse(reader["Fecha_Salida"].ToString()),
                            FormaPago = reader["Forma_Pago"] as string,
                            Voucher = reader["Voucher"] as string,
                            MontoTarjeta = reader["MontoTarjeta"] is DBNull ? 0 : decimal.Parse(reader["MontoTarjeta"].ToString()), // Convertir a decimal, manejar DBNull.
                            MontoEfectivo = reader["MontoEfectivo"] is DBNull ? 0 : decimal.Parse(reader["MontoEfectivo"].ToString()),
                            Estado = reader["Estado"] as string
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Error al obtener reservaciones activas por hotel.", new Exception($"Detalle del error: {ex.Message}", ex));
            }

            return reservacionesEncontradas;
        }
    }
}
