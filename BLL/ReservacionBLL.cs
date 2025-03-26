using Hoteles.DAL;
using Hoteles.DEL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hoteles.BLL
{
    public class ReservacionBLL
    {
        private ReservacionDAL reservacionDAL;
        private HabitacionBLL habitacionBLL;

        public ReservacionBLL()
        {
            reservacionDAL = new ReservacionDAL();
            habitacionBLL = new HabitacionBLL();
        }

        public ResultadoOperacion CrearReserva(Reservacion reservacion)
        {
            if (reservacion == null)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El objeto de reservación es nulo." };
            }

            // Calculamos la cantidad de dias de hospedaje
            TimeSpan diferencia = reservacion.FechaSalida - reservacion.FechaLlegada;
            int estadia = (int)Math.Ceiling(diferencia.TotalDays);  // Redondea hacia arriba

            double montoPagado = (double)reservacion.MontoTarjeta + (double)reservacion.MontoEfectivo;
            string codHabitacion = reservacion.CodHabit.ToString();// Ya es string, no? Por qué le hace un ToString??
            HabitacionDAL habitacion = new HabitacionDAL();
            double montoHabitacion;
            if (habitacion.SeleccionarPorId(codHabitacion) != null) // Revisa si la habitacion buscada existe
            {
                 montoHabitacion = (double)habitacion.SeleccionarPorId(codHabitacion).Precio * estadia; // Se multiplica por lo días de hospedaje
            }
            else
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"ERROR! La habitación insertada no existe." };
            } // MOD Ricardo 24/02/2024 10:38:59

            int codHotel = reservacion.CodHotel;
            if (HotelDAL.SeleccionarPorId(codHotel) == null)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"ERROR! El hotel ingresado no existe." };
            } // MOD Ricardo 24/02/2024 11:30:43

            ClienteBLL clienteBLL = new ClienteBLL();
            int IdCliente = reservacion.IdCliente;
            if (!clienteBLL.ExistePorId(IdCliente))
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"No existe el registro del nuevo huesped." };
            } // MOD Ricardo 24/02/2024 11:38:31

            if (montoHabitacion != montoPagado)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"ERROR! El monto ingresado (₡{montoPagado}), no coincide con el monto de la estadía (\u20A1{montoHabitacion} para {estadia} dias)." };
            }

            if (reservacion.FechaLlegada >= reservacion.FechaSalida)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "La fecha de salida no puede ser anterior a la fecha de ingreso." };
            }

            if (reservacion.FechaLlegada < DateTime.Now)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "No puedes reservar para días anteriores." };
            }

            if (habitacionBLL.EstaDisponible(reservacion.CodHabit, reservacion.FechaLlegada, reservacion.FechaSalida))
            {
                try
                {
                    reservacionDAL.Insertar(reservacion);
                    return new ResultadoOperacion { Exito = true, Mensaje = "Reservación creada con éxito." };
                }
                catch (Exception ex)
                {
                    return new ResultadoOperacion { Exito = false, Mensaje = $"Error al insertar la reservación: {ex.Message}" };
                }
            }
            else
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "La habitación no está disponible para las fechas seleccionadas." };
            }
        }

        // En este método solicito el codigo de la habitación inicial, para ver si cambiaron de habitación
        public ResultadoOperacion ModificarReserva(Reservacion reservacionMod, int idClienteNuevo, string codHabitacionOriginal)
        {
            if (reservacionMod == null || idClienteNuevo <= 0)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "Los datos de la reservación modificada o el ID del cliente son inválidos." };
            }

            if (reservacionMod.FechaLlegada == null || reservacionMod.FechaLlegada >= reservacionMod.FechaSalida)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "La fecha de llegada debe ser anterior a la fecha de salida." };
            }

            if (reservacionMod.FormaPago != "Efectivo" && reservacionMod.FormaPago != "Tarjeta" && reservacionMod.FormaPago != "Mixto")
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "La forma de pago es inválida. Debe ser 'Efectivo', 'Tarjeta' o 'Mixto'." };
            }

            if ((reservacionMod.FormaPago == "Tarjeta" || reservacionMod.FormaPago == "Mixto") &&
                (string.IsNullOrWhiteSpace(reservacionMod.Voucher) || reservacionMod.MontoTarjeta <= 0))
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "Para pagos con tarjeta, el voucher debe ser válido y el monto de la tarjeta debe ser mayor que cero." };
            }

            if ((reservacionMod.FormaPago == "Efectivo" || reservacionMod.FormaPago == "Mixto") &&
                reservacionMod.MontoEfectivo <= 0)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "Para pagos en efectivo, el monto debe ser mayor que cero." };
            }

            if (codHabitacionOriginal != reservacionMod.CodHabit)
            {
                List<Reservacion> reservaciones = ReservacionDAL.ObtenerReservacionesPorHabitacionYFechas(reservacionMod.CodHabit, reservacionMod.FechaLlegada, reservacionMod.FechaSalida);
                if (reservaciones == null || reservaciones.Any())
                {
                    return new ResultadoOperacion { Exito = false, Mensaje = "La nueva habitación no está disponible en las fechas deseadas." };
                }
            }
            // MOD Ricardo
            ClienteBLL clienteBLL = new ClienteBLL();
            if (!clienteBLL.ExistePorId(idClienteNuevo))
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"No existe el registro del nuevo huesped." };
            }

            HabitacionDAL habitacion = new HabitacionDAL();
            if (habitacion.SeleccionarPorId(reservacionMod.CodHabit) == null) // Revisa si la habitacion buscada existe
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"No existe la habitación ingresada no existe." };
            }

            if (HotelDAL.SeleccionarPorId(reservacionMod.CodHotel) == null)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"ERROR! El hotel ingresado no existe." };
            }
            // End MOD
            try
            {
                reservacionDAL.Actualizar(reservacionMod);
                return new ResultadoOperacion { Exito = true, Mensaje = "Reservación modificada correctamente." };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"Error al actualizar la reservación: {ex.Message}" };
            }
        }

        public ResultadoOperacion EliminarReserva(string codReserva)
        {
            if (string.IsNullOrEmpty(codReserva))
            {
                return new ResultadoOperacion { Exito = false, Mensaje = "El código de la reserva no puede estar vacío." };
            }

            try
            {
                reservacionDAL.Eliminar(codReserva);
                return new ResultadoOperacion { Exito = true, Mensaje = "Reserva eliminada correctamente." };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion { Exito = false, Mensaje = $"Error al eliminar la reserva: {ex.Message}" };
            }
        }

        public Reservacion SeleccionarPorCodigo(string codReserva)
        {
            if (string.IsNullOrWhiteSpace(codReserva) || string.IsNullOrEmpty(codReserva))
            {
                throw new ArgumentException("El código de la reservación está vacio.", nameof(codReserva));
            }

            try
            {
                return ReservacionDAL.SeleccionarPorCodigo(codReserva);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"No se pudo seleccionar la reservacion.{ex.Message}", nameof(codReserva));
            }
        }
        // MOD: Ricardo Fecha 23/04/2024 03:02:00
        public List<Reservacion> ObtenerReservacionesPorCodHabityFechas(string codHabit, DateTime fechaLlegada, DateTime fechaSalida)
        {
            if (string.IsNullOrWhiteSpace(codHabit) || string.IsNullOrEmpty(codHabit))
            {
                throw new ArgumentException("El código de la reservación está vacio.", nameof(codHabit));
            }

            try
            {
                return ReservacionDAL.ObtenerReservacionesPorHabitacionYFechas(codHabit, fechaLlegada, fechaSalida);

            }
            catch (Exception ex)
            {

                throw new ArgumentException($"No se pudo encontrar ninguna reservacion.{ex.Message}", nameof(codHabit)); ;
            }
        }

        // Metodo utilizado para contabilidad
        public static List<Reservacion> ObtenerReservacionesPorFechas(DateTime fechaInicio, DateTime fechaFinal)
        {
            if (fechaInicio > fechaFinal)
            {
                throw new ArgumentException("La fecha de inicio de busqueda no puede ser posterior a la fecha final de busqueda.");
            }

            try
            {
                return ReservacionDAL.ObtenerReservacionesPorFechas(fechaInicio, fechaFinal);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"No se pudo seleccionar las reservaciones en ese rango de fechas.{ex.Message}");
            }
        }

        // Método utilizado en el módulo de contabiliadad para buscar las reservaciones de un hotel en cierto rango de fechas
        public static List<Reservacion> ObtenerReservacionesPorFechas(DateTime fechaInicio, DateTime fechaFinal, int cod_Hotel)
        {
            if (fechaInicio > fechaFinal)
            {
                throw new ArgumentException("La fecha de inicio de busqueda no puede ser posterior a la fecha final de busqueda.");
            }

            if (cod_Hotel <= 0)
            {
                throw new ArgumentException("Código de hotel no válido.");
            }

            try
            {
                return ReservacionDAL.ObtenerReservacionesPorFechas(fechaInicio, fechaFinal, cod_Hotel);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"No se pudo seleccionar las reservaciones en ese rango de fechas en ese hotel.{ex.Message}");
            }
        }

        // Metodo utilizado para mostrar las reservaciones activas por Hotel, en el módulo Mostrar Reservaciones.
        public static List<Reservacion> ReservacionesActivasPorHotel(int cod_Hotel)
        {
            if (cod_Hotel <= 0)
            {
                throw new ArgumentException("Código de hotel no válido.");
            }

            try
            {
                return ReservacionDAL.ReservacionesActivasPorHotel(cod_Hotel);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"No se pudo seleccionar las reservaciones activas por hotel.{ex.Message}");
            }
        }
    }
}
