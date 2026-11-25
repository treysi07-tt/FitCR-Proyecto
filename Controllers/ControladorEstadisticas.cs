using System;
using System.Collections.Generic;
using System.Linq;
using FitCR.Interfaces;
using FitCR.Models.Usuarios;
using FitCR.Models.Rutinas;
using FitCR.Models.Visitas;
using FitCR.Models.Medidas;
using FitCR.Models.Objetivos;
using FitCR.Models.Clases;
using FitCR.Models.Estadisticas;

namespace FitCR.Controllers
{
    public class ControladorEstadisticas
    {
        private readonly IRepositorio<Cliente>repositorioClientes;

        private readonly IRepositorio<Rutina>repositorioRutinas;

        private readonly IRepositorio<RegistroVisita>repositorioVisitas;

        private readonly IRepositorio<RegistroMedida>repositorioMedidas;

        private readonly IRepositorio<Objetivo>repositorioObjetivos;

        private readonly IRepositorio<RegistroInvitado>repositorioInvitados;

        private readonly IRepositorio<ClaseGimnasio>repositorioClases;

        private readonly IRepositorio<InscripcionClase>repositorioInscripciones;

        private readonly IRepositorio<AsistenciaClase>repositorioAsistencias;

        public ControladorEstadisticas(
            IRepositorio<Cliente> clientes,
            IRepositorio<Rutina> rutinas,
            IRepositorio<RegistroVisita> visitas,
            IRepositorio<RegistroMedida> medidas,
            IRepositorio<Objetivo> objetivos,
            IRepositorio<RegistroInvitado> invitados,
            IRepositorio<ClaseGimnasio> clases,
            IRepositorio<InscripcionClase> inscripciones,
            IRepositorio<AsistenciaClase> asistencias)
        {
            repositorioClientes = clientes;
            repositorioRutinas = rutinas;
            repositorioVisitas = visitas;
            repositorioMedidas = medidas;
            repositorioObjetivos = objetivos;
            repositorioInvitados = invitados;
            repositorioClases = clases;
            repositorioInscripciones = inscripciones;
            repositorioAsistencias = asistencias;
        }

        public List<EstadisticaCliente>
            ObtenerEstadisticasClientes(
                DateTime inicio,
                DateTime fin)
        {
            ValidarFechas(inicio, fin);

            var clientes =
                repositorioClientes.ObtenerTodos();

            var rutinas =
                repositorioRutinas.ObtenerTodos();

            var visitas =
                repositorioVisitas.ObtenerTodos();

            var medidas =
                repositorioMedidas.ObtenerTodos();

            var objetivos =
                repositorioObjetivos.ObtenerTodos();

            var invitados =
                repositorioInvitados.ObtenerTodos();

            var asistencias =
                repositorioAsistencias.ObtenerTodos();

            List<EstadisticaCliente> resultados =
                new List<EstadisticaCliente>();

            foreach (Cliente cliente in clientes)
            {
                var rutinasCliente = rutinas
                    .Where(r =>
                        r.IdCliente == cliente.Id &&
                        EstaEnPeriodo(
                            r.FechaCreacion,
                            inicio,
                            fin))
                    .ToList();

                var objetivosCliente = objetivos
                    .Where(o =>
                        o.IdCliente == cliente.Id &&
                        EstaEnPeriodo(
                            o.FechaCreacion,
                            inicio,
                            fin))
                    .ToList();

                EstadisticaCliente estadistica =
                    new EstadisticaCliente
                    {
                        IdCliente = cliente.Id,
                        Nombre = cliente.Nombre,
                        NombreUsuario = cliente.NombreUsuario,

                        TotalVisitas = visitas.Count(v =>
                            v.IdCliente == cliente.Id &&
                            EstaEnPeriodo(
                                v.FechaVisita,
                                inicio,
                                fin)),

                        TotalRutinas =
                            rutinasCliente.Count,

                        RutinasCompletadas =
                            rutinasCliente.Count(r =>
                                r.Completada &&
                                r.FechaCompletada.HasValue &&
                                EstaEnPeriodo(
                                    r.FechaCompletada.Value,
                                    inicio,
                                    fin)),

                        TotalObjetivos =
                            objetivosCliente.Count,

                        ObjetivosCompletados =
                            objetivosCliente.Count(o =>
                                o.Completado),

                        TotalMedidas = medidas.Count(m =>
                            m.IdCliente == cliente.Id &&
                            EstaEnPeriodo(
                                m.Fecha,
                                inicio,
                                fin)),

                        TotalInvitados = invitados.Count(i =>
                            i.IdCliente == cliente.Id &&
                            EstaEnPeriodo(
                                i.FechaVisita,
                                inicio,
                                fin)),

                        AsistenciasClases =
                            asistencias.Count(a =>
                                a.IdCliente == cliente.Id &&
                                EstaEnPeriodo(
                                    a.FechaAsistencia,
                                    inicio,
                                    fin))
                    };

                resultados.Add(estadistica);
            }

            return resultados
                .OrderBy(c => c.Nombre)
                .ToList();
        }

