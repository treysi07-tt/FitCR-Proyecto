using System;
using System.Collections.Generic;
using System.Linq;
using FitCR.Interfaces;
using FitCR.Models.Insignias;
using FitCR.Models.Rutinas;
using FitCR.Models.Visitas;
using FitCR.Models.Objetivos;
using FitCR.Models.Medidas;
using FitCR.Services;

namespace FitCR.Controllers
{
    public class ControladorInsignias
    {
        private readonly IRepositorio<InsigniaObtenida> repositorioInsignias;
        private readonly IRepositorio<Rutina> repositorioRutinas;
        private readonly IRepositorio<RegistroVisita> repositorioVisitas;
        private readonly IRepositorio<Objetivo> repositorioObjetivos;
        private readonly IRepositorio<RegistroMedida> repositorioMedidas;
        private readonly IRepositorio<RegistroInvitado> repositorioInvitados;

        public ControladorInsignias(
            IRepositorio<InsigniaObtenida> insignias,
            IRepositorio<Rutina> rutinas,
            IRepositorio<RegistroVisita> visitas,
            IRepositorio<Objetivo> objetivos,
            IRepositorio<RegistroMedida> medidas,
            IRepositorio<RegistroInvitado> invitados)
        {
            repositorioInsignias = insignias;
            repositorioRutinas = rutinas;
            repositorioVisitas = visitas;
            repositorioObjetivos = objetivos;
            repositorioMedidas = medidas;
            repositorioInvitados = invitados;
        }

        public List<Insignia> ObtenerCatalogo()
        {
            return CatalogoInsignias.ObtenerInsignias();
        }

        public List<InsigniaObtenida> ObtenerInsigniasCliente(
            int idCliente)
        {
            return repositorioInsignias.ObtenerTodos()
                .Where(i => i.IdCliente == idCliente)
                .OrderByDescending(i => i.FechaObtencion)
                .ToList();
        }

        public List<Insignia> EvaluarInsignias(int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException(
                    "El cliente no es válido.");
            }

            ServicioClases servicioClases =
                new ServicioClases();

            bool cumpleAtletaVersatil =
                servicioClases.CumpleAtletaVersatil(idCliente);

            var controladorProgreso =
                new ControladorProgresoObjetivos(
                    repositorioObjetivos,
                    repositorioRutinas,
                    repositorioMedidas,
                    repositorioVisitas
                );

            controladorProgreso.SincronizarObjetivos(idCliente);

            var rutinas = repositorioRutinas.ObtenerTodos()
                .Where(r => r.IdCliente == idCliente &&
                            r.Completada)
                .ToList();

            var visitas = repositorioVisitas.ObtenerTodos()
                .Where(v => v.IdCliente == idCliente)
                .ToList();

            var objetivos = repositorioObjetivos.ObtenerTodos()
                .Where(o => o.IdCliente == idCliente)
                .ToList();

            var medidas = repositorioMedidas.ObtenerTodos()
                .Where(m => m.IdCliente == idCliente)
                .ToList();

            var invitados = repositorioInvitados.ObtenerTodos()
                .Where(i => i.IdCliente == idCliente)
                .ToList();

            var objetivosCompletados = objetivos
                .Where(o => o.Completado)
                .ToList();

            var catalogo = ObtenerCatalogo();

            var registros =
                repositorioInsignias.ObtenerTodos();

            var nuevasInsignias = new List<Insignia>();

            int siguienteId = registros.Count == 0
                ? 1
                : registros.Max(i => i.Id) + 1;

            int racha = CalcularRachaMaxima(visitas);

