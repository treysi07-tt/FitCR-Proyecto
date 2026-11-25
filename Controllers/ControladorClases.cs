using System;
using System.Collections.Generic;
using System.Linq;
using FitCR.Interfaces;
using FitCR.Models.Clases;
using FitCR.Models.Usuarios;

namespace FitCR.Controllers
{
    public class ControladorClases
    {
        private readonly IRepositorio<ClaseGimnasio> repositorioClases;
        private readonly IRepositorio<InscripcionClase> repositorioInscripciones;
        private readonly IRepositorio<AsistenciaClase> repositorioAsistencias;
        private readonly IRepositorio<Cliente> repositorioClientes;

        public ControladorClases(
            IRepositorio<ClaseGimnasio> clases,
            IRepositorio<InscripcionClase> inscripciones,
            IRepositorio<AsistenciaClase> asistencias,
            IRepositorio<Cliente> clientes)
        {
            repositorioClases = clases;
            repositorioInscripciones = inscripciones;
            repositorioAsistencias = asistencias;
            repositorioClientes = clientes;
        }

        public List<ClaseGimnasio> ObtenerClases()
        {
            return repositorioClases.ObtenerTodos()
                .OrderBy(c => c.FechaHora)
                .ToList();
        }

        public List<ClaseGimnasio> ObtenerClasesEntrenador(
            int idEntrenador)
        {
            return ObtenerClases()
                .Where(c => c.IdEntrenador == idEntrenador)
                .ToList();
        }

        public List<ClaseGimnasio> ObtenerClasesDisponibles()
        {
            return ObtenerClases()
                .Where(c =>
                    !c.Cancelada &&
                    c.FechaHora > DateTime.Now &&
                    ObtenerCuposDisponibles(c.Id) > 0)
                .ToList();
        }

        public ClaseGimnasio ObtenerClase(int idClase)
        {
            ClaseGimnasio? clase = repositorioClases
                .ObtenerTodos()
                .FirstOrDefault(c => c.Id == idClase);

            if (clase == null)
            {
                throw new ArgumentException(
                    "No se encontró la clase."
                );
            }

            return clase;
        }

        public void RegistrarClase(
            ClaseGimnasio nuevaClase,
            Entrenador entrenador)
        {
            if (entrenador.Id <= 0)
            {
                throw new ArgumentException(
                    "El entrenador no es válido."
                );
            }

            string[] clasesPermitidas =
            {
                "Yoga",
                "Zumba",
                "Spinning",
                "CrossFit",
                "Gimnasio abierto"
            };

            if (!clasesPermitidas.Contains(
                entrenador.ClaseImpartida))
            {
                throw new ArgumentException(
                    "El entrenador no tiene una clase asignada."
                );
            }

            if (string.IsNullOrWhiteSpace(
                nuevaClase.Descripcion))
            {
                throw new ArgumentException(
                    "La descripción es obligatoria."
                );
            }

            if (nuevaClase.FechaHora <= DateTime.Now)
            {
                throw new ArgumentException(
                    "La fecha y hora deben ser posteriores al momento actual."
                );
            }

            if (nuevaClase.Duracion <= 0)
            {
                throw new ArgumentException(
                    "La duración debe ser mayor que cero."
                );
            }

            if (nuevaClase.CupoMaximo <= 0)
            {
                throw new ArgumentException(
                    "El cupo máximo debe ser mayor que cero."
                );
            }

            List<ClaseGimnasio> clases =
                repositorioClases.ObtenerTodos();

            DateTime fechaFinal = nuevaClase.FechaHora
                .AddMinutes(nuevaClase.Duracion);

            bool horarioOcupado = clases.Any(c =>
                c.IdEntrenador == entrenador.Id &&
                !c.Cancelada &&
                nuevaClase.FechaHora <
                    c.FechaHora.AddMinutes(c.Duracion) &&
                fechaFinal > c.FechaHora);

            if (horarioOcupado)
            {
                throw new ArgumentException(
                    "El entrenador ya tiene una clase en ese horario."
                );
            }

            nuevaClase.Id = clases
                .Select(c => c.Id)
                .DefaultIfEmpty(0)
                .Max() + 1;

            nuevaClase.Nombre = entrenador.ClaseImpartida;
            nuevaClase.IdEntrenador = entrenador.Id;
            nuevaClase.Descripcion =
                nuevaClase.Descripcion.Trim();
            nuevaClase.Cancelada = false;

            clases.Add(nuevaClase);

            repositorioClases.GuardarTodos(clases);
        }

