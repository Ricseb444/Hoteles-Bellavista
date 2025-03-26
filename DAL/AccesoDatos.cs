using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Hoteles.UTL;

namespace Hoteles.DAL
{
    public class AccesoDatos
    {
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=Mis_hoteles;Integrated Security=True";
        private SqlConnection connection;
        //"Provider=SQLOLEDB;Data Source=.;Integrated Security=SSPI;Initial Catalog=Mis_hoteles"
        //"Provider=SQLNCLI11;Data Source=LAPTOP-SOL;Integrated Security=SSPI;Initial Catalog=Mis_hoteles"
        //"Data Source=localhost;Initial Catalog=Mis_hoteles;Integrated Security=True"

        public AccesoDatos()
        {
            // Constructor vacío si la cadena de conexión es fija y no necesita ser pasada como parámetro.
        }

        protected SqlConnection GetConnection()
        {
            // Retorna la conexión actual si existe, si no, crea una nueva.
            return connection ?? (connection = new SqlConnection(connectionString));
        }

        private void CloseConnection()
        {
            SimpleLogger.LogWarning("Intentando cerrar la conexión...");
            connection?.Close();
            connection = null;
            SimpleLogger.LogWarning("Conexión cerrada.");
        }

        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            int filasafectadas = 0;
            try
            {
                if (connection == null)
                {
                    SimpleLogger.LogWarning("Intentando crear conexión en NonQuery...");
                    connection = GetConnection();
                    SimpleLogger.LogWarning("Conexión creada en NonQuery.");
                }

                if (connection.State != ConnectionState.Open)
                {
                    SimpleLogger.LogWarning("Intentando abrir conexión en NonQuery...");
                    connection.Open();
                    SimpleLogger.LogWarning("Conexión abierta en NonQuery.");
                }

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    filasafectadas = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Escribimos en el log de errores
                SimpleLogger.LogError("Ocurrió un error al intentar procesar la solicitud", new Exception($"Detalle del error: {ex.Message}", ex));
                //SimpleLogger.LogInfo("Algún mensaje informativo");

            }
            finally
            {
                SimpleLogger.LogWarning("Intentando cerrar Conexión.");
                CloseConnection();
                SimpleLogger.LogWarning("Conexión cerrada.");
            }
            return filasafectadas; // Devuelve el número de filas afectadas
        }

        public SqlDataReader ExecuteReader(string query, SqlParameter[] parameters = null)
        {
            try
            {
                if (connection == null)
                {
                    SimpleLogger.LogWarning("Intentando crear conexión.");
                    connection = GetConnection();
                    SimpleLogger.LogWarning("Conexión creada.");
                }

                if (connection.State != ConnectionState.Open)
                {
                    SimpleLogger.LogWarning("Intentando abrir conexión...");
                    connection.Open();
                    SimpleLogger.LogWarning("Conexión abierta.");
                }

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    // Importante: no se debe cerrar la conexión a la base de datos después de ejecutar el método ExecuteReader.
                    // Es responsabilidad del código que llama a ExecuteReader manejar el cierre de la conexión.
                    // Esto permite que el código que recibe el OleDbDataReader pueda leer los datos antes de que la conexión se cierre.
                    return command.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                // Escribimos en el log de errores
                SimpleLogger.LogError("Ocurrió un error al intentar procesar la solicitud", new Exception($"Detalle del error: {ex.Message}", ex));
                //SimpleLogger.LogInfo("Algún mensaje informativo");
                throw;
            }
        }


        // El método ExecuteScalar es una función comúnmente utilizada en ADO.NET y otros marcos de trabajo de acceso a datos en .NET
        // para ejecutar consultas SQL que están diseñadas para retornar un único valor.
        // Esto es útil para operaciones que requieren obtener un solo dato de la base de datos, como un conteo de filas,
        // un valor máximo o mínimo, o el valor de una celda específica.
        public object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            object result = null;
            try
            {
                if (connection == null)
                {
                    connection = GetConnection();
                    connection.Open();
                }

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    result = command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                // Escribimos en el log de errores
                SimpleLogger.LogError("Ocurrió un error al intentar procesar la solicitud", new Exception($"Detalle del error: {ex.Message}", ex));
                //SimpleLogger.LogInfo("Algún mensaje informativo");
                throw; // Re-lanza la excepción para que pueda ser manejada por el llamador
            }
            finally
            {
                SimpleLogger.LogWarning("Intentando cerrar Conexión en ExecuteScalar.");
                CloseConnection();
                SimpleLogger.LogWarning("Conexión cerrada en ExecuteScalar.");
            }

            return result;
        }
    }
}
