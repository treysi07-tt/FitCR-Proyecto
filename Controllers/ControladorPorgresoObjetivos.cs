using System;
using System.Collections.Generic;
using System.Linq;
using FitCR.Interfaces;
using FitCR.Models.Objetivos;
using FitCR.Models.Progreso;
using FitCR.Models.Rutinas;
using FitCR.Models.Medidas;
using FitCR.Models.Visitas;

namespace FitCR.Controllers
{
    public class ControladorProgresoObjetivos
    {
        private readonly IRepositorio<Objetivo> repositorioObjetivos;
        private readonly IRepositorio<Rutina> repositorioRutinas;
        private readonly IRepositorio<RegistroMedida> repositorioMedidas;
        private readonly IRepositorio<RegistroVisita> repositorioVisitas;

        public ControladorProgresoObjetivos(
            IRepositorio<Objetivo> objetivos,
            IRepositorio<Rutina> rutinas,
            IRepositorio<RegistroMedida> medidas,
            IRepositorio<RegistroVisita> visitas)
        {
            repositorioObjetivos = objetivos;
            repositorioRutinas = rutinas;
            repositorioMedidas = medidas;
            repositorioVisitas = visitas;
        }

        public List<ProgresoObjetivo> ObtenerProgresoObjetivos(
            int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException(
                    "El cliente no es válido."
                );
            }

            List<Objetivo> objetivos = repositorioObjetivos
                .ObtenerTodos()
                .Where(o => o.IdCliente == idCliente)
                .ToList();

            List<ProgresoObjetivo> resultados =
                new List<ProgresoObjetivo>();

            foreach (Objetivo objetivo in objetivos)
            {
                ProgresoObjetivo progreso =
                    CalcularProgreso(objetivo);

                resultados.Add(progreso);
            }

            return resultados;
        }

        public int SincronizarObjetivos(int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException(
                    "El cliente no es válido."
                );
            }

            List<Objetivo> objetivos =
                repositorioObjetivos.ObtenerTodos();

            int cantidadActualizada = 0;

            foreach (Objetivo objetivo in objetivos)
            {
                if (objetivo.IdCliente != idCliente)
                {
                    continue;
                }

                ProgresoObjetivo progreso =
                    CalcularProgreso(objetivo);

                bool nuevoEstado = progreso.MetaAlcanzada;

                DateTime? nuevaFecha = nuevoEstado
                    ? ObtenerFechaCumplimiento(objetivo)
                    : null;

                if (objetivo.Completado != nuevoEstado ||
                    objetivo.FechaCompletado != nuevaFecha)
                {
                    objetivo.Completado = nuevoEstado;
                    objetivo.FechaCompletado = nuevaFecha;

                    cantidadActualizada++;
                }
            }

            if (cantidadActualizada > 0)
            {
                repositorioObjetivos.GuardarTodos(objetivos);
            }