        public List<EstadisticaCliente>
            ObtenerClientesMayorAsistencia(
                DateTime inicio,
                DateTime fin,
                int cantidad = 10)
        {
            return ObtenerEstadisticasClientes(inicio, fin)
                .Where(c => c.TotalVisitas > 0)
                .OrderByDescending(c => c.TotalVisitas)
                .ThenBy(c => c.Nombre)
                .ThenBy(c => c.IdCliente)
                .Take(cantidad)
                .ToList();
        }

        public List<EstadisticaCliente>
            ObtenerClientesMenorAsistencia(
                DateTime inicio,
                DateTime fin,
                int cantidad = 10)
        {
            return ObtenerEstadisticasClientes(inicio, fin)
                .OrderBy(c => c.TotalVisitas)
                .ThenBy(c => c.Nombre)
                .ThenBy(c => c.IdCliente)
                .Take(cantidad)
                .ToList();
        }

        public List<EstadisticaCliente>
            ObtenerClientesMejoresObjetivos(
                DateTime inicio,
                DateTime fin)
        {
            return ObtenerEstadisticasClientes(inicio, fin)
                .Where(c => c.TotalObjetivos > 0)
                .OrderByDescending(c =>
                    c.PorcentajeObjetivos)
                .ThenByDescending(c =>
                    c.ObjetivosCompletados)
                .ThenBy(c => c.Nombre)
                .ToList();
        }

        public List<EstadisticaCliente>
            ObtenerClientesMejoresRutinas(
                DateTime inicio,
                DateTime fin)
        {
            return ObtenerEstadisticasClientes(inicio, fin)
                .Where(c => c.TotalRutinas > 0)
                .OrderByDescending(c =>
                    c.RutinasCompletadas)
                .ThenByDescending(c =>
                    c.PorcentajeRutinas)
                .ThenBy(c => c.Nombre)
                .ToList();
        }

        public List<EstadisticaPeso>
            ObtenerEstadisticasPeso(
                DateTime inicio,
                DateTime fin)
        {
            ValidarFechas(inicio, fin);

            var clientes =
                repositorioClientes.ObtenerTodos();

            var medidas =
                repositorioMedidas.ObtenerTodos();

            List<EstadisticaPeso> resultados =
                new List<EstadisticaPeso>();

            foreach (Cliente cliente in clientes)
            {
                var historial = medidas
                    .Where(m =>
                        m.IdCliente == cliente.Id &&
                        EstaEnPeriodo(
                            m.Fecha,
                            inicio,
                            fin))
                    .OrderBy(m => m.Fecha)
                    .ThenBy(m => m.Id)
                    .ToList();

                if (historial.Count < 2)
                {
                    continue;
                }

                var primera = historial.First();
                var ultima = historial.Last();

                resultados.Add(new EstadisticaPeso
                {
                    IdCliente = cliente.Id,
                    Nombre = cliente.Nombre,
                    PesoInicial = primera.Peso,
                    PesoFinal = ultima.Peso,
                    FechaInicial = primera.Fecha,
                    FechaFinal = ultima.Fecha,
                    TotalRegistros = historial.Count
                });
            }

            return resultados
                .OrderByDescending(p => p.CambioPeso)
                .ToList();
        }

