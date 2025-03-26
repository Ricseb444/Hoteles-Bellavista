namespace Hoteles.DAL
{
    using System.Collections.Generic;
    using System.Data.OleDb;
    using System.Data.SqlClient;

    public class TelefonoHotelDAL : AccesoDatos
    {
        //******* La inserción, modificación y Eliminación se están realizando desde HotelDAL por medio de procedimientos almacenados ******

        public List<string> ObtenerTelefonosPorHotel(int cod_Hotel)
        {
            List<string> telefonosIDs = new List<string>();

            string query = "SELECT Numero FROM TelefonosHoteles WHERE Cod_Hotel = @CodHotel";
            SqlParameter[] parameters = {
                new SqlParameter("@CodHotel", cod_Hotel)
            };

            using (SqlDataReader reader = ExecuteReader(query, parameters))
            {
                while (reader.Read())
                {
                    telefonosIDs.Add(reader["Numero"].ToString());
                }
            }

            return telefonosIDs;
        }
    }
}
