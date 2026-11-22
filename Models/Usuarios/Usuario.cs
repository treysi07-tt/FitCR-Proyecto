using System;
using System.Collections.Generic;
using System.Text;

namespace FitCR.Models.Usuarios
{
    public abstract class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = "";

        public string NombreUsuario { get; set; } = "";

        public int Edad { get; set; }

        public abstract string ObtenerTipoUsuario();
    }
}
