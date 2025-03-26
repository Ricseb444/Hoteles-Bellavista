using Hoteles.DAL;
using Hoteles.DEL;
using System;
using System.Collections.Generic;

namespace Hoteles.BLL
{
    public class HabitacionBLL
    {
        private HabitacionDAL habitacionDAL;
        public HabitacionBLL()
        {
            habitacionDAL = new HabitacionDAL();
        }

        public ResultadoOperacion InsertarHabitacion(Habitacion habitacion)
        {
            if (habitacion == null)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El objeto habitación es nulo." };
            }

            if (string.IsNullOrEmpty(habitacion.Categ) ||
                string.IsNullOrWhiteSpace(habitacion.Categ) ||
                !(habitacion.Categ.Equals("Standard", StringComparison.OrdinalIgnoreCase) ||
                  habitacion.Categ.Equals("Junior", StringComparison.OrdinalIgnoreCase) ||
                  habitacion.Categ.Equals("Premier", StringComparison.OrdinalIgnoreCase)))
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "La categoría proporcionada no es válida. Debe ser 'Stándard', 'Junior' o 'Premier'." };
            }

            if (habitacion.Precio <= 0)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El precio debe ser mayor que cero." };
            }

            if (habitacion.CodHotel <= 0)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El código del hotel debe ser un número positivo." };
            }

            if (habitacion.CantPers <= 0)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "La cantidad de personas debe ser mayor que cero." };
            }

            if (HotelDAL.SeleccionarPorId(habitacion.CodHotel) == null)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"ERROR! El hotel ingresado no existe." };
            }

            // habitacion.CodHabit NO SE EVALUA ACÁ PORQUE SE CREA AUTOMÁTICAMENTE EN EL METODO DE INSERTAR

            try
            {
                habitacionDAL.Insertar(habitacion);
                return new ResultadoOperacion { Exito = true, Mensaje = "Habitación insertada correctamente." };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"Error al insertar la habitación: {ex.Message}" };
            }
        }

        public ResultadoOperacion ActualizarHabitacion(Habitacion habitacion)
        {
            if (habitacion == null)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El objeto habitación es nulo." };
            }

            if (string.IsNullOrEmpty(habitacion.Categ) ||
                string.IsNullOrWhiteSpace(habitacion.Categ) ||
                !(habitacion.Categ.Equals("Standard", StringComparison.OrdinalIgnoreCase) ||
                  habitacion.Categ.Equals("Junior", StringComparison.OrdinalIgnoreCase) ||
                  habitacion.Categ.Equals("Premier", StringComparison.OrdinalIgnoreCase)))
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "La categoría proporcionada no es válida. Debe ser 'Stándard', 'Junior' o 'Premier'." };
            }

            if (habitacion.Precio <= 0)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El precio debe ser mayor que cero." };
            }

            if (habitacion.CantPers <= 0)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "La cantidad de personas debe ser mayor que cero." };
            }

            try
            {
                habitacionDAL.Actualizar(habitacion);
                return new ResultadoOperacion { Exito = true, Mensaje = "Habitación actualizada correctamente." };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"Error al actualizar la habitación: {ex.Message}" };
            }
        }


        public bool EliminarSiEstaLibre(string codHabitacion)
        {
            // Chequeamos que ese codigo de Habitación no exista
            Habitacion existente = habitacionDAL.SeleccionarPorId(codHabitacion);
            if (existente == null)
            {
                return false; // No existe esa habitación
            }

            // Verificar si la habitación está actualmente reservada
            if (habitacionDAL.ExistenReservasActivasParaHabitacion(codHabitacion))
            {
                // La habitación está reservada, no se puede eliminar
                return false;
            }
            else
            {
                // La habitación no está reservada, se puede eliminar
                habitacionDAL.Eliminar(codHabitacion);
                return true;
            }
        }

        public bool EstaDisponible(string codHabit, DateTime fechaLlegada, DateTime fechaSalida)
        {
            var reservaciones = ReservacionDAL.ObtenerReservacionesPorHabitacionYFechas(codHabit, fechaLlegada, fechaSalida);
            return reservaciones.Count == 0;
        }

        public List<Habitacion> ObtenerHabitacionesDisponibles(DateTime fechaInicio, DateTime fechaFin)
        {
            List<Habitacion> habitacionesDisponibles = new List<Habitacion>();
            // Obtener los códigos de habitaciones disponibles.
            List<string> codigosDisponibles = ReservacionDAL.ObtenerHabitacionesDisponiblesPorFechas(fechaInicio, fechaFin);

            // Iterar sobre los códigos para obtener detalles completos de cada habitación.
            foreach (var codigo in codigosDisponibles)
            {
                Habitacion habitacion = habitacionDAL.SeleccionarPorId(codigo);
                if (habitacion != null)
                {
                    habitacionesDisponibles.Add(habitacion);
                }
            }

            return habitacionesDisponibles;
        }

        public static List<Habitacion> ObtenerHabitacionesPorHotel(int codHotel)//actualización: nuevo método
        {
            List<Habitacion> habitacionesPorHotel = new List<Habitacion>();

            habitacionesPorHotel = HabitacionDAL.ObtenerHabitacionesPorHotel(codHotel);


            return habitacionesPorHotel;

        }

        public Habitacion seleccionarPorId(string codHabit)
        {
            if (string.IsNullOrEmpty(codHabit) || string.IsNullOrWhiteSpace(codHabit))
            {
                throw new ArgumentException("El Código de la habitación está vacio.", nameof(codHabit));
            }

            try
            {
                return habitacionDAL.SeleccionarPorId(codHabit);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"No se pudo seleccionar el Cliente.{ex.Message}", nameof(codHabit));
            }
        }
    }
}