        public List<EstadisticaClase>
            ObtenerEstadisticasClases(
                DateTime inicio,
                DateTime fin)
        {
            ValidarFechas(inicio, fin);

            var clases = repositorioClases
                .ObtenerTodos()
                .Where(c => EstaEnPeriodo(
                    c.FechaHora,
                    inicio,
                    fin))
                .ToList();

            var inscripciones =
                repositorioInscripciones.ObtenerTodos();

            var asistencias =
                repositorioAsistencias.ObtenerTodos();

            string[] tiposClases =
            {
                "Yoga",
                "Zumba",
                "Spinning",
                "CrossFit",
                "Gimnasio abierto"
            };

            List<EstadisticaClase> resultados =
                new List<EstadisticaClase>();

            foreach (string tipo in tiposClases)
            {
                var clasesTipo = clases
                    .Where(c => string.Equals(
                        c.Nombre,
                        tipo,
                        StringComparison.OrdinalIgnoreCase))
                    .ToList();

                var idsClases = clasesTipo
                    .Where(c => !c.Cancelada)
                    .Select(c => c.Id)
                    .ToHashSet();

                var idsFinalizadas = clasesTipo
                    .Where(c =>
                        !c.Cancelada &&
                        c.FechaHora.AddMinutes(c.Duracion)
                            <= DateTime.Now)
                    .Select(c => c.Id)
                    .ToHashSet();

                resultados.Add(new EstadisticaClase
                {
                    NombreClase = tipo,

                    TotalClases = clasesTipo.Count,

                    ClasesCanceladas = clasesTipo
                        .Count(c => c.Cancelada),

                    ClasesFinalizadas =
                        idsFinalizadas.Count,

                    TotalInscripciones =
                        inscripciones.Count(i =>
                            idsClases.Contains(i.IdClase)),

                    TotalAsistencias =
                        asistencias.Count(a =>
                            idsClases.Contains(a.IdClase)),

                    InscripcionesFinalizadas =
                        inscripciones.Count(i =>
                            idsFinalizadas.Contains(
                                i.IdClase)),

                    AsistenciasFinalizadas =
                        asistencias.Count(a =>
                            idsFinalizadas.Contains(
                                a.IdClase))
                });
            }

            return resultados
                .OrderByDescending(c =>
                    c.TotalAsistencias)
                .ToList();
        }

        public ResumenEstadisticas ObtenerResumen(
            DateTime inicio,
            DateTime fin)
        {
            ValidarFechas(inicio, fin);

            var clientes =
                ObtenerEstadisticasClientes(inicio, fin);

            var clases =
                ObtenerEstadisticasClases(inicio, fin);

            return new ResumenEstadisticas
            {
                FechaInicio = inicio.Date,
                FechaFin = fin.Date,

                TotalClientes = clientes.Count,

                ClientesActivos = clientes.Count(c =>
                    c.TotalVisitas > 0),

                TotalVisitas = clientes.Sum(c =>
                    c.TotalVisitas),

                TotalRutinas = clientes.Sum(c =>
                    c.TotalRutinas),

                RutinasCompletadas = clientes.Sum(c =>
                    c.RutinasCompletadas),

                TotalObjetivos = clientes.Sum(c =>
                    c.TotalObjetivos),

                ObjetivosCompletados = clientes.Sum(c =>
                    c.ObjetivosCompletados),

                TotalMedidas = clientes.Sum(c =>
                    c.TotalMedidas),

                TotalInvitados = clientes.Sum(c =>
                    c.TotalInvitados),

                TotalClases = clases.Sum(c =>
                    c.TotalClases),

                ClasesCanceladas = clases.Sum(c =>
                    c.ClasesCanceladas),

                ClasesFinalizadas = clases.Sum(c =>
                    c.ClasesFinalizadas),

                TotalInscripciones = clases.Sum(c =>
                    c.TotalInscripciones),

                TotalAsistencias = clases.Sum(c =>
                    c.TotalAsistencias)
            };
        }

        private static bool EstaEnPeriodo(
            DateTime fecha,
            DateTime inicio,
            DateTime fin)
        {
            return fecha.Date >= inicio.Date &&
                   fecha.Date <= fin.Date;
        }

        private static void ValidarFechas(
            DateTime inicio,
            DateTime fin)
        {
            if (inicio.Date > fin.Date)
            {
                throw new ArgumentException(
                    "La fecha inicial no puede ser " +
                    "posterior a la fecha final."
                );
            }
        }
    }
}
