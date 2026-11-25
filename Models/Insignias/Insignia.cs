namespace FitCR.Models.Insignias
{
    public class Insignia
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int CantidadRequerida { get; set; }
        public string Icono { get; set; } = string.Empty;
        public bool EsNegativa { get; set; }
        public bool EsPeriodica { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}


