using System;

namespace FitCR.Models.Estadisticas
{
    public class EstadisticaPeso
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = "";
        public double PesoInicial { get; set; }
        public double PesoFinal { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public int TotalRegistros { get; set; }

        public double CambioPeso
        {
            get
            {
                return Math.Round(
                    PesoFinal - PesoInicial, 2
                );
            }
        }

        public string Tendencia
        {
            get
            {
                if (CambioPeso > 0)
                {
                    return "Aumento de peso";
                }

                if (CambioPeso < 0)
                {
                    return "Disminución de peso";
                }

                return "Sin cambios";
            }
        }
    }
}