            foreach (Insignia insignia in catalogo)
            {
                if (insignia.EsPeriodica)
                {
                    continue;
                }

                bool yaObtenida = registros.Any(i =>
                    i.IdCliente == idCliente &&
                    i.IdInsignia == insignia.Id);

                if (yaObtenida)
                {
                    continue;
                }

                bool cumple = insignia.Id switch
                {
                    1 => rutinas.Count >= 1,
                    2 => rutinas.Count >= 10,
                    3 => rutinas.Count >= 25,
                    4 => rutinas.Count >= 50,
                    5 => rutinas.Count(r =>
                        string.Equals(
                            r.TipoEjercicio,
                            "Fuerza",
                            StringComparison.OrdinalIgnoreCase
                        )) >= 10,

                    6 => rutinas.Count(r =>
                        string.Equals(
                            r.TipoEjercicio,
                            "Cardio",
                            StringComparison.OrdinalIgnoreCase
                        )) >= 10,
                    7 => rutinas.Sum(r => r.Duracion) >= 600,

                    9 => visitas.Count >= 1,
                    10 => visitas.Count >= 10,
                    11 => visitas.Count >= 30,
                    12 => visitas.Count >= 100,
                    13 => racha >= 3,
                    14 => racha >= 7,
                    18 => RegresoAlGimnasio(visitas),

                    19 => objetivosCompletados.Count >= 1,
                    20 => objetivosCompletados.Count >= 5,
                    21 => objetivosCompletados.Count >= 10,
                    22 => objetivosCompletados.Any(o =>
                        o.TipoObjetivo == "Peso"),
                    23 => objetivosCompletados.Any(o =>
                        o.TipoObjetivo == "Rutinas completadas"),
                    24 => objetivosCompletados.Any(o =>
                        o.TipoObjetivo == "Visitas al gimnasio"),
                    26 => objetivos.Count >= 3 &&
                        objetivos.All(o => o.Completado),

                    27 => medidas.Count >= 5,
                    28 => medidas.Count >= 10,
                    29 => medidas
                        .Select(m => new
                        {
                            m.Fecha.Year,
                            m.Fecha.Month
                        })
                        .Distinct()
                        .Count() >= 3,

                    30 => invitados.Count >= 1,
                    31 => invitados.Count >= 5,
                    32 => invitados
                        .Where(i =>
                            !string.IsNullOrWhiteSpace(
                                i.NombreInvitado))
                        .Select(i =>
                            i.NombreInvitado.Trim())
                        .Distinct(
                            StringComparer.OrdinalIgnoreCase)
                        .Count() >= 10,

                    33 => cumpleAtletaVersatil,

                    35 => TieneInactividad(visitas),
                    36 => objetivos.Any(o =>
                        !o.Completado &&
                        o.FechaLimite.Date < DateTime.Today),

                    _ => false
                };

                if (cumple)
                {
                    registros.Add(new InsigniaObtenida
                    {
                        Id = siguienteId++,
                        IdCliente = idCliente,
                        IdInsignia = insignia.Id,
                        FechaObtencion = DateTime.Now
                    });

                    nuevasInsignias.Add(insignia);
                }
            }

            Insignia insigniaMesActivo =
                catalogo.First(i => i.Id == 15);
            var mesesActivos = visitas
                .GroupBy(v => new
                {
                    v.FechaVisita.Year,
                    v.FechaVisita.Month
                })
                .Where(g => g.Count() >= 15);

            foreach (var mes in mesesActivos)
            {
                DateTime inicio = new DateTime(
                    mes.Key.Year,
                    mes.Key.Month,
                    1);

                DateTime fin =
                    inicio.AddMonths(1).AddDays(-1);

                bool yaObtenida = registros.Any(i =>
                    i.IdCliente == idCliente &&
                    i.IdInsignia == 15 &&
                    i.PeriodoInicio == inicio);

                if (!yaObtenida)
                {
                    registros.Add(new InsigniaObtenida
                    {
                        Id = siguienteId++,
                        IdCliente = idCliente,
                        IdInsignia = 15,
                        FechaObtencion = DateTime.Now,
                        PeriodoInicio = inicio,
                        PeriodoFin = fin
                    });
                    nuevasInsignias.Add(insigniaMesActivo);
                }
            }

            if (nuevasInsignias.Count > 0)
            {
                repositorioInsignias.GuardarTodos(registros);
            }

            return nuevasInsignias;
        }

        private int CalcularRachaMaxima(
            List<RegistroVisita> visitas)
        {
            var fechas = visitas
                .Select(v => v.FechaVisita.Date)
                .Distinct()
                .OrderBy(f => f)
                .ToList();

            if (fechas.Count == 0)
            {
                return 0;
            }

            int rachaActual = 1;
            int rachaMaxima = 1;

            for (int i = 1; i < fechas.Count; i++)
            {
                int diferencia =
                    (fechas[i] - fechas[i - 1]).Days;

                if (diferencia == 1)
                {
                    rachaActual++;
                }
                else
                {
                    rachaActual = 1;
                }

                rachaMaxima = Math.Max(
                    rachaMaxima,
                    rachaActual);
            }

            return rachaMaxima;
        }

        private bool RegresoAlGimnasio(
            List<RegistroVisita> visitas)
        {
            var fechas = visitas
                .Select(v => v.FechaVisita.Date)
                .Distinct()
                .OrderBy(f => f)
                .ToList();

            for (int i = 1; i < fechas.Count; i++)
            {
                int diasSinAsistir =
                    (fechas[i] - fechas[i - 1]).Days - 1;

                if (diasSinAsistir >= 15)
                {
                    return true;
                }
            }

            return false;
        }

        private bool TieneInactividad(
            List<RegistroVisita> visitas)
        {
            if (visitas.Count == 0)
            {
                return false;
            }

            DateTime ultimaVisita = visitas
                .Max(v => v.FechaVisita)
                .Date;

            int diasSinAsistir =
                (DateTime.Today - ultimaVisita).Days - 1;

            return diasSinAsistir >= 15;
        }

        private bool EstaDisponible(int idInsignia)
        {
            int[] pendientes =
            {
        8, 16, 17, 25, 34, 37, 38
    };

            return !pendientes.Contains(idInsignia);
        }

    }
}

