using System;

namespace FitCR.Models.Progreso
{
    public class ProgresoObjetivo
    {
        public int IdObjetivo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string TipoObjetivo { get; set; } = string.Empty;
        public double? ValorInicial { get; set; }
        public double? ValorActual { get; set; }
        public double ValorMeta { get; set; }
        public double Porcentaje { get; set; }
        public double? CantidadRestante { get; set; }
        public DateTime FechaLimite { get; set; }
        public bool MetaAlcanzada { get; set; }
        public bool Vencido { get; set; }
        public string Unidad { get; set; } = string.Empty;

        public string Estado
        {
            get
            {
                if (MetaAlcanzada)
                {
                    return "Completado";
                }

                if (Vencido)
                {
                    return "Vencido";
                }

                if (!ValorActual.HasValue)
                {
                    return "Sin registros";
                }

                return "En progreso";
            }
        }
    }
}