        public void CancelarClase(
            int idClase,
            int idEntrenador)
        {
            List<ClaseGimnasio> clases =
                repositorioClases.ObtenerTodos();

            ClaseGimnasio? clase = clases.FirstOrDefault(c =>
                c.Id == idClase &&
                c.IdEntrenador == idEntrenador);

            if (clase == null)
            {
                throw new ArgumentException(
                    "La clase no pertenece a este entrenador."
                );
            }

            if (clase.Cancelada)
            {
                throw new ArgumentException(
                    "La clase ya está cancelada."
                );
            }

            if (clase.FechaHora <= DateTime.Now)
            {
                throw new ArgumentException(
                    "No se puede cancelar una clase que ya inició."
                );
            }

            clase.Cancelada = true;

            repositorioClases.GuardarTodos(clases);
        }

        public List<InscripcionClase> ObtenerInscripciones(
            int idClase)
        {
            return repositorioInscripciones.ObtenerTodos()
                .Where(i => i.IdClase == idClase)
                .ToList();
        }

        public int ObtenerCuposDisponibles(int idClase)
        {
            ClaseGimnasio clase = ObtenerClase(idClase);

            if (clase.Cancelada)
            {
                return 0;
            }

            int inscritos =
                ObtenerInscripciones(idClase).Count;

            return Math.Max(
                0,
                clase.CupoMaximo - inscritos
            );
        }

        public bool EstaInscrito(
            int idClase,
            int idCliente)
        {
            return repositorioInscripciones.ObtenerTodos()
                .Any(i =>
                    i.IdClase == idClase &&
                    i.IdCliente == idCliente);
        }

        public void InscribirCliente(
            int idClase,
            int idCliente)
        {
            ClaseGimnasio clase = ObtenerClase(idClase);

            bool existeCliente = repositorioClientes
                .ObtenerTodos()
                .Any(c => c.Id == idCliente);

            if (!existeCliente)
            {
                throw new ArgumentException(
                    "No se encontró el cliente."
                );
            }

            if (clase.Cancelada)
            {
                throw new ArgumentException(
                    "La clase está cancelada."
                );
            }

            if (clase.FechaHora <= DateTime.Now)
            {
                throw new ArgumentException(
                    "La clase ya inició o finalizó."
                );
            }

            if (EstaInscrito(idClase, idCliente))
            {
                throw new ArgumentException(
                    "Ya estás inscrito en esta clase."
                );
            }

            if (ObtenerCuposDisponibles(idClase) == 0)
            {
                throw new ArgumentException(
                    "No hay cupos disponibles."
                );
            }

            List<InscripcionClase> inscripciones =
                repositorioInscripciones.ObtenerTodos();

            List<ClaseGimnasio> clases =
                repositorioClases.ObtenerTodos();

            DateTime fechaFinal = clase.FechaHora
                .AddMinutes(clase.Duracion);

            bool horarioOcupado = clases.Any(c =>
                !c.Cancelada &&
                inscripciones.Any(i =>
                    i.IdClase == c.Id &&
                    i.IdCliente == idCliente) &&
                clase.FechaHora <
                    c.FechaHora.AddMinutes(c.Duracion) &&
                fechaFinal > c.FechaHora);

            if (horarioOcupado)
            {
                throw new ArgumentException(
                    "Ya tienes otra clase en ese horario."
                );
            }

            InscripcionClase inscripcion =
                new InscripcionClase
                {
                    Id = inscripciones
                        .Select(i => i.Id)
                        .DefaultIfEmpty(0)
                        .Max() + 1,

                    IdClase = idClase,
                    IdCliente = idCliente,
                    FechaInscripcion = DateTime.Now
                };

            inscripciones.Add(inscripcion);

            repositorioInscripciones
                .GuardarTodos(inscripciones);
        }

