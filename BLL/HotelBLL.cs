using Hoteles.DAL;
using Hoteles.DEL;
using System;
using System.Collections.Generic; //  Para la clase genérica List<>

namespace Hoteles.BLL
{
    public class HotelBLL
    {
        private HotelDAL hotelDAL;
        public HotelBLL()
        {
            hotelDAL = new HotelDAL();
        }

        public ResultadoOperacion InsertarHotel(Hotel hotel)
        {
            if (hotel == null)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El objeto hotel es nulo." };
            }

            if (string.IsNullOrEmpty(hotel.DPais) || string.IsNullOrWhiteSpace(hotel.DPais) ||
                string.IsNullOrEmpty(hotel.DProvincia) || string.IsNullOrWhiteSpace(hotel.DProvincia) ||
                string.IsNullOrEmpty(hotel.DCanton) || string.IsNullOrWhiteSpace(hotel.DCanton) ||
                string.IsNullOrEmpty(hotel.DExacta) || string.IsNullOrWhiteSpace(hotel.DExacta))
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "Todos los campos del hotel deben estar completos." };
            }

            try
            {
                hotelDAL.Insertar(hotel);
                return new ResultadoOperacion { Exito = true, Mensaje = "Hotel insertado correctamente." };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"Error al insertar el hotel: {ex.Message}" };
            }
        }

        public ResultadoOperacion ActualizarHotel(Hotel hotel)
        {
            if (hotel == null)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El objeto hotel es nulo." };
            }

            if (string.IsNullOrEmpty(hotel.DPais) || string.IsNullOrWhiteSpace(hotel.DPais) ||
                string.IsNullOrEmpty(hotel.DProvincia) || string.IsNullOrWhiteSpace(hotel.DProvincia) ||
                string.IsNullOrEmpty(hotel.DCanton) || string.IsNullOrWhiteSpace(hotel.DCanton) ||
                string.IsNullOrEmpty(hotel.DExacta) || string.IsNullOrWhiteSpace(hotel.DExacta))
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "Todos los campos obligatorios deben estar completos." };
            }

            try
            {
                hotelDAL.Actualizar(hotel);
                return new ResultadoOperacion { Exito = true, Mensaje = "Hotel actualizado correctamente." };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"Error al actualizar el hotel: {ex.Message}" };
            }
        }

        public ResultadoOperacion EliminarHotel(int codHotel)
        {
            if (codHotel <= 0)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El código de hotel no es válido." };
            }

            if (ReservacionDAL.ExistenReservacionesActivasParaHotel(codHotel))
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "No se puede eliminar el hotel porque existen reservaciones activas para algunas habitaciones." };
            }

            try
            {
                hotelDAL.Eliminar(codHotel);
                return new ResultadoOperacion { Exito = true, Mensaje = "Hotel eliminado correctamente." };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"Error al eliminar el hotel: {ex.Message}" };
            }
        }


        public Hotel seleccionarPorId(int codHotel)
        {
            if (codHotel <= 0)
            {
                throw new ArgumentException("El ID está vacio.", nameof(codHotel));
            }

            try
            {
                return HotelDAL.SeleccionarPorId(codHotel);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"No se pudo seleccionar el Hotel: {ex.Message}", nameof(codHotel));//Modificación: Cliente --> Hotel
            }//Mod.2: Añadí el ex.Message
        }

        public List<Hotel> ListarTodosLosHoteles()
        {
            return hotelDAL.ListarTodos();
        }
    }
}
