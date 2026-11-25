using System;

namespace FitCR.Models.Notificaciones
{
    public class Notificacion
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public bool Leida { get; set; }
        public string Referencia { get; set; } = string.Empty;
    }
}
