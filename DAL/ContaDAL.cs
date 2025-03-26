using Hoteles.DAL;
using Hoteles.UTL;
using System;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace DAL
{
    public class ContaDAL : AccesoDatos
    {
        public ContaDAL() : base() { }

        // Método para calcular el total de tarjeta y efectivo en un rango de fechas, devolviendo una tupla (2 resultados)
        public static (decimal TotalTarjeta, decimal TotalEfectivo) CalcularTotales(DateTime fechaInicio, DateTime fechaFinal)
        {
            try
            {
                // Inicializar los totales
                decimal totalTarjeta = 0;
                decimal totalEfectivo = 0;

                // Consulta SQL para obtener el total de tarjeta y efectivo en el rango de fechas
                string query = "SELECT SUM(MontoTarjeta) AS TotalTarjeta, SUM(MontoEfectivo) AS TotalEfectivo FROM Reservaciones " +
                    "WHERE (Fecha_Llegada BETWEEN @FechaInicio AND @FechaFinal) AND Estado != 'Eliminada'";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FechaInicio", fechaInicio),
                    new SqlParameter("@FechaFinal", fechaFinal)
                };

                using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
                {
                    // Comprueba si se encontraron filas
                    if (reader.Read())
                    {
                        // Obtener los valores de suma de tarjeta y efectivo
                        if (reader["TotalTarjeta"] != DBNull.Value)
                        {
                            totalTarjeta = (decimal)reader["TotalTarjeta"];
                        }

                        if (reader["TotalEfectivo"] != DBNull.Value)
                        {
                            totalEfectivo = (decimal)reader["TotalEfectivo"];
                        }
                    }
                }

                // Retornar una tupla con los totales de tarjeta y efectivo
                return (totalTarjeta, totalEfectivo);
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Error al ejecutar el cálculo.", ex);
                throw; // Relanzar la excepción para que el cliente pueda manejarla si es necesario
            }
        }

        // Método recargado para agregar el codigo del hotel en la búsqueda.
        public static (decimal TotalTarjeta, decimal TotalEfectivo) CalcularTotales(DateTime fechaInicio, DateTime fechaFinal, int cod_Hotel)
        {
            try
            {
                // Inicializar los totales
                decimal totalTarjeta = 0;
                decimal totalEfectivo = 0;

                // Consulta SQL para obtener el total de tarjeta y efectivo en el rango de fechas
                string query = @"SELECT SUM(MontoTarjeta) AS TotalTarjeta, SUM(MontoEfectivo) AS TotalEfectivo FROM Reservaciones 
                            WHERE (Fecha_Llegada BETWEEN @FechaInicio AND @FechaFinal) AND
                            Cod_Habit IN (SELECT Cod_Habit FROM Habitaciones WHERE Cod_Hotel = @CodHotel) AND Estado != 'Eliminada'";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FechaInicio", fechaInicio),
                    new SqlParameter("@FechaFinal", fechaFinal),
                    new SqlParameter("@CodHotel", cod_Hotel)
                };

                using (SqlDataReader reader = new AccesoDatos().ExecuteReader(query, parameters))
                {
                    // Comprueba si se encontraron filas
                    if (reader.Read())
                    {
                        // Obtener los valores de suma de tarjeta y efectivo
                        if (reader["TotalTarjeta"] != DBNull.Value)
                        {
                            totalTarjeta = (decimal)reader["TotalTarjeta"];
                        }

                        if (reader["TotalEfectivo"] != DBNull.Value)
                        {
                            totalEfectivo = (decimal)reader["TotalEfectivo"];
                        }
                    }
                }

                // Retornar una tupla con los totales de tarjeta y efectivo
                return (totalTarjeta, totalEfectivo);
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Error al ejecutar el cálculo.", ex);
                throw; // Relanzar la excepción para que el cliente pueda manejarla si es necesario
            }
        }
    }
}
