using Hoteles.DAL;
using Hoteles.DEL;
using System;
using System.Collections.Generic;
using System.Linq; // Para el metodo Any()

namespace Hoteles.BLL
{
    public class ClienteBLL
    {
        private ClienteDAL clienteDAL;
        private ReservacionDAL reservacionDAL;

        public ClienteBLL()
        {
            reservacionDAL = new ReservacionDAL();
            clienteDAL = new ClienteDAL();
        }

        public ResultadoOperacion InsertarCliente(Cliente cliente)
        {
            if (cliente == null)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El cliente no puede ser null." };
            }

            if (cliente.IDCliente <= 0)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El ID del cliente debe ser un número positivo y no puede estar vacío." };
            }

            var clienteExistente = clienteDAL.SeleccionarPorId(cliente.IDCliente);
            if (clienteExistente != null)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "Ya existe un cliente con el ID proporcionado." };
            }

            try
            {
                bool resultado = clienteDAL.Insertar(cliente);
                if (!resultado)
                {
                    return new ResultadoOperacion { Exito = false, Mensaje = "Error al insertar el cliente. Por favor, verifique los datos e intente nuevamente." };
                }
                return new ResultadoOperacion { Exito = true, Mensaje = "Cliente insertado con éxito." };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"Error al insertar el cliente: {ex.Message}" };
            }
        }

        public ResultadoOperacion ModificarCliente(Cliente cliente)
        {
            if (cliente == null)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El cliente no puede ser null." };
            }

            if (cliente.IDCliente <= 0)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El ID del cliente debe ser un número positivo y no puede estar vacío." };
            }

            var clienteExistente = clienteDAL.SeleccionarPorId(cliente.IDCliente);
            if (clienteExistente == null)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "No existe un cliente con el ID proporcionado." };
            }

            try
            {
                clienteDAL.Actualizar(cliente);
                return new ResultadoOperacion { Exito = true, Mensaje = "Cliente actualizado con éxito." };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "Error al actualizar el cliente: " + ex.Message };
            }
        }

        public ResultadoOperacion EliminarCliente(int idCliente)
        {
            var codigosReservasActivas = ReservacionDAL.ObtenerCodigosReservasActivas(idCliente);
            if (codigosReservasActivas.Any())
            {
                // Si existen reservas activas, no se puede eliminar el cliente
                return new ResultadoOperacion
                {
                    Exito = false,
                    Mensaje = "No se puede eliminar el cliente porque posee reservaciones activas."
                };
            }
            else
            {
                try
                {
                    // No hay reservas activas, proceder con la eliminación del cliente
                    clienteDAL.Eliminar(idCliente);
                    return new ResultadoOperacion
                    {
                        Exito = true,
                        Mensaje = "El cliente ha sido eliminado exitosamente."
                    };
                }
                catch (Exception ex)
                {
                    // Manejar cualquier excepción que pueda ocurrir durante la eliminación del cliente
                    return new ResultadoOperacion
                    {
                        Exito = false,
                        Mensaje = $"Error al eliminar el cliente: {ex.Message}"
                    };
                }
            }
        }

        // Para aprovechar estos mensajes del throw, debemos capturarlos con un try catch en el metodo que llamó a este
        public Cliente seleccionarPorId(int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException("El ID está vacio.", nameof(idCliente));
            }

            try
            {
                return clienteDAL.SeleccionarPorId(idCliente);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"No se pudo seleccionar el Cliente.{ex.Message}", nameof(idCliente));
            }
        }

        public List<Cliente> seleccionarPorNombre(String NombreCliente, String Apellido1Cliente, String Apellido2Cliente)
        {
            if (string.IsNullOrWhiteSpace(NombreCliente) && string.IsNullOrWhiteSpace(Apellido1Cliente) && string.IsNullOrWhiteSpace(Apellido2Cliente))
            {
                throw new ArgumentException("No indicó Nombre ni Apellidos.", nameof(NombreCliente));
            }

            try
            {
                return clienteDAL.SeleccionarPorNombre(NombreCliente, Apellido1Cliente, Apellido2Cliente);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"No se pudo seleccionar el Cliente: {ex.Message}", nameof(NombreCliente));
            }
        }

        public List<Cliente> seleccionarPorNombre(String NombreCliente)
        {
            if (string.IsNullOrWhiteSpace(NombreCliente))
            {
                throw new ArgumentException("No indicó Nombre.", nameof(NombreCliente));
            }

            try
            {
                return clienteDAL.SeleccionarPorNombre(NombreCliente);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"No se pudo seleccionar el Cliente: {ex.Message}", nameof(NombreCliente));
            }
        }

        public List<Cliente> ListarTodosLosClientes()
        {
            return clienteDAL.ListarTodos();
        }


        public List<Reservacion> VerHistorialReservasPorClienteID(int idCliente)
        {
            return ReservacionDAL.ObtenerHistorialReservacionesPorCliente(idCliente);
        }

        public bool ExistePorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID no es válido.", nameof(id));
            }

            try
            {
                return clienteDAL.ExistePorId(id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error en la busqueda del nuevo huesped.{ex.Message}", nameof(id));
            }
        }

    }
}
