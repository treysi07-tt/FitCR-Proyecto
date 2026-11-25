
using System;
using System.Collections.Generic;
using System.Linq;
using FitCR.Interfaces;
using FitCR.Models.Visitas;

namespace FitCR.Controllers
{
    public class ControladorVisitas
    {
        private readonly IRepositorio<RegistroVisita> repositorio;

        public ControladorVisitas(
            IRepositorio<RegistroVisita> repositorioRecibido)
        {
            repositorio = repositorioRecibido;
        }

        //Validaciones
        public void RegistrarVisita(int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException(
                    "El cliente no es válido. Intente de nuevo"
                );
            }

            List<RegistroVisita> visitas =
                repositorio.ObtenerTodos();

            bool yaRegistroHoy = visitas.Any(v =>
                v.IdCliente == idCliente &&
                v.FechaVisita.Date == DateTime.Today
            );

            if (yaRegistroHoy)
            {
                throw new ArgumentException(
                    "Ya registraste una visita el día de hoy."
                );
            }

            int nuevoId = 1;

            foreach (RegistroVisita visita in visitas)
            {
                if (visita.Id >= nuevoId)
                {
                    nuevoId = visita.Id + 1;
                }
            }

            RegistroVisita nuevaVisita = new RegistroVisita
            {
                Id = nuevoId,
                IdCliente = idCliente,
                FechaVisita = DateTime.Now
            };

            visitas.Add(nuevaVisita);
            repositorio.GuardarTodos(visitas);
        }

        public List<RegistroVisita> ObtenerHistorial(int idCliente)
        {
            return repositorio.ObtenerTodos()
                .Where(v => v.IdCliente == idCliente)
                .OrderByDescending(v => v.FechaVisita)
                .ToList();
        }

        public int ContarVisitas(int idCliente)
        {
            return ObtenerHistorial(idCliente).Count;
        }
    }
}
