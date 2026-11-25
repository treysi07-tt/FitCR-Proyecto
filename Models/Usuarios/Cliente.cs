using System;
using System.Collections.Generic;
using System.Text;

namespace FitCR.Models.Usuarios
{
    public class Cliente : Usuario
    {
        public double Peso { get; set; }
        public double Estatura { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public double CalcularIMC()
        {
            if (Estatura <= 0 || Peso <= 0)
            {
                throw new InvalidOperationException(
                    "El peso y la estaturan no puede ser cero. Ingrese datos correctos."
                );
            }

            double imc = Peso / (Estatura * Estatura);

            return Math.Round(imc, 2);
        }

        public override string ObtenerTipoUsuario()
        {
            return "Cliente";
        }
    }
}
