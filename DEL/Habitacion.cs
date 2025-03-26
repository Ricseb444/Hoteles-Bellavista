namespace Hoteles.DEL
{
    using System.Collections.Generic;

    public class Habitacion
    {
        public string CodHabit { get; set; }
        public int CodHotel { get; set; }
        public string Categ { get; set; }
        public bool Soleada { get; set; }
        public bool Lavado { get; set; }
        public bool Nevera { get; set; }
        public decimal Precio { get; set; }
        public int CantPers { get; set; }
        public string Estado { get; set; }
        public List<Mobiliario> Mobiliarios { get; set; } = new List<Mobiliario>();
    }
}
