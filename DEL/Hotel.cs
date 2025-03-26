using System.Collections.Generic;

namespace Hoteles.DEL
{
    public class Hotel
    {
        public int CodHotel { get; set; }
        public string DPais { get; set; }
        public string DProvincia { get; set; }
        public string DCanton { get; set; }
        public string DExacta { get; set; }
        public bool Activo { get; set; }
        public List<TelefonoHotel> Telefonos { get; set; } = new List<TelefonoHotel>();
    }
}
