using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FitCR.Models.Usuarios;
using FitCR.Models.Visitas;
using FitCR.Repositories;

namespace FitCR.Services
{
    public class ServicioEstadisticasMensuales
    {
        private readonly RepositorioJson<Cliente>repositorioClientes;
        private readonly RepositorioJson<RegistroVisita>repositorioVisitas;
        public ServicioEstadisticasMensuales()
        {
            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data"
            );

            repositorioClientes =
                new RepositorioJson<Cliente>(
                    Path.Combine(ruta, "clientes.json")
                );

            repositorioVisitas =
                new RepositorioJson<RegistroVisita>(
                    Path.Combine(ruta, "visitas.json")
                );
        }

        public List<DateTime> ObtenerMesesEvaluables(
            int idCliente)
        {
            var cliente = repositorioClientes
                .ObtenerTodos()
                .FirstOrDefault(c => c.Id == idCliente);

            List<DateTime> meses =
                new List<DateTime>();

            if (cliente == null ||
                !cliente.FechaRegistro.HasValue)
            {
                return meses;
            }

            DateTime registro =
                cliente.FechaRegistro.Value.Date;

            DateTime inicio = new DateTime(
                registro.Year,
                registro.Month,
                1
            );

            if (registro.Day != 1)
            {
                inicio = inicio.AddMonths(1);
            }

            DateTime mesActual = new DateTime(
                DateTime.Today.Year,
                DateTime.Today.Month,
                1
            );

            while (inicio < mesActual)
            {
                meses.Add(inicio);
                inicio = inicio.AddMonths(1);
            }

            return meses;
        }

        public List<int> ObtenerInsigniasMensuales(
            int idCliente,
            DateTime mes)
        {
            DateTime inicio = new DateTime(
                mes.Year,
                mes.Month,
                1
            );

            DateTime fin = inicio.AddMonths(1);

            List<int> insignias = new List<int>();

            if (fin > DateTime.Today)
            {
                return insignias;
            }

            var clientes = repositorioClientes
                .ObtenerTodos()
                .Where(c =>
                    c.FechaRegistro.HasValue &&
                    c.FechaRegistro.Value.Date <= inicio)
                .ToList();

            var visitas = repositorioVisitas
                .ObtenerTodos()
                .Where(v =>
                    v.FechaVisita >= inicio &&
                    v.FechaVisita < fin)
                .ToList();

            var clasificacion = clientes
                .Select(c => new
                {
                    c.Id,
                    c.Nombre,

                    CantidadVisitas = visitas.Count(v =>
                        v.IdCliente == c.Id)
                })
                .ToList();

            var clienteActual = clasificacion
                .FirstOrDefault(c => c.Id == idCliente);

            if (clienteActual == null)
            {
                return insignias;
            }

            var mayorAsistencia = clasificacion
                .Where(c => c.CantidadVisitas > 0)
                .OrderByDescending(c => c.CantidadVisitas)
                .ThenBy(c => c.Nombre)
                .ThenBy(c => c.Id)
                .ToList();

            var menorAsistencia = clasificacion
                .OrderBy(c => c.CantidadVisitas)
                .ThenBy(c => c.Nombre)
                .ThenBy(c => c.Id)
                .ToList();

            if (mayorAsistencia
                .Take(10)
                .Any(c => c.Id == idCliente))
            {
                insignias.Add(16);
            }

            if (mayorAsistencia.Count > 0 &&
                mayorAsistencia[0].Id == idCliente)
            {
                insignias.Add(17);
            }

            if (clienteActual.CantidadVisitas == 0)
            {
                insignias.Add(34);
            }

            if (clienteActual.CantidadVisitas < 4)
            {
                insignias.Add(37);
            }

            if (clasificacion.Count >= 20 &&
                menorAsistencia
                    .Take(10)
                    .Any(c => c.Id == idCliente))
            {
                insignias.Add(38);
            }

            return insignias;
        }
    }
}
