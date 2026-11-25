namespace FitCR.Models.Usuarios
{
    public abstract class Entrenador : Usuario
    {
        public string ClaseImpartida { get; set; } = "Ninguna";
    }
}
