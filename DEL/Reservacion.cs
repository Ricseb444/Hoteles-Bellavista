namespace Hoteles.DEL
{
    using System;

    public class Reservacion
    {
        public string CodReserva { get; set; }
        public int IdCliente { get; set; }
        public int CodHotel { get; set; }
        public string CodHabit { get; set; }
        public DateTime FechaLlegada { get; set; }
        public DateTime FechaSalida { get; set; }
        public string FormaPago { get; set; }
        public string Voucher { get; set; } = "No";
        public decimal MontoTarjeta { get; set; }
        public decimal MontoEfectivo { get; set; }
        public string Estado { get; set; }
    }
}
