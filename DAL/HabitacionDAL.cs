namespace Hoteles.DAL
{
    using Hoteles.DEL;
    using System;
    using System.Data.OleDb;
    using System.Collections.Generic;  // Para la clase genérica List<>
    using Hoteles.UTL;
    using System.Data.SqlClient;

    public class HabitacionDAL : AccesoDatos
    {
        public HabitacionDAL() : base() { }

        public bool Insertar(Habitacion habitacion)
        {
            // Extraer la primera letra de la categoría
            char primeraLetraCateg = habitacion.Categ[0];

            // Formar la base del código de la habitación
            string claveBase = $"{habitacion.CodHotel}{primeraLetraCateg}";

            // Consultar el máximo consecutivo actual para esa combinación
            string queryConsecutivo = "SELECT MAX(Cod_Habit) FROM Habitaciones WHERE Cod_Habit LIKE @pattern";
            SqlParameter[] queryConsecutivoParams = new SqlParameter[] {
                new SqlParameter("@pattern", $"{claveBase}%")
            };

            string maxCodHabit = ExecuteScalar(queryConsecutivo, queryConsecutivoParams) as string;
            int consecutivo = 1; // Iniciar el consecutivo en 1
            if (!string.IsNullOrEmpty(maxCodHabit))
            {
                // Intenta extraer y parsear el último carácter de maxCodHabit como un entero.
                // Esto se utiliza para obtener el número consecutivo actual del último Cod_Habit registrado que cumple con el patrón especificado.
                int lastNumber;
                if (int.TryParse(maxCodHabit.Substring(maxCodHabit.Length - 1), out lastNumber))
                {
                    consecutivo = lastNumber + 1;
                }
            }

            // Generar el nuevo Cod_Habit
            string newCodHabit = $"{claveBase}{consecutivo}";
            // Insertar la nueva habitación
            try
            {
                string query = "INSERT INTO Habitaciones " +
                    "(Cod_Habit, Cod_Hotel, Categ, Soleada, Lavado, Nevera, Precio, Cant_Pers, Estado) " +
                    "VALUES (@CodHabit, @CodHotel, @Categ, @Soleada, @Lavado, @Nevera, @Precio, @CantPers, @Estado)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@CodHabit", newCodHabit),
                new SqlParameter("@CodHotel", habitacion.CodHotel),
                new SqlParameter("@Categ", habitacion.Categ),
                new SqlParameter("@Soleada", habitacion.Soleada),
                new SqlParameter("@Lavado", habitacion.Lavado),
                new SqlParameter("@Nevera", habitacion.Nevera),
                new SqlParameter("@Precio", habitacion.Precio),
                new SqlParameter("@CantPers", habitacion.CantPers),
                new SqlParameter("@Estado", habitacion.Estado)
                };
                if (ExecuteNonQuery(query, parameters) == 0)
                {
                    throw new Exception("Failed to insert habitacion");
                }
                return true;
            }
            catch (Exception ex)
            {
                // Escribimos en el log de errores
                SimpleLogger.LogError($"Ocurrió un error al intentar insertar la habitacion: {newCodHabit}", 
                    new Exception($"Detalle del error: {ex.Message}", ex));
                return false;
            }
        }

        public bool Actualizar(Habitacion habitacion)
        {
            // Validación de la instancia de Habitacion
            if (habitacion == null)
            {
                SimpleLogger.LogError("Intento de actualizar una habitación nula.", 
                    new Exception("Datos de habitación no proporcionados."));
                return false;
            }

            bool EstadoActual = HabitacionOcupada(habitacion.CodHabit);
            if (EstadoActual)
            {
                SimpleLogger.LogError("Actualizacion de habitacion: ", 
                    new Exception("Imposible actualizar una habitación ocupada."));
                return false;
            }

            // Validar categoría
            var categoriasValidas = new HashSet<string> { "Premier", "Standard", "Junior" };
            if (!categoriasValidas.Contains(habitacion.Categ))
            {
                SimpleLogger.LogError("Categoría de habitación inválida.", 
                    new Exception($"Categoría proporcionada: {habitacion.Categ}"));
                return false;
            }

            // Validación de Precio y CantPers
            if (habitacion.Precio <= 0 || habitacion.CantPers <= 0)
            {
                SimpleLogger.LogError("Datos de habitación inválidos para la actualización.", 
                    new Exception("Precio o cantidad de personas es inválido."));
                return false;
            }

            string query = "UPDATE Habitaciones SET " +
                "Cod_Hotel = @CodHotel, " +
                "Categ = @Categ, " +
                "Soleada = @Soleada, " +
                "Lavado = @Lavado, " +
                "Nevera = @Nevera, " +
                "Precio = @Precio, " +
                "Cant_Pers = @CantPers " +
                "WHERE Cod_Habit = @CodHabit AND " +
                "Estado != 'Eliminada'";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CodHotel", habitacion.CodHotel),
                new SqlParameter("@Categ", habitacion.Categ),
                new SqlParameter("@Soleada", habitacion.Soleada),
                new SqlParameter("@Lavado", habitacion.Lavado),
                new SqlParameter("@Nevera", habitacion.Nevera),
                new SqlParameter("@Precio", habitacion.Precio),
                new SqlParameter("@CantPers", habitacion.CantPers),
                new SqlParameter("@CodHabit", habitacion.CodHabit)
            };

            try
            {
                int affectedRows = ExecuteNonQuery(query, parameters);
                if (affectedRows > 0)
                {
                    SimpleLogger.LogError($"La actualización de la habitación {habitacion.CodHabit} no se logró realizar.", 
                        new Exception("Error en la cantidad de filas afectadas."));                   
                    return false;
                }                
                return affectedRows > 0; // Devuelve true si se afectó al menos una fila, lo que indica que la actualización fue exitosa.                
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Error al intentar actualizar datos de habitación {habitacion.CodHabit}.", ex);
                return false;
            }
        }

        public bool Eliminar(string codHabit)
        {
            //string query = "DELETE FROM Habitaciones WHERE Cod_Habit = ?";
            //OleDbParameter[] parameters = new OleDbParameter[]
            //{
            //    new OleDbParameter("@CodHabit", codHabit)
            //};

            //ExecuteNonQuery(query, parameters);

            // No eliminamos el registro, únicamente le cambiamos el estado
            string query = "UPDATE Habitaciones SET Estado = 'Eliminada' WHERE Cod_Habit = @CodHabit AND Estado != 'Eliminada'";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CodHabit", codHabit)
            };

            try
            {
                int affectedRows = ExecuteNonQuery(query, parameters);
                if (affectedRows > 0)
                {
                    return true;
                }
                else
                {
                    SimpleLogger.LogWarning($"Error al intentar eliminar (actualizar estado de) la habitación con código {codHabit}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Registrar el error utilizando un logger
                SimpleLogger.LogError($"Error al intentar eliminar (actualizar estado de) la habitación con código {codHabit}.", ex);
                return false; // Devuelve false si ocurre un error durante la ejecución de la consulta
            }
        }

        public Habitacion SeleccionarPorId(string codHabit)
        {
            string query = "SELECT Cod_Habit, Cod_Hotel, Categ, Soleada, Lavado, Nevera, Precio, Cant_Pers, Estado " +
                "FROM Habitaciones WHERE Cod_Habit = @CodHabit AND Estado != 'Eliminada'";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CodHabit", codHabit)
            };

            using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
            {
                if (reader.Read())
                {
                    var habitacion = new Habitacion
                    {
                        CodHabit = reader["Cod_Habit"].ToString(),
                        CodHotel = reader["Cod_Hotel"] == DBNull.Value ? 0 : int.Parse(reader["Cod_Hotel"].ToString()),
                        Categ = reader["Categ"].ToString(),
                        Soleada = reader["Soleada"] == DBNull.Value ? false : bool.Parse(reader["Soleada"].ToString()),
                        Lavado = reader["Lavado"] == DBNull.Value ? false : bool.Parse(reader["Lavado"].ToString()),
                        Nevera = reader["Nevera"] == DBNull.Value ? false : bool.Parse(reader["Nevera"].ToString()),
                        Precio = reader["Precio"] == DBNull.Value ? 0 : decimal.Parse(reader["Precio"].ToString()),
                        CantPers = reader["Cant_Pers"] == DBNull.Value ? 0 : int.Parse(reader["Cant_Pers"].ToString()),
                        Estado = reader["Estado"] == DBNull.Value ? "" : reader["Estado"].ToString(),
                        Mobiliarios = MobiliarioDAL.SeleccionarMobiliarioPorHabitacionId(codHabit)
                    };
                    bool EstadoActual = HabitacionOcupada(codHabit);
                    if (EstadoActual)
                    {
                        habitacion.Estado = "Ocupada";
                    }
                    return habitacion;
                }
                else
                {
                    return null;
                }
            }
        }

        // Este otro metodo es con la intención de saber si una habitación posee alguna reserva pendiente.
        // Para saber si podemos eliminar una habitación o no.
        public bool ExistenReservasActivasParaHabitacion(string codHabitacion)
        {
            string query = @"
            SELECT 1
            FROM Reservaciones
            WHERE Cod_Habit = @CodHabitacion AND Fecha_Salida >= @FechaActual AND Estado != 'Eliminada'";

            SqlParameter[] parameters = {
            new SqlParameter("@CodHabitacion", codHabitacion),
            new SqlParameter("@FechaActual", DateTime.Now)
            };

            using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
            {
                bool hayReservaActiva = reader.Read();  // Retorna true si hay al menos una reserva activa, de lo contrario, false
                return hayReservaActiva;
            }
        }

        // Metodo utilizado en el metodo ObtenerHabitacionesPorHotel de esta misma clase
        // Para saber si la habitación está actualmente reservada y mostrar el Estado = 'Reservada'
        public static bool HabitacionOcupada(string codHabitacion)
        {
            string query = @"
            SELECT 1
            FROM Reservaciones
            WHERE Cod_Habit = @CodHabitacion AND Fecha_Llegada <= @Fecha_Llegada AND Fecha_Salida >= @Fecha_Salida AND Estado != 'Eliminada'";

            SqlParameter[] parameters = {
            new SqlParameter("@CodHabitacion", codHabitacion),
            new SqlParameter("@Fecha_Llegada", DateTime.Now),
            new SqlParameter("@Fecha_Salida", DateTime.Now)
            };

            using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
            {
                bool ocupada = reader.Read();  // Retorna true si hay la habitación está ocupada, de lo contrario, false
                return ocupada;
            }
        }

        public static List<Habitacion> ObtenerHabitacionesPorHotel(int codHotel)
        {
            List<Habitacion> habitacionesDelHotel = new List<Habitacion>();
            string query = "SELECT Cod_Habit, Categ, Soleada, Lavado, Nevera, Precio, Cant_Pers, Estado " +
                "FROM Habitaciones WHERE Cod_Hotel = @CodHotel AND Estado != 'Eliminada'";
            SqlParameter[] parameters = { new SqlParameter("@CodHotel", codHotel) };

            using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
            {
                string estado = "";
                bool reservada = false;
                while (reader.Read())
                {
                    reservada =  HabitacionOcupada(reader["Cod_Habit"].ToString());
                    if (reservada)
                    {
                        estado = "Reservada";
                    }
                    else
                    {
                        estado = reader["Estado"] == DBNull.Value ? "" : reader["Estado"].ToString();
                    }

                    habitacionesDelHotel.Add(new Habitacion
                    {
                        CodHabit = reader["Cod_Habit"].ToString(),
                        CodHotel = codHotel,
                        Categ = reader["Categ"].ToString(),
                        Soleada = bool.Parse(reader["Soleada"].ToString()),
                        Lavado = bool.Parse(reader["Lavado"].ToString()),
                        Nevera = bool.Parse(reader["Nevera"].ToString()),
                        Precio = decimal.Parse(reader["Precio"].ToString()),
                        CantPers = int.Parse(reader["Cant_Pers"].ToString()),
                        Estado = estado
                    });
                }
            }

            return habitacionesDelHotel;
        }
    }
}
