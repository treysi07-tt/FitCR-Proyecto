using System;
using System.Collections.Generic;
using System.Text;

namespace FitCR.Models.Usuarios
{
    public class EntrenadorGimnasio : Entrenador
    {
        public override string ObtenerTipoUsuario()
        {
            return "Entrenador de Gimnasio";
        }
    }
}
