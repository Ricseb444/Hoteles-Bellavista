namespace Hoteles.DAL
{
    using Hoteles.DEL;
    using Hoteles.UTL;
    using System;
    using System.Collections.Generic;
    using System.Data.OleDb;
    using System.Data.SqlClient;

    public class HotelDAL : AccesoDatos
    {
        public HotelDAL() : base() { }

        public bool Insertar(Hotel hotel)
        {
            if (hotel == null)
            {
                SimpleLogger.LogError("El hotel proporcionado es nulo.", new Exception("Error al insertar el hotel."));
                return false;
            }

            string listaCodigosPais = "", listaTelefonosCliente = "";
            if (hotel.Telefonos != null && hotel.Telefonos.Count > 0)
            {
                foreach (var telefono in hotel.Telefonos)
                {
                    listaCodigosPais += telefono.CodigoPais.ToString() + ",";
                    listaTelefonosCliente += telefono.Numero.ToString() + ",";
                }

                // Eliminar la última coma
                if (!string.IsNullOrEmpty(listaCodigosPais))
                {
                    listaCodigosPais = listaCodigosPais.TrimEnd(',');
                }

                if (!string.IsNullOrEmpty(listaTelefonosCliente))
                {
                    listaTelefonosCliente = listaTelefonosCliente.TrimEnd(',');
                }
            }

            string queryHotel = "EXEC sp_InsertarHotel @DPais, @DProvincia, @DCanton, @DExacta, @ListaCodigos, @ListaNumeros";
            SqlParameter[] parametersHotel = {
                new SqlParameter("@DPais", hotel.DPais),
                new SqlParameter("@DProvincia", hotel.DProvincia),
                new SqlParameter("@DCanton", hotel.DCanton),
                new SqlParameter("@DExacta", hotel.DExacta),
                new SqlParameter("@ListaCodigos", listaCodigosPais),
                new SqlParameter("@ListaNumeros", listaTelefonosCliente)
            };

            try
            {
                int affectedRowsHotel = ExecuteNonQuery(queryHotel, parametersHotel);
                if (affectedRowsHotel > 0)
                {
                    return true;
                }
                else
                {
                    SimpleLogger.LogError($"No se pudo registrar el hotel.", new Exception("Error al insertar el hotel."));
                    return false;
                }
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Ocurrió un error al intentar insertar un hotel.", new Exception($"Detalle del error: {ex.Message}", ex));
                return false;
            }
        }

        public bool Actualizar(Hotel hotel)
        {
            try
            {
                string listaCodigosPais = "", listaTelefonosCliente = "";
                if (hotel.Telefonos != null && hotel.Telefonos.Count > 0)
                {
                    foreach (var telefono in hotel.Telefonos)
                    {
                        listaCodigosPais += telefono.CodigoPais.ToString() + ",";
                        listaTelefonosCliente += telefono.Numero.ToString() + ",";
                    }

                    // Eliminar la última coma
                    if (!string.IsNullOrEmpty(listaCodigosPais))
                    {
                        listaCodigosPais = listaCodigosPais.TrimEnd(',');
                    }

                    if (!string.IsNullOrEmpty(listaTelefonosCliente))
                    {
                        listaTelefonosCliente = listaTelefonosCliente.TrimEnd(',');
                    }
                }

                string query = "EXEC sp_ActualizarHotel  @CodHotel, @DPais, @DProvincia, @DCanton, @DExacta, @ListaCodigos, @ListaNumeros";
                SqlParameter[] parameters = {
                    new SqlParameter("@CodHotel", hotel.CodHotel),
                    new SqlParameter("@DPais", hotel.DPais),
                    new SqlParameter("@DProvincia", hotel.DProvincia),
                    new SqlParameter("@DCanton", hotel.DCanton),
                    new SqlParameter("@DExacta", hotel.DExacta),
                    new SqlParameter("@ListaCodigos", listaCodigosPais),
                    new SqlParameter("@ListaNumeros", listaTelefonosCliente)
                };
                int filasAfectadas = ExecuteNonQuery(query, parameters);
                if (filasAfectadas == 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Ocurrió un error al intentar actualizar el hotel {hotel.CodHotel}", new Exception($"Detalle del error: {ex.Message}", ex));
                return false;
            }
        }

        // Al eliminar el Hotel no hace falta eliminar varias cosas, entre esas tenemos las habitaciones,
        // porque en la BD en la Tabla Habitaciones, la Cod_Hotel es llave foranea en CASCADE de Cod_Hotel en la tabla Hoteles.
        // Por lo que al eliminar el Hotel se eliminaran tambien las habitaciones de ese hotel.
        // Lo mismo sucede con los telefonos de ese Hotel.
        // Lo mismo sucede con las reservaciones de ese Hotel.
        // Pero actualmente no estamos eliminando realmente, solamente cambiamos el estado del hotel.
        public bool Eliminar(int codHotel)
        {
            // No eliminamos el registro, unicamente le cambiamos el estado
            string query = "EXEC sp_EliminarHotel  @CodHotel";
            SqlParameter[] parameters = {
                    new SqlParameter("@CodHotel", codHotel)
                };
            try
            {
                int filasAfectadas = ExecuteNonQuery(query, parameters);
                return filasAfectadas > 0; // Retorna true si se afectó al menos una fila, indicando éxito en la inserción.
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Ocurrió un error al intentar eliminar el hotel: {codHotel}", new Exception($"Detalle del error: {ex.Message}", ex));
                return false;
            }
        }

        public static Hotel SeleccionarPorId(int codHotel)
        {
            string queryHotel = "SELECT Cod_Hotel, D_Pais, D_Provincia, D_Canton, D_Exacta FROM Hoteles WHERE Cod_Hotel = @CodHotel AND Activo = 1";
            SqlParameter[] parametersHotel = { new SqlParameter("@CodHotel", codHotel) };

            Hotel hotel = null;

            using (SqlDataReader readerHotel = new AccesoDatos().ExecuteReader(queryHotel, parametersHotel))//Modificación: trycatch
            {
                if (readerHotel.Read())
                {
                    try
                    {
                        hotel = new Hotel
                        {
                            CodHotel = int.Parse(readerHotel["Cod_Hotel"].ToString()),
                            DPais = readerHotel["D_Pais"].ToString(),
                            DProvincia = readerHotel["D_Provincia"].ToString(),
                            DCanton = readerHotel["D_Canton"].ToString(),
                            DExacta = readerHotel["D_Exacta"].ToString(),
                            Telefonos = new List<TelefonoHotel>() // Inicializa la lista de teléfonos para el hotel
                        };
                    }
                    catch (Exception ex)
                    {
                        SimpleLogger.LogError($"Ocurrió un error al intentar consultar el hotel.", new Exception($"Detalle del error: {ex.Message}", ex));
                        throw;
                    }

                }
            }

            if (hotel != null)
            {
                string queryTelefonos = "SELECT Cod_Hotel, Codigo_Pais, Numero FROM TelefonosHoteles WHERE Cod_Hotel = @CodHotel";//Corrección 
                SqlParameter[] parametersTelefonos = { new SqlParameter("@CodHotel", codHotel) }; //Abrir nuevo SqlParameter para teléfonos

                using (SqlDataReader readerTelefonos = new AccesoDatos().ExecuteReader(queryTelefonos, parametersTelefonos))
                {
                    while (readerTelefonos.Read())
                    {
                        hotel.Telefonos.Add(new TelefonoHotel
                        {
                            CodHotel = int.Parse(readerTelefonos["Cod_Hotel"].ToString()),
                            CodigoPais = int.Parse(readerTelefonos["Codigo_Pais"].ToString()),
                            Numero = readerTelefonos["Numero"].ToString()
                        });
                    }
                }
            }
            
            return hotel;
        }

        public List<Hotel> ListarTodos()
        {
            List<Hotel> hoteles = new List<Hotel>();
            string query = "SELECT Cod_Hotel, D_Pais, D_Provincia, D_Canton, D_Exacta FROM Hoteles WHERE Activo = 1";

            using (SqlDataReader reader = ExecuteReader(query))
            {
                while (reader.Read())
                {
                    Hotel hotel = new Hotel
                    {
                        CodHotel = int.Parse(reader["Cod_Hotel"].ToString()),
                        DPais = reader["D_Pais"].ToString(),
                        DProvincia = reader["D_Provincia"].ToString(),
                        DCanton = reader["D_Canton"].ToString(),
                        DExacta = reader["D_Exacta"].ToString(),
                        Telefonos = new List<TelefonoHotel>() // Inicializa la lista de teléfonos para el hotel
                    };

                    hoteles.Add(hotel);
                }
            }

            return hoteles;
        }
    }
}
