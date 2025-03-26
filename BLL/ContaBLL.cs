using Hoteles.BLL;
using Hoteles.DEL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class ContaBLL
    {
        public static (decimal TotalTarjeta, decimal TotalEfectivo, List<Reservacion> TotalReservaciones) DesglosePagos(DateTime fechaInicio, DateTime fechaFinal, int codHotel)
        {
            if (fechaInicio > fechaFinal)
            {
                throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha final.");
            }

            if (codHotel <= 0)
            {
                try
                {
                    // ObtenerReservacionesPorFechas devuelve una lista de reservaciones
                    List<Reservacion> reservaciones = ReservacionBLL.ObtenerReservacionesPorFechas(fechaInicio, fechaFinal);

                    // CalcularTotales devuelve una tupla (decimal, decimal)
                    var (totalTarjeta, totalEfectivo) = DAL.ContaDAL.CalcularTotales(fechaInicio, fechaFinal);

                    return (totalTarjeta, totalEfectivo, reservaciones);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error al calcular totales desde la capa BLL: {ex.Message}");
                }
            }
            else
            {
                try
                {
                    // ObtenerReservacionesPorFechas devuelve una lista de reservaciones
                    List<Reservacion> reservaciones = ReservacionBLL.ObtenerReservacionesPorFechas(fechaInicio, fechaFinal, codHotel);

                    // CalcularTotales devuelve una tupla (decimal, decimal)
                    var (totalTarjeta, totalEfectivo) = DAL.ContaDAL.CalcularTotales(fechaInicio, fechaFinal, codHotel);

                    return (totalTarjeta, totalEfectivo, reservaciones);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error al calcular totales desde la capa BLL: {ex.Message}");
                }
            }
        }
    }
}
