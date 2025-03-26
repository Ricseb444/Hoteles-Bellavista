using System.Collections.Generic;

namespace Hoteles.DEL
{
    public class Cliente
    {
        public int IDCliente { get; set; }
        public string Nombre { get; set; } = "No indica"; // Valor "No indica" predeterminado
        public string Apellido1 { get; set; } = "No indica"; // Valor "No indica" predeterminado
        public string Apellido2 { get; set; } = "No indica"; // Valor "No indica" predeterminado
        public bool Activo { get; set; } = true; // Valor 1 predeterminado

        public List<TelefonoCliente> Telefonos { get; set; } = new List<TelefonoCliente>(); // Lista de teléfonos del cliente
    }
}