        public void CancelarInscripcion(
            int idClase,
            int idCliente)
        {
            ClaseGimnasio clase = ObtenerClase(idClase);

            if (clase.Cancelada)
            {
                throw new ArgumentException(
                    "La clase ya fue cancelada."
                );
            }

            if (clase.FechaHora <= DateTime.Now)
            {
                throw new ArgumentException(
                    "No puedes cancelar una inscripción después de iniciar la clase."
                );
            }

            List<InscripcionClase> inscripciones =
                repositorioInscripciones.ObtenerTodos();

            InscripcionClase? inscripcion =
                inscripciones.FirstOrDefault(i =>
                    i.IdClase == idClase &&
                    i.IdCliente == idCliente);

            if (inscripcion == null)
            {
                throw new ArgumentException(
                    "No estás inscrito en esta clase."
                );
            }

            inscripciones.Remove(inscripcion);

            repositorioInscripciones
                .GuardarTodos(inscripciones);
        }

        public List<ClaseGimnasio> ObtenerClasesCliente(
            int idCliente)
        {
            var idsClases = repositorioInscripciones
                .ObtenerTodos()
                .Where(i => i.IdCliente == idCliente)
                .Select(i => i.IdClase)
                .ToHashSet();

            return ObtenerClases()
                .Where(c => idsClases.Contains(c.Id))
                .ToList();
        }

        public List<Cliente> ObtenerParticipantes(
            int idClase,
            int idEntrenador)
        {
            ClaseGimnasio clase = ObtenerClase(idClase);

            if (clase.IdEntrenador != idEntrenador)
            {
                throw new ArgumentException(
                    "La clase no pertenece a este entrenador."
                );
            }

            var idsClientes = ObtenerInscripciones(idClase)
                .Select(i => i.IdCliente)
                .ToHashSet();

            return repositorioClientes.ObtenerTodos()
                .Where(c => idsClientes.Contains(c.Id))
                .OrderBy(c => c.Nombre)
                .ToList();
        }

        public void RegistrarAsistencia(
            int idClase,
            int idCliente,
            int idEntrenador)
        {
            ClaseGimnasio clase = ObtenerClase(idClase);

            if (clase.IdEntrenador != idEntrenador)
            {
                throw new ArgumentException(
                    "La clase no pertenece a este entrenador."
                );
            }

            if (clase.Cancelada)
            {
                throw new ArgumentException(
                    "No se puede registrar asistencia a una clase cancelada."
                );
            }

            if (clase.FechaHora > DateTime.Now)
            {
                throw new ArgumentException(
                    "La clase todavía no ha iniciado."
                );
            }

            if (!EstaInscrito(idClase, idCliente))
            {
                throw new ArgumentException(
                    "El cliente no está inscrito en esta clase."
                );
            }

            List<AsistenciaClase> asistencias =
                repositorioAsistencias.ObtenerTodos();

            bool asistenciaRegistrada = asistencias.Any(a =>
                a.IdClase == idClase &&
                a.IdCliente == idCliente);

            if (asistenciaRegistrada)
            {
                throw new ArgumentException(
                    "La asistencia ya está registrada."
                );
            }

            AsistenciaClase asistencia =
                new AsistenciaClase
                {
                    Id = asistencias
                        .Select(a => a.Id)
                        .DefaultIfEmpty(0)
                        .Max() + 1,

                    IdClase = idClase,
                    IdCliente = idCliente,
                    FechaAsistencia = DateTime.Now
                };

            asistencias.Add(asistencia);

            repositorioAsistencias
                .GuardarTodos(asistencias);
        }

        public List<AsistenciaClase> ObtenerAsistencias(
            int idClase)
        {
            return repositorioAsistencias.ObtenerTodos()
                .Where(a => a.IdClase == idClase)
                .OrderBy(a => a.FechaAsistencia)
                .ToList();
        }

        public List<AsistenciaClase> ObtenerAsistenciasCliente(
            int idCliente)
        {
            return repositorioAsistencias.ObtenerTodos()
                .Where(a => a.IdCliente == idCliente)
                .OrderByDescending(a => a.FechaAsistencia)
                .ToList();
        }

        public string ObtenerEstadoClase(
            ClaseGimnasio clase)
        {
            if (clase.Cancelada)
            {
                return "Cancelada";
            }

            if (DateTime.Now < clase.FechaHora)
            {
                return "Programada";
            }

            if (DateTime.Now <
                clase.FechaHora.AddMinutes(clase.Duracion))
            {
                return "En curso";
            }

            return "Finalizada";
        }
    }
}