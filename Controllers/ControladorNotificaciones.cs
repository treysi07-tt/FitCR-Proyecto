using System;
using System.Collections.Generic;
using System.Linq;
using FitCR.Interfaces;
using FitCR.Models.Notificaciones;

namespace FitCR.Controllers
{
    public class ControladorNotificaciones
    {
        private readonly IRepositorio<Notificacion> repositorio;

        public ControladorNotificaciones(
            IRepositorio<Notificacion> repositorioRecibido)
        {
            repositorio = repositorioRecibido;
        }

        public bool RegistrarNotificacion(
            int idCliente,
            string titulo,
            string mensaje,
            string tipo,
            string referencia,
            DateTime? fecha = null)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException(
                    "El cliente no es válido.");
            }

            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException(
                    "El título es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(mensaje))
            {
                throw new ArgumentException(
                    "El mensaje es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(tipo))
            {
                throw new ArgumentException(
                    "El tipo de notificación es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(referencia))
            {
                throw new ArgumentException(
                    "La referencia es obligatoria.");
            }

            List<Notificacion> notificaciones =
                repositorio.ObtenerTodos();

            bool existe = notificaciones.Any(n =>
                n.IdCliente == idCliente &&
                n.Referencia == referencia);

            if (existe)
            {
                return false;
            }

            int nuevoId = notificaciones.Count == 0
                ? 1
                : notificaciones.Max(n => n.Id) + 1;

            Notificacion nuevaNotificacion = new Notificacion
            {
                Id = nuevoId,
                IdCliente = idCliente,
                Titulo = titulo,
                Mensaje = mensaje,
                Tipo = tipo,
                Fecha = fecha ?? DateTime.Now,
                Leida = false,
                Referencia = referencia
            };

            notificaciones.Add(nuevaNotificacion);

            repositorio.GuardarTodos(notificaciones);

            return true;
        }

        public List<Notificacion> ObtenerNotificaciones(
            int idCliente)
        {
            return repositorio.ObtenerTodos()
                .Where(n => n.IdCliente == idCliente)
                .OrderByDescending(n => n.Fecha)
                .ThenByDescending(n => n.Id)
                .ToList();
        }

        public List<Notificacion> ObtenerNotificacionesSinLeer(
            int idCliente)
        {
            return ObtenerNotificaciones(idCliente)
                .Where(n => !n.Leida)
                .ToList();
        }

        public int ContarNotificacionesSinLeer(
            int idCliente)
        {
            return ObtenerNotificacionesSinLeer(idCliente)
                .Count;
        }

        public void MarcarComoLeida(
            int idNotificacion,
            int idCliente)
        {
            List<Notificacion> notificaciones =
                repositorio.ObtenerTodos();

            Notificacion? notificacion =
                notificaciones.FirstOrDefault(n =>
                    n.Id == idNotificacion &&
                    n.IdCliente == idCliente);

            if (notificacion == null)
            {
                throw new ArgumentException(
                    "La notificación no existe.");
            }

            if (notificacion.Leida)
            {
                return;
            }

            notificacion.Leida = true;

            repositorio.GuardarTodos(notificaciones);
        }

        public int MarcarTodasComoLeidas(int idCliente)
        {
            List<Notificacion> notificaciones =
                repositorio.ObtenerTodos();

            int cantidad = 0;

            foreach (Notificacion notificacion in notificaciones)
            {
                if (notificacion.IdCliente == idCliente &&
                    !notificacion.Leida)
                {
                    notificacion.Leida = true;
                    cantidad++;
                }
            }

            if (cantidad > 0)
            {
                repositorio.GuardarTodos(notificaciones);
            }

            return cantidad;
        }

        public List<Notificacion> ObtenerNotificacionesPorTipo(
            int idCliente,
            string tipo)
        {
            return ObtenerNotificaciones(idCliente)
                .Where(n => string.Equals(
                    n.Tipo,
                    tipo,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
