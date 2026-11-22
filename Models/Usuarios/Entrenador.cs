using System;
using System.Collections.Generic;
using System.Text;

namespace FitCR.Models.Usuarios
{
    public abstract class Entrenador : Usuario
    {
        public string Especialidad { get; set; } = "";

        public string ClaseImpartida { get; set; } = "Ninguna";
    }
}
