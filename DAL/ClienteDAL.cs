namespace Hoteles.DAL
{
    using Hoteles.DEL;
    using Hoteles.UTL;
    using System;
    using System.Collections.Generic; // Para la clase genérica List<>
    using System.Data.SqlClient;

    public class ClienteDAL : AccesoDatos
    {
        // define un constructor para la clase ClienteDAL, que a su vez llama al constructor de la clase base
        // (en este caso, AccesoDatos) antes de ejecutar cualquier otro código dentro de su propio cuerpo del constructor.
        // Aunque en este caso el cuerpo del constructor está vacío, la llamada a base() es crucial si el constructor base
        // realiza alguna operación necesaria para que el objeto funcione correctamente.
        public ClienteDAL() : base() { }

        public bool Insertar(Cliente cliente)
        {
            if (cliente == null)
            {
                SimpleLogger.LogWarning("El cliente proporcionado es nulo.");
                return false;
            }

            string listaCodigosPais = "", listaTelefonosCliente = "";
            if (cliente.Telefonos != null && cliente.Telefonos.Count > 0)
            {
                foreach (var telefono in cliente.Telefonos)
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

            // sp_InsertarCliente es el procedimiento almacenado para insertar los clientes
            // y ya debe estar creado en SQL Server para poder llamarlo
            string queryCliente = "EXEC sp_InsertarCliente @IDCliente, @Nombre, @Apellido1, @Apellido2, @ListaCodigos, @ListaNumeros";
            SqlParameter[] parametersCliente = {
                new SqlParameter("@IDCliente", cliente.IDCliente),
                new SqlParameter("@Nombre", string.IsNullOrWhiteSpace(cliente.Nombre) ? "No indica" : cliente.Nombre),
                new SqlParameter("@Apellido1", string.IsNullOrWhiteSpace(cliente.Apellido1) ? "No indica" : cliente.Apellido1),
                new SqlParameter("@Apellido2", string.IsNullOrWhiteSpace(cliente.Apellido2) ? "No indica" : cliente.Apellido2),
                new SqlParameter("@ListaCodigos", listaCodigosPais),
                new SqlParameter("@ListaNumeros", listaTelefonosCliente) 
            };
            // El atributo Activo se ingresa directamente en SQL Server
            //parametersCliente[6] = new SqlParameter("@Activo", 1);

            try
            {
                int affectedRowsCliente = ExecuteNonQuery(queryCliente, parametersCliente);
                if (affectedRowsCliente > 0)
                {
                    return true;
                }
                else
                {
                    SimpleLogger.LogError($"No se pudo registrar el cliente {cliente.IDCliente}.", new Exception("Error al insertar el cliente."));
                    return false;
                }
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Error al registrar el cliente {cliente.IDCliente} y sus teléfonos: {ex.Message}", ex);
                return false;
            }
        }

        public bool Actualizar(Cliente cliente)
        {
            try
            {
                string listaCodigosPais = "", listaTelefonosCliente = "";
                if (cliente.Telefonos != null && cliente.Telefonos.Count > 0)
                {
                    foreach (var telefono in cliente.Telefonos)
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

                // sp_ActualizarCliente es el procedimiento almacenado para modificar los clientes
                // y ya debe estar creado en SQL Server para poder llamarlo
                string query = "EXEC sp_ActualizarCliente @IDCliente, @Nombre, @Apellido1, @Apellido2, @ListaCodigos, @ListaNumeros";
                SqlParameter[] parametersCliente = {
                    new SqlParameter("@IDCliente", cliente.IDCliente),
                    new SqlParameter("@Nombre", string.IsNullOrWhiteSpace(cliente.Nombre) ? "No indica" : cliente.Nombre),
                    new SqlParameter("@Apellido1", string.IsNullOrWhiteSpace(cliente.Apellido1) ? "No indica" : cliente.Apellido1),
                    new SqlParameter("@Apellido2", string.IsNullOrWhiteSpace(cliente.Apellido2) ? "No indica" : cliente.Apellido2),
                    new SqlParameter("@ListaCodigos", listaCodigosPais),
                    new SqlParameter("@ListaNumeros", listaTelefonosCliente)
                };
                // El atributo Activo se ingresa directamente en SQL Server
                //parametersCliente[6] = new OleDbParameter("@Activo", 1);

                int affectedRows = ExecuteNonQuery(query, parametersCliente);
                if (affectedRows <= 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                // Escribimos en el log de errores
                SimpleLogger.LogError($"Ocurrió un error al intentar actualizar el cliente: {cliente.IDCliente}", new Exception($"Detalle del error: {ex.Message}", ex));
                return false; // Devolver false si ocurrió un error
            }
        }

        // Al eliminar el Cliente no hace falta eliminar varias cosas, entre esas tenemos los telefonos de ese cliente,
        // por la relación de las llaves foraneas en CASCADE. Pero no voy a eliminar los registros, solo les voy a cambiar el estado.
        public bool Eliminar(int idCliente)
        {
            try
            {
                // Primeramente revisamos que el cliente no tenga reservas activas
                List<string> ReservasActivas = ReservacionDAL.ObtenerCodigosReservasActivas(idCliente);
                if (ReservasActivas.Count > 0)
                {
                    SimpleLogger.LogError("El huesped posee reservas activas.", new Exception("Error al eliminar el huesped. Posee reservaciones activas."));
                    return false;
                }
                else
                {
                    // Actualizar el estado del cliente a no activo, eliminar telefonos y cambiar estado de sus reservas
                    // sp_EliminarCliente es el procedimiento almacenado para eliminar los clientes
                    // y ya debe estar creado en SQL Server para poder llamarlo
                    string query = "EXEC sp_EliminarCliente @IDCliente";
                    SqlParameter[] parameters = { new SqlParameter("@IDCliente", idCliente) };

                    int filasAfectadas = ExecuteNonQuery(query, parameters);
                    if (filasAfectadas == 0)
                    {
                        // No se actualizó ningún registro, posiblemente porque el cliente no existe o ya estaba inactivo
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Escribimos en el log de errores
                SimpleLogger.LogError($"Ocurrió un error al intentar eliminar el cliente: {idCliente}", new Exception($"Detalle del error: {ex.Message}", ex));
                return false;
            }
        }

        public List<Cliente> SeleccionarPorNombre(string NombreCliente, string Apellido1Cliente, string Apellido2Cliente)
        {
            List<Cliente> clientes = new List<Cliente>();
            if (string.IsNullOrWhiteSpace(NombreCliente))
            {
                return null;
            }

            // Añadir condición para seleccionar clientes por nombre y apellidos, además de estar activos
            string queryClientes = @"SELECT ID_Cliente, Nombre, Apellido1, Apellido2 FROM Clientes 
                WHERE Nombre LIKE @Nombre AND Apellido1 LIKE @Apellido1 AND Apellido2 LIKE @Apellido2 AND Activo = 1";

            string nombre = "%" + NombreCliente + "%";
            string apellido1 = "%" + Apellido1Cliente + "%";
            string apellido2 = "%" + Apellido2Cliente + "%";

            SqlParameter[] parametersClientes = new SqlParameter[]
            {
                new SqlParameter("@Nombre", nombre),
                new SqlParameter("@Apellido1", apellido1),
                new SqlParameter("@Apellido2", apellido2)
            };

            using (SqlDataReader readerClientes = ExecuteReader(queryClientes, parametersClientes))
            {
                while (readerClientes.Read())
                {
                    Cliente cliente = new Cliente
                    {
                        IDCliente = readerClientes["ID_Cliente"] == DBNull.Value ? 0 : int.Parse(readerClientes["ID_Cliente"].ToString()),
                        Nombre = readerClientes["Nombre"].ToString(),
                        Apellido1 = readerClientes["Apellido1"].ToString(),
                        Apellido2 = readerClientes.IsDBNull(readerClientes.GetOrdinal("Apellido2")) ? null : readerClientes["Apellido2"].ToString(),
                    };
                    clientes.Add(cliente);
                }
            }

            // Obtener teléfonos para cada cliente activo
            foreach (var cliente in clientes)
            {
                string queryTelefonos = "SELECT Codigo_Pais, Numero FROM TelefonosClientes WHERE ID_Cliente = @IDCliente";
                SqlParameter[] parametersTelefonos = { new SqlParameter("@IDCliente", cliente.IDCliente) };

                using (SqlDataReader readerTelefonos = ExecuteReader(queryTelefonos, parametersTelefonos))
                {
                    while (readerTelefonos.Read())
                    {
                        TelefonoCliente telefono = new TelefonoCliente
                        {
                            CodigoPais = readerTelefonos["Codigo_Pais"] == DBNull.Value ? 0 : int.Parse(readerTelefonos["Codigo_Pais"].ToString()),
                            Numero = readerTelefonos["Numero"].ToString()
                        };
                        cliente.Telefonos.Add(telefono);
                    }
                }
            }
            return clientes;
        }

        public List<Cliente> SeleccionarPorNombre(string NombreCliente)
        {
            List<Cliente> clientes = new List<Cliente>();
            if (string.IsNullOrWhiteSpace(NombreCliente))
            {
                return null;
            }

            // Añadir condición para seleccionar clientes por nombre y apellidos, además de estar activos
            string queryClientes = @"SELECT ID_Cliente, Nombre, Apellido1, Apellido2 FROM Clientes 
                WHERE Nombre LIKE @Nombre AND Activo = 1";

            string nombre = "%" + NombreCliente + "%";

            SqlParameter[] parametersClientes = new SqlParameter[]
            {
                new SqlParameter("@Nombre", nombre)
            };

            using (SqlDataReader readerClientes = ExecuteReader(queryClientes, parametersClientes)) // Esto no esta trayendo los telefonos
            {
                while (readerClientes.Read())
                {
                    Cliente cliente = new Cliente
                    {
                        IDCliente = readerClientes["ID_Cliente"] == DBNull.Value ? 0 : int.Parse(readerClientes["ID_Cliente"].ToString()),
                        Nombre = readerClientes["Nombre"].ToString(),
                        Apellido1 = readerClientes["Apellido1"].ToString(),
                        Apellido2 = readerClientes.IsDBNull(readerClientes.GetOrdinal("Apellido2")) ? null : readerClientes["Apellido2"].ToString(),
                    };
                    clientes.Add(cliente);
                }
            }

            // Obtener teléfonos para cada cliente activo
            foreach (var cliente in clientes)
            {
                string queryTelefonos = "SELECT Codigo_Pais, Numero FROM TelefonosClientes WHERE ID_Cliente = @IDCliente";
                SqlParameter[] parametersTelefonos = { new SqlParameter("@IDCliente", cliente.IDCliente) };

                using (SqlDataReader readerTelefonos = ExecuteReader(queryTelefonos, parametersTelefonos))
                {
                    while (readerTelefonos.Read())
                    {
                        TelefonoCliente telefono = new TelefonoCliente
                        {
                            CodigoPais = readerTelefonos["Codigo_Pais"] == DBNull.Value ? 0 : int.Parse(readerTelefonos["Codigo_Pais"].ToString()),
                            Numero = readerTelefonos["Numero"].ToString()
                        };
                        cliente.Telefonos.Add(telefono);
                    }
                }
            }

            return clientes;
        }

        public Cliente SeleccionarPorId(int id)
        {
            // Revisamos si tiene reservas, para mandar este valor al objeto, así si van a eliminar este cliente, podremos dar un mensaje mas especifico.
            // Por ejemplo, el huesped posee reservas activas, realmente desea eliminarlo? Este valor lo enviaremos en el atributo Activo, para aprovechar su campo.
            bool estado = true;
            List<string> listaReservasActivas = ReservacionDAL.ObtenerCodigosReservasActivas(id);
            if (listaReservasActivas == null || listaReservasActivas.Count == 0)
            {
                estado = false;
            }

            string queryCliente = "SELECT ID_Cliente, Nombre, Apellido1, Apellido2 FROM Clientes WHERE ID_Cliente = @IDCliente AND Activo = 1";
            SqlParameter[] parametersCliente = { new SqlParameter("@IDCliente", id) };

            Cliente cliente = null;

            using (SqlDataReader readerCliente = ExecuteReader(queryCliente, parametersCliente))
            {
                if (readerCliente.Read())
                {
                    try
                    {
                        cliente = new Cliente
                        {
                            IDCliente = readerCliente["ID_Cliente"] == DBNull.Value ? 0 : int.Parse(readerCliente["ID_Cliente"].ToString()),
                            Nombre = readerCliente["Nombre"].ToString(),
                            Apellido1 = readerCliente["Apellido1"].ToString(),
                            Apellido2 = readerCliente.IsDBNull(readerCliente.GetOrdinal("Apellido2")) ? null : readerCliente["Apellido2"].ToString(),
                            Activo = estado,
                            Telefonos = new List<TelefonoCliente>() // Inicializa la lista de teléfonos para el cliente
                        };
                    }
                    catch (Exception ex)
                    {
                        SimpleLogger.LogError($"Error en la consulta del cliente:", ex);
                        throw;
                    }
                }
            }

            if (cliente != null)
            {
                string queryTelefonos = "SELECT Codigo_Pais, Numero FROM TelefonosClientes WHERE ID_Cliente = @IDCliente";
                SqlParameter[] parametersTelefonos = { new SqlParameter("@IDCliente", id) };

                using (SqlDataReader readerTelefonos = ExecuteReader(queryTelefonos, parametersTelefonos))
                {
                    while (readerTelefonos.Read())
                    {
                        TelefonoCliente telefono = new TelefonoCliente
                        {
                            CodigoPais = readerTelefonos["Codigo_Pais"] == DBNull.Value ? 0 : int.Parse(readerTelefonos["Codigo_Pais"].ToString()),
                            Numero = readerTelefonos["Numero"].ToString()
                        };
                        cliente.Telefonos.Add(telefono);
                    }
                }
            }

            return cliente;
        }

        public List<Cliente> ListarTodos()
        {
            List<Cliente> clientes = new List<Cliente>();
            string query = "SELECT ID_Cliente, Nombre, Apellido1, Apellido2 FROM Clientes WHERE Activo = 1";

            using (SqlDataReader reader = ExecuteReader(query))
            {
                while (reader.Read())
                {
                    Cliente cliente = new Cliente
                    {
                        IDCliente = int.Parse(reader["ID_Cliente"].ToString()),
                        Nombre = reader["Nombre"].ToString(),
                        Apellido1 = reader["Apellido1"].ToString(),
                        Apellido2 = reader["Apellido2"].ToString(),
                    };

                    string queryTelefonos = "SELECT CodigoPais, Numero FROM TelefonosClientes WHERE ID_Cliente = @IDCliente";
                    SqlParameter[] parametersTelefonos = { new SqlParameter("@IDCliente", cliente.IDCliente) };

                    using (SqlDataReader readerTelefonos = ExecuteReader(queryTelefonos, parametersTelefonos))
                    {
                        while (readerTelefonos.Read())
                        {
                            TelefonoCliente telefono = new TelefonoCliente
                            {
                                CodigoPais = int.Parse(readerTelefonos["CodigoPais"].ToString()),
                                Numero = readerTelefonos["Numero"].ToString()
                            };
                            cliente.Telefonos.Add(telefono);
                        }
                    }

                    clientes.Add(cliente);
                }
            }
            return clientes;
        }

        // Metodo utilizado en Modificar (ReservacionDAL) para saber si el nuevo ID_Cliente realmente existe.
        public bool ExistePorId(int id)
        {
            try
            {
                string queryCliente = "SELECT ID_Cliente FROM Clientes WHERE ID_Cliente = @IDCliente AND Activo = 1";
                SqlParameter[] parametersCliente = { new SqlParameter("@IDCliente", id) };

                using (SqlDataReader readerCliente = ExecuteReader(queryCliente, parametersCliente))
                {
                    return readerCliente.Read();  // Si puede leer, retorna true; de lo contrario, false
                }
            }
            catch (Exception ex)
            {
                SimpleLogger.LogError($"Error en a la hora de consultar cliente en modificar reservación:", ex);
                return false;
            }
        }
    }
}
