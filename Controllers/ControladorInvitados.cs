using System;
using System.Collections.Generic;
using System.Linq;
using FitCR.Interfaces;
using FitCR.Models.Visitas;

namespace FitCR.Controllers
{
    public class ControladorInvitados
    {
        private readonly IRepositorio<RegistroInvitado> repositorio;
        private const int LimiteMensual = 3;

        public ControladorInvitados(
            IRepositorio<RegistroInvitado> repositorioRecibido)
        {
            repositorio = repositorioRecibido;
        }

        public void RegistrarInvitado(
            int idCliente, string nombreInvitado)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException(
                    "El cliente no es válido. Intente de nuevo"
                );
            }

            if (string.IsNullOrWhiteSpace(nombreInvitado))
            {
                throw new ArgumentException(
                    "Debes ingresar el nombre del invitado."
                );
            }

            List<RegistroInvitado> invitados =
                repositorio.ObtenerTodos();

            DateTime fechaActual = DateTime.Now;

            int visitasDelMes = invitados.Count(i =>
                i.IdCliente == idCliente &&
                i.FechaVisita.Year == fechaActual.Year &&
                i.FechaVisita.Month == fechaActual.Month
            );

            if (visitasDelMes >= LimiteMensual)
            {
                throw new ArgumentException(
                    "Has alcanzado el límite de 3 visitas " +
                    "de invitados durante este mes."
                );
            }

            int nuevoId = 1;

            foreach (RegistroInvitado invitado in invitados)
            {
                if (invitado.Id >= nuevoId)
                {
                    nuevoId = invitado.Id + 1;
                }
            }

            RegistroInvitado nuevoInvitado = new RegistroInvitado
            {
                Id = nuevoId,
                IdCliente = idCliente,
                NombreInvitado = nombreInvitado.Trim(),
                FechaVisita = fechaActual
            };

            invitados.Add(nuevoInvitado);
            repositorio.GuardarTodos(invitados);
        }

        public List<RegistroInvitado> ObtenerHistorial(int idCliente)
        {
            return repositorio.ObtenerTodos()
                .Where(i => i.IdCliente == idCliente)
                .OrderByDescending(i => i.FechaVisita)
                .ToList();
        }

        public int ContarVisitasDelMes(int idCliente)
        {
            DateTime hoy = DateTime.Today;

            return ObtenerHistorial(idCliente)
                .Count(i =>
                    i.FechaVisita.Year == hoy.Year &&
                    i.FechaVisita.Month == hoy.Month
                );
        }

        public int ObtenerVisitasDisponibles(int idCliente)
        {
            int utilizadas = ContarVisitasDelMes(idCliente);

            return Math.Max(0, LimiteMensual - utilizadas);
        }
    }
}
