using System;
using System.Collections.Generic;
using System.Linq;
using FitCR.Interfaces;
using FitCR.Models.Progreso;
using FitCR.Models.Rutinas;
using FitCR.Models.Medidas;
using FitCR.Models.Visitas;
using FitCR.Models.Objetivos;

namespace FitCR.Controllers
{
    public class ControladorProgreso
    {
        private readonly IRepositorio<Rutina> repositorioRutinas;
        private readonly IRepositorio<RegistroMedida> repositorioMedidas;
        private readonly IRepositorio<RegistroVisita> repositorioVisitas;
        private readonly IRepositorio<Objetivo> repositorioObjetivos;

        public ControladorProgreso(
            IRepositorio<Rutina> rutinas,
            IRepositorio<RegistroMedida> medidas,
            IRepositorio<RegistroVisita> visitas,
            IRepositorio<Objetivo> objetivos)
        {
            repositorioRutinas = rutinas;
            repositorioMedidas = medidas;
            repositorioVisitas = visitas;
            repositorioObjetivos = objetivos;
        }

        public ResumenProgreso ObtenerProgreso(int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException(
                    "El cliente no es válido. Intente de nuevo"
                );
            }

            ControladorProgresoObjetivos controladorObjetivos =
                new ControladorProgresoObjetivos(
                    repositorioObjetivos,
                    repositorioRutinas,
                    repositorioMedidas,
                    repositorioVisitas
                );

            controladorObjetivos.SincronizarObjetivos(idCliente);

            List<Rutina> rutinas = repositorioRutinas
                .ObtenerTodos()
                .Where(r => r.IdCliente == idCliente)
                .ToList();

            List<RegistroMedida> medidas = repositorioMedidas
                .ObtenerTodos()
                .Where(m => m.IdCliente == idCliente)
                .OrderBy(m => m.Fecha)
                .ToList();

            List<RegistroVisita> visitas = repositorioVisitas
                .ObtenerTodos()
                .Where(v => v.IdCliente == idCliente)
                .ToList();

            List<Objetivo> objetivos = repositorioObjetivos
                .ObtenerTodos()
                .Where(o => o.IdCliente == idCliente)
                .ToList();

            DateTime fechaActual = DateTime.Now;

            ResumenProgreso resumen = new ResumenProgreso
            {
                IdCliente = idCliente,

                TotalRutinas = rutinas.Count,

                RutinasCompletadas = rutinas.Count(
                    r => r.Completada
                ),

                TotalVisitas = visitas.Count,

                VisitasMesActual = visitas.Count(v =>
                    v.FechaVisita.Month == fechaActual.Month &&
                    v.FechaVisita.Year == fechaActual.Year
                ),

                TotalObjetivos = objetivos.Count,

                ObjetivosCompletados = objetivos.Count(
                    o => o.Completado
                ),

                TotalMedidas = medidas.Count
            };

            if (medidas.Count > 0)
            {
                resumen.PesoInicial = (double)medidas.First().Peso;

                resumen.PesoActual = (double)medidas.Last().Peso;
            }

            return resumen;
        }

        public List<RegistroMedida> ObtenerHistorialPeso(
            int idCliente)
        {
            return repositorioMedidas
                .ObtenerTodos()
                .Where(m => m.IdCliente == idCliente)
                .OrderBy(m => m.Fecha)
                .ToList();
        }
    }
}

