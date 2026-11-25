using System;

namespace FitCR.Models.Visitas
{
    public class RegistroInvitado
    {
        public int Id { get; set; }

        public int IdCliente { get; set; }

        public string NombreInvitado { get; set; } = string.Empty;

        public DateTime FechaVisita { get; set; }
    }
}
