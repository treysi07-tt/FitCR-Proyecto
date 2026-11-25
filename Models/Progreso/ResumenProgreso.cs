namespace FitCR.Models.Progreso
{
    public class ResumenProgreso
    {
        public int IdCliente { get; set; }

        // Rutinas
        public int TotalRutinas { get; set; }

        public int RutinasCompletadas { get; set; }

        public int RutinasPendientes
        {
            get
            {
                return TotalRutinas - RutinasCompletadas;
            }
        }

        public int TotalVisitas { get; set; }

        public int VisitasMesActual { get; set; }

        // Objetivos
        public int TotalObjetivos { get; set; }

        public int ObjetivosCompletados { get; set; }

        public int ObjetivosPendientes
        {
            get
            {
                return TotalObjetivos - ObjetivosCompletados;
            }
        }

        public int TotalMedidas { get; set; }

        public double? PesoInicial { get; set; }

        public double? PesoActual { get; set; }

        public double? CalcularCambioPeso()
        {
            if (!PesoInicial.HasValue ||
                !PesoActual.HasValue)
            {
                return null;
            }

            return PesoActual.Value - PesoInicial.Value;
        }

        public double CalcularPorcentajeRutinas()
        {
            if (TotalRutinas == 0)
            {
                return 0;
            }

            return (double)RutinasCompletadas
                / TotalRutinas * 100;
        }

        public double CalcularPorcentajeObjetivos()
        {
            if (TotalObjetivos == 0)
            {
                return 0;
            }

            return (double)ObjetivosCompletados
                / TotalObjetivos * 100;
        }
    }
}

