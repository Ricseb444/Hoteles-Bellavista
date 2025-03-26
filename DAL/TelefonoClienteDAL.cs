namespace Hoteles.DAL
{
    using Hoteles.DEL;
    using Hoteles.UTL;
    using System;
    using System.Collections.Generic;
    using System.Data.OleDb;
    using System.Data.SqlClient;

    public class TelefonoClienteDAL : AccesoDatos
    {

        //******* La inserción, modificación y Eliminación se están realizando desde ClienteDAL por medio de procedimientos almacenados ******

        public List<string> ObtenerTelefonosPorCliente(int idCliente)
        {
            List<string> telefonos = new List<string>();

            string query = "SELECT Numero FROM TelefonosClientes WHERE ID_Cliente = @IDCliente";
            SqlParameter[] parameters = {
                new SqlParameter("@IDCliente", idCliente)
            };

            using (SqlDataReader reader = ExecuteReader(query, parameters))
            {
                while (reader.Read())
                {
                    telefonos.Add(reader["Numero"].ToString());
                }
            }

            return telefonos;
        }
    }
}
