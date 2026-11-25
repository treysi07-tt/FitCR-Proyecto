namespace FitCR.Models.Estadisticas
{
    public class EstadisticaCliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = "";
        public string NombreUsuario { get; set; } = "";
        public int TotalVisitas { get; set; }
        public int TotalRutinas { get; set; }
        public int RutinasCompletadas { get; set; }
        public int TotalObjetivos { get; set; }
        public int ObjetivosCompletados { get; set; }
        public int TotalMedidas { get; set; }
        public int TotalInvitados { get; set; }
        public int AsistenciasClases { get; set; }
        public double PorcentajeRutinas
        {
            get
            {
                if (TotalRutinas == 0)
                {
                    return 0;
                }

                return Math.Round(
                    RutinasCompletadas * 100.0 /
                    TotalRutinas, 2
                );
            }
        }

        public double PorcentajeObjetivos
        {
            get
            {
                if (TotalObjetivos == 0)
                {
                    return 0;
                }

                return Math.Round(
                    ObjetivosCompletados * 100.0 /
                    TotalObjetivos, 2
                );
            }
        }
    }
}