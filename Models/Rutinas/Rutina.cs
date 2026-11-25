using System;

namespace FitCR.Models.Rutinas
{
    public class Rutina
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string TipoEjercicio { get; set; } = string.Empty;
        public int Duracion { get; set; }
        public int Series { get; set; }
        public int Repeticiones { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool Completada { get; set; }
        public DateTime? FechaCompletada { get; set; }
    }
}

