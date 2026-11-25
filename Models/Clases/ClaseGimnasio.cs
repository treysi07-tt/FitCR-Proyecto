using System;

namespace FitCR.Models.Clases
{
    public class ClaseGimnasio
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public int IdEntrenador { get; set; }
        public DateTime FechaHora { get; set; }
        public int Duracion { get; set; }
        public int CupoMaximo { get; set; }
        public bool Cancelada { get; set; }
    }
}