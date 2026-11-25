using System;

namespace FitCR.Models.Clases
{
    public class InscripcionClase
    {
        public int Id { get; set; }
        public int IdClase { get; set; }
        public int IdCliente { get; set; }
        public DateTime FechaInscripcion { get; set; }
    }
}
