using System;

namespace FitCR.Models.Estadisticas
{
    public class EstadisticaClase
    {
        public string NombreClase { get; set; } = "";
        public int TotalClases { get; set; }
        public int ClasesCanceladas { get; set; }
        public int ClasesFinalizadas { get; set; }
        public int TotalInscripciones { get; set; }
        public int TotalAsistencias { get; set; }
        public int InscripcionesFinalizadas { get; set; }
        public int AsistenciasFinalizadas { get; set; }

        public double PorcentajeAsistencia
        {
            get
            {
                if (InscripcionesFinalizadas == 0)
                {
                    return 0;
                }

                return Math.Round(
                    AsistenciasFinalizadas * 100.0 /
                    InscripcionesFinalizadas, 2
                );
            }
        }
    }
}