            return cantidadActualizada;
        }

        private ProgresoObjetivo CalcularProgreso(
            Objetivo objetivo)
        {
            ProgresoObjetivo progreso = new ProgresoObjetivo
            {
                IdObjetivo = objetivo.Id,
                Nombre = objetivo.Nombre,
                TipoObjetivo = objetivo.TipoObjetivo,
                ValorMeta = objetivo.ValorMeta,
                FechaLimite = objetivo.FechaLimite,
                Porcentaje = 0,
                MetaAlcanzada = false
            };

            switch (objetivo.TipoObjetivo)
            {
                case "Rutinas completadas":
                    CalcularRutinas(objetivo, progreso);
                    break;

                case "Visitas al gimnasio":
                    CalcularVisitas(objetivo, progreso);
                    break;

                case "Peso":
                    CalcularPeso(objetivo, progreso);
                    break;
            }

            progreso.Vencido =
                DateTime.Today > objetivo.FechaLimite.Date &&
                !progreso.MetaAlcanzada;

            return progreso;
        }

        private void CalcularRutinas(
            Objetivo objetivo,
            ProgresoObjetivo progreso)
        {
            int cantidad = repositorioRutinas
                .ObtenerTodos()
                .Count(r =>
                    r.IdCliente == objetivo.IdCliente &&
                    r.Completada &&
                    r.FechaCompletada.HasValue &&
                    r.FechaCompletada.Value >=
                        objetivo.FechaCreacion &&
                    r.FechaCompletada.Value.Date <=
                        objetivo.FechaLimite.Date
                );

            progreso.Unidad = "rutinas";

            CalcularCantidad(
                cantidad,
                objetivo.ValorMeta,
                progreso
            );
        }

        private void CalcularVisitas(
            Objetivo objetivo,
            ProgresoObjetivo progreso)
        {
            int cantidad = repositorioVisitas
                .ObtenerTodos()
                .Count(v =>
                    v.IdCliente == objetivo.IdCliente &&
                    v.FechaVisita >= objetivo.FechaCreacion &&
                    v.FechaVisita.Date <=
                        objetivo.FechaLimite.Date
                );

            progreso.Unidad = "visitas";

            CalcularCantidad(
                cantidad,
                objetivo.ValorMeta,
                progreso
            );
        }

        private void CalcularCantidad(
            double cantidad,
            double meta,
            ProgresoObjetivo progreso)
        {
            progreso.ValorInicial = 0;
            progreso.ValorActual = cantidad;

            if (meta <= 0)
            {
                return;
            }

            progreso.Porcentaje = Math.Clamp(
                cantidad / meta * 100,
                0,
                100
            );

            progreso.CantidadRestante =
                Math.Max(0, meta - cantidad);

            progreso.MetaAlcanzada = cantidad >= meta;
        }

        private void CalcularPeso(
            Objetivo objetivo,
            ProgresoObjetivo progreso)
        {
            progreso.Unidad = "kg";

            List<RegistroMedida> medidas = repositorioMedidas
                .ObtenerTodos()
                .Where(m =>
                    m.IdCliente == objetivo.IdCliente &&
                    m.Fecha.Date <= objetivo.FechaLimite.Date
                )
                .OrderBy(m => m.Fecha)
                .ToList();

            if (medidas.Count == 0)
            {
                return;
            }

            RegistroMedida? medidaInicial = medidas
                .LastOrDefault(m =>
                    m.Fecha <= objetivo.FechaCreacion
                );

            List<RegistroMedida> medidasPeriodo = medidas
                .Where(m =>
                    m.Fecha >= objetivo.FechaCreacion
                )
                .ToList();

            if (medidaInicial == null)
            {
                medidaInicial = medidasPeriodo.FirstOrDefault();
            }

            if (medidaInicial == null)
            {
                return;
            }

            double pesoInicial = (double)medidaInicial.Peso;
            double pesoActual = medidasPeriodo.Count > 0
                ? (double)medidasPeriodo.Last().Peso
                : pesoInicial;
            double pesoMeta = objetivo.ValorMeta;
            progreso.ValorInicial = pesoInicial;
            progreso.ValorActual = pesoActual;

            bool aumentarPeso = pesoMeta > pesoInicial;
            double diferenciaTotal =
                Math.Abs(pesoMeta - pesoInicial);

            double avance;

            if (aumentarPeso)
            {
                avance = pesoActual - pesoInicial;
                progreso.MetaAlcanzada =
                    pesoActual >= pesoMeta;
                progreso.CantidadRestante =
                    Math.Max(0, pesoMeta - pesoActual);
            }
            else
            {
                avance = pesoInicial - pesoActual;
                progreso.MetaAlcanzada =
                    pesoActual <= pesoMeta;
                progreso.CantidadRestante =
                    Math.Max(0, pesoActual - pesoMeta);
            }

            if (progreso.MetaAlcanzada)
            {
                progreso.Porcentaje = 100;
            }
            else if (diferenciaTotal > 0)
            {
                progreso.Porcentaje = Math.Clamp(
                    avance / diferenciaTotal * 100,
                    0,
                    100
                );
            }
        }

        private DateTime? ObtenerFechaCumplimiento(
            Objetivo objetivo)
        {
            if (objetivo.ValorMeta <= 0 ||
                !double.IsFinite(objetivo.ValorMeta))
            {
                return null;
            }

            switch (objetivo.TipoObjetivo)
            {
                case "Rutinas completadas":

                    var fechasRutinas = repositorioRutinas
                        .ObtenerTodos()
                        .Where(r =>
                            r.IdCliente == objetivo.IdCliente &&
                            r.Completada &&
                            r.FechaCompletada.HasValue &&
                            r.FechaCompletada.Value >=
                                objetivo.FechaCreacion &&
                            r.FechaCompletada.Value.Date <=
                                objetivo.FechaLimite.Date)
                        .Select(r =>
                            r.FechaCompletada.GetValueOrDefault())
                        .OrderBy(f => f)
                        .ToList();

                    if (fechasRutinas.Count >= objetivo.ValorMeta)
                    {
                        int posicion =
                            (int)Math.Ceiling(objetivo.ValorMeta) - 1;

                        return fechasRutinas[posicion];
                    }

                    break;

                case "Visitas al gimnasio":

                    var fechasVisitas = repositorioVisitas
                        .ObtenerTodos()
                        .Where(v =>
                            v.IdCliente == objetivo.IdCliente &&
                            v.FechaVisita >= objetivo.FechaCreacion &&
                            v.FechaVisita.Date <=
                                objetivo.FechaLimite.Date)
                        .Select(v => v.FechaVisita)
                        .OrderBy(f => f)
                        .ToList();

                    if (fechasVisitas.Count >= objetivo.ValorMeta)
                    {
                        int posicion =
                            (int)Math.Ceiling(objetivo.ValorMeta) - 1;

                        return fechasVisitas[posicion];
                    }

                    break;

                case "Peso":

                    var medidas = repositorioMedidas
                        .ObtenerTodos()
                        .Where(m =>
                            m.IdCliente == objetivo.IdCliente &&
                            m.Fecha.Date <= objetivo.FechaLimite.Date)
                        .OrderBy(m => m.Fecha)
                        .ToList();

                    var medidasPeriodo = medidas
                        .Where(m =>
                            m.Fecha >= objetivo.FechaCreacion)
                        .ToList();

                    RegistroMedida? medidaInicial = medidas
                        .LastOrDefault(m =>
                            m.Fecha <= objetivo.FechaCreacion);

                    if (medidaInicial == null)
                    {
                        medidaInicial =
                            medidasPeriodo.FirstOrDefault();
                    }

                    if (medidaInicial == null)
                    {
                        return null;
                    }

                    double pesoInicial =
                        (double)medidaInicial.Peso;

                    bool aumentarPeso =
                        objetivo.ValorMeta > pesoInicial;

                    if (pesoInicial == objetivo.ValorMeta)
                    {
                        return medidaInicial.Fecha <
                            objetivo.FechaCreacion
                            ? objetivo.FechaCreacion
                            : medidaInicial.Fecha;
                    }

                    foreach (RegistroMedida medida in medidasPeriodo)
                    {
                        double peso = (double)medida.Peso;

                        bool metaAlcanzada = aumentarPeso
                            ? peso >= objetivo.ValorMeta
                            : peso <= objetivo.ValorMeta;

                        if (metaAlcanzada)
                        {
                            return medida.Fecha;
                        }
                    }

                    break;
            }

            return null;
        }

    }
}

