using System;

namespace FitCR.Models.Objetivos
{
    public class Objetivo
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string TipoObjetivo { get; set; } = string.Empty;
        public double ValorMeta { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaLimite { get; set; }
        public bool Completado { get; set; }
        public DateTime? FechaCompletado { get; set; }
    }
}

