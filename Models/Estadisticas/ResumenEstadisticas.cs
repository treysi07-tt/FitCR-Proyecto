using System;

namespace FitCR.Models.Estadisticas
{
    public class ResumenEstadisticas
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int TotalClientes { get; set; }
        public int ClientesActivos { get; set; }
        public int TotalVisitas { get; set; }
        public int TotalRutinas { get; set; }
        public int RutinasCompletadas { get; set; }
        public int TotalObjetivos { get; set; }
        public int ObjetivosCompletados { get; set; }
        public int TotalMedidas { get; set; }
        public int TotalInvitados { get; set; }
        public int TotalClases { get; set; }
        public int ClasesCanceladas { get; set; }
        public int ClasesFinalizadas { get; set; }
        public int TotalInscripciones { get; set; }
        public int TotalAsistencias { get; set; }

        public double PromedioVisitas
        {
            get
            {
                if (TotalClientes == 0)
                {
                    return 0;
                }

                return Math.Round(
                    TotalVisitas * 1.0 /
                    TotalClientes, 2
                );
            }
        }
    }
}
