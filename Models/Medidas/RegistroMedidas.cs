
using System;

namespace FitCR.Models.Medidas
{
    public class RegistroMedida
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public double Peso { get; set; }
        public double Estatura { get; set; }
        public double IMC { get; set; }
    }
}
