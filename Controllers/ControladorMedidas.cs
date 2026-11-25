using System;
using System.Collections.Generic;
using FitCR.Interfaces;
using FitCR.Models.Medidas;

namespace FitCR.Controllers
{
    public class ControladorMedidas
    {
        private readonly IRepositorio<RegistroMedida> repositorio;

        public ControladorMedidas(
            IRepositorio<RegistroMedida> repositorioRecibido)
        {
            repositorio = repositorioRecibido;
        }

        public void RegistrarMedida(
            int idCliente,
            double peso,
            double estatura)
        {
            // Validar datos
            if (idCliente <= 0)
            {
                throw new ArgumentException(
                    "El cliente no es válido. Intente de nuevo. "
                );
            }

            if (peso <= 0)
            {
                throw new ArgumentException(
                    "El peso debe ser mayor que cero. Intente de nuevo."
                );
            }

            if (estatura <= 0)
            {
                throw new ArgumentException(
                    "La estatura debe ser mayor que cero. Intente de nuevo."
                );
            }

            List<RegistroMedida> medidas = repositorio.ObtenerTodos();
            int nuevoId = 1;

            foreach (RegistroMedida medida in medidas)
            {
                if (medida.Id >= nuevoId)
                {
                    nuevoId = medida.Id + 1;
                }
            }

            RegistroMedida nuevaMedida = new RegistroMedida();

            nuevaMedida.Id = nuevoId;
            nuevaMedida.IdCliente = idCliente;
            nuevaMedida.Fecha = DateTime.Now;
            nuevaMedida.Peso = peso;
            nuevaMedida.Estatura = estatura;
            nuevaMedida.IMC = Math.Round(
                peso / (estatura * estatura), 2
            );

            medidas.Add(nuevaMedida);
            repositorio.GuardarTodos(medidas);
        }

        public List<RegistroMedida> ObtenerHistorial(int idCliente)
        {
            List<RegistroMedida> medidas = repositorio.ObtenerTodos();

            List<RegistroMedida> historial =
                new List<RegistroMedida>();

            foreach (RegistroMedida medida in medidas)
            {
                if (medida.IdCliente == idCliente)
                {
                    historial.Add(medida);
                }
            }

            historial.Sort((a, b) =>
                b.Fecha.CompareTo(a.Fecha));

            return historial;
        }
    }
}

