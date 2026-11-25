using System;

namespace FitCR.Models.Insignias
{
    public class InsigniaObtenida
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdInsignia { get; set; }
        public DateTime FechaObtencion { get; set; }
        public DateTime? PeriodoInicio { get; set; }
        public DateTime? PeriodoFin { get; set; }
    }
}

