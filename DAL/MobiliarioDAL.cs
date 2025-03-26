namespace Hoteles.DAL
{
    using Hoteles.DEL;
    using Hoteles.UTL;
    using System;
    using System.Collections.Generic;
    using System.Data.OleDb;
    using System.Data.SqlClient;

    public class MobiliarioDAL : AccesoDatos
    {
        public MobiliarioDAL() : base() { }

        // En este metodo tambien recibimos el parametro cantidad, el cual es para agilizar la inserción de mobiliario identico en la habitación.
        // Evitando así tener que incluir uno por uno, ya que son exactamente iguales. Por ejemplo 4 Almohadas.
        public bool Insertar(Mobiliario mobiliario, int cantidad)
        {
            string primeraPalabraDescripcion = mobiliario.Descripcion.Split(' ')[0];
            string claveBase = $"{mobiliario.CodHabit}-{primeraPalabraDescripcion.ToLower()}-";
            string queryConsecutivo = "SELECT MAX(Cod_Mobil) FROM Mobiliario WHERE Cod_Mobil LIKE @pattern";
            SqlParameter[] queryConsecutivoParams = new SqlParameter[] {
                new SqlParameter("@pattern", $"{claveBase}%")
            };
            string maxCodMobil = ExecuteScalar(queryConsecutivo, queryConsecutivoParams) as string;

            int consecutivo = 1;
            if (!string.IsNullOrEmpty(maxCodMobil))
            {
                int lastDashIndex = maxCodMobil.LastIndexOf('-');
                if (lastDashIndex != -1 && int.TryParse(maxCodMobil.Substring(lastDashIndex + 1), out int lastConsecutivo))
                {
                    consecutivo = lastConsecutivo + 1;
                }
            }

            try
            {
                for (int i = 0; i < cantidad; i++)
                {
                    string newCodMobil = $"{claveBase}{consecutivo + i}";  // Incrementa consecutivo para cada elemento
                    string query = "INSERT INTO Mobiliario (Cod_Mobil, Cod_Habit, Descripcion, Precio, Estado) " +
                        "VALUES (@CodMobil, @CodHabit, @Descripcion, @Precio, @Estado)";
                    SqlParameter[] parameters = {
                        new SqlParameter("@CodMobil", newCodMobil),
                        new SqlParameter("@CodHabit", mobiliario.CodHabit),
                        new SqlParameter("@Descripcion", mobiliario.Descripcion),
                        new SqlParameter("@Precio", mobiliario.Precio),
                        new SqlParameter("@Estado", "En uso")
                    };

                    if (ExecuteNonQuery(query, parameters) == 0)
                    {
                        throw new Exception("Failed to insert the mobiliario.");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Ocurrió un error al intentar insertar un mobiliario", new Exception($"Detalle del error: {ex.Message}", ex));
                return false;
            }
        }

        public bool Actualizar(Mobiliario mobiliario)
        {
            string query = "UPDATE Mobiliario SET " +
                "Cod_Habit = @CodHabit, " +
                "Descripcion = @Descripcion, " +
                "Precio = @Precio " +
                "WHERE Cod_Mobil = @CodMobil";
            SqlParameter[] parameters = {
                new SqlParameter("@CodHabit", mobiliario.CodHabit),
                new SqlParameter("@Descripcion", mobiliario.Descripcion),
                new SqlParameter("@Precio", mobiliario.Precio),
                new SqlParameter("@CodMobil", mobiliario.CodMobil)
            };

            try
            {
                int affectedRows = ExecuteNonQuery(query, parameters);
                return affectedRows > 0; // Devuelve true si se modificó al menos una fila.
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Ocurrió un error al intentar actualizar un mobiliario.", new Exception($"Detalle del error: {ex.Message}", ex));
                return false; // Devuelve false si ocurre un error.
            }
        }

        public bool Eliminar(string codMobil)
        {
            //string query = "DELETE FROM Mobiliario WHERE Cod_Mobil = ?";
            //OleDbParameter[] parameters = {
            //    new OleDbParameter("@CodMobil", codMobil)
            //};

            // No eliminamos el registro, solamente le cambiamos el estado.
            string query = "UPDATE Mobiliario SET Estado = @Estado WHERE Cod_Mobil = @CodMobil";
            SqlParameter[] parameters = {
                new SqlParameter("@Estado", "Eliminado"),
                new SqlParameter("@CodMobil", codMobil)
            };

            try
            {
                int affectedRows = ExecuteNonQuery(query, parameters);
                return affectedRows > 0; // Devuelve true si se modificó al menos una fila.
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Ocurrió un error al intentar eliminar un mobiliario.", 
                    new Exception($"Detalle del error: {ex.Message}", ex));
                return false;
            }
        }

        public static Mobiliario SeleccionarPorId(string codMobil)
        {
            string query = "SELECT Cod_Mobil, Cod_Habit, Descripcion, Precio FROM Mobiliario " +
                "WHERE Cod_Mobil = @CodMobil AND Estado != 'Eliminado'";
            SqlParameter[] parameters = {
                new SqlParameter("@CodMobil", codMobil)
            };

            using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
            {
                if (reader.Read())
                {
                    try
                    {
                        return new Mobiliario
                        {
                            CodMobil = reader["Cod_Mobil"] as string,
                            CodHabit = reader["Cod_Habit"] as string,
                            Descripcion = reader["Descripcion"] as string,
                            Precio = decimal.Parse(reader["Precio"].ToString())
                        };
                    }
                    catch (Exception ex)
                    {
                        SimpleLogger.LogError($"Ocurrió un error al intentar eliminar un mobiliario.", new Exception($"Detalle del error: {ex.Message}", ex));
                        throw;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<Mobiliario> SeleccionarMobiliarioPorHabitacionId(string codHabit)
        {
            List<Mobiliario> mobiliarios = new List<Mobiliario>();
            string query = "SELECT Cod_Mobil, Cod_Habit, Descripcion, Precio FROM Mobiliario " +
                "WHERE Cod_Habit = @CodHabit AND Estado != 'Eliminada'";
            SqlParameter[] parameters = {
                new SqlParameter("@CodHabit", codHabit)
            };

            using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
            {
                while (reader.Read())
                {
                    mobiliarios.Add(new Mobiliario
                    {
                        CodMobil = reader["Cod_Mobil"] as string,
                        CodHabit = reader["Cod_Habit"] as string,
                        Descripcion = reader["Descripcion"] as string,
                        Precio = decimal.Parse(reader["Precio"].ToString()),
                    });
                }
            }
            return mobiliarios;
        }

        public List<Mobiliario> SeleccionarPorDetalle(string DetalleMobiliario)
        {
            List<Mobiliario> mobiliarios = new List<Mobiliario>();
            if (string.IsNullOrWhiteSpace(DetalleMobiliario))
            {
                return null;
            }

            // Añadir condición para seleccionar mobiliarios por el detalle, además de estar activos
            string query = @"SELECT Cod_Mobil, Cod_Habit, Descripcion, Precio FROM Mobiliario 
                WHERE Descripcion LIKE @Detalle AND Estado != 'Eliminado'";

            string detalle = "%" + DetalleMobiliario + "%";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Detalle", detalle)
            };

            using (SqlDataReader reader = ExecuteReader(query, parameters))
            {
                while (reader.Read())
                {
                    Mobiliario mobiliario = new Mobiliario
                    {
                        CodMobil = reader["Cod_Mobil"] as string,
                        CodHabit = reader["Cod_Habit"] as string,
                        Descripcion = reader["Descripcion"] as string,
                        Precio = decimal.Parse(reader["Precio"].ToString())
                    };
                    mobiliarios.Add(mobiliario);
                }
            }

            return mobiliarios;
        }
    }
}
