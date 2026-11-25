using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FitCR.Controllers;
using FitCR.Models.Clases;
using FitCR.Models.Usuarios;
using FitCR.Repositories;

namespace FitCR.Services
{
    public class ServicioClases
    {
        private readonly ControladorClases controlador;

        public ServicioClases()
        {
            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data"
            );

            controlador = new ControladorClases(
                new RepositorioJson<ClaseGimnasio>(
                    Path.Combine(ruta, "clases.json")
                ),
                new RepositorioJson<InscripcionClase>(
                    Path.Combine(ruta, "inscripciones_clases.json")
                ),
                new RepositorioJson<AsistenciaClase>(
                    Path.Combine(ruta, "asistencias_clases.json")
                ),
                new RepositorioJson<Cliente>(
                    Path.Combine(ruta, "clientes.json")
                )
            );
        }

        public List<string> ObtenerTiposClasesAsistidas(
            int idCliente)
        {
            var idsClases = controlador
                .ObtenerAsistenciasCliente(idCliente)
                .Select(a => a.IdClase)
                .ToHashSet();

            return controlador.ObtenerClases()
                .Where(c =>
                    !c.Cancelada &&
                    idsClases.Contains(c.Id))
                .Select(c => c.Nombre)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        public bool CumpleAtletaVersatil(int idCliente)
        {
            string[] tiposRequeridos =
            {
                "Yoga",
                "Zumba",
                "Spinning",
                "CrossFit",
                "Gimnasio abierto"
            };

            var tiposAsistidos =
                ObtenerTiposClasesAsistidas(idCliente);

            return tiposRequeridos.All(tipo =>
                tiposAsistidos.Contains(
                    tipo,
                    StringComparer.OrdinalIgnoreCase
                ));
        }

        public List<ClaseGimnasio> ObtenerClasesCliente(
            int idCliente)
        {
            return controlador.ObtenerClasesCliente(
                idCliente
            );
        }

        public List<AsistenciaClase> ObtenerAsistenciasCliente(
            int idCliente)
        {
            return controlador.ObtenerAsistenciasCliente(
                idCliente
            );
        }

        public ClaseGimnasio ObtenerClase(int idClase)
        {
            return controlador.ObtenerClase(idClase);
        }

        public List<InscripcionClase> ObtenerInscripcionesCliente(
            int idCliente)
        {
            return controlador.ObtenerClasesCliente(idCliente)
                .SelectMany(c =>
                    controlador.ObtenerInscripciones(c.Id))
                .Where(i => i.IdCliente == idCliente)
                .OrderBy(i => i.FechaInscripcion)
                .ToList();
        }
    }
}
