using System;
using System.IO;
using System.Linq;
using FitCR.Models.Usuarios;
using FitCR.Repositories;

namespace FitCR.Services
{
    public class ServicioUsuarios
    {
        private readonly RepositorioJson<Cliente> clientes;
        private readonly RepositorioJson<EntrenadorPersonal>entrenadoresPersonales;

        private readonly RepositorioJson<EntrenadorGimnasio>entrenadoresGimnasio;

        public ServicioUsuarios()
        {
            string rutaData = Path.Combine(
                AppContext.BaseDirectory, "Data");

            clientes = new RepositorioJson<Cliente>(
                Path.Combine(rutaData, "clientes.json"));

            entrenadoresPersonales =
                new RepositorioJson<EntrenadorPersonal>(
                    Path.Combine(
                        rutaData,
                        "entrenadores_personales.json"));

            entrenadoresGimnasio =
                new RepositorioJson<EntrenadorGimnasio>(
                    Path.Combine(
                        rutaData,
                        "entrenadores_gimnasio.json"));
        }

        public bool ExisteNombreUsuario(string nombreUsuario)
        {
            string nombre = nombreUsuario.Trim();

            bool existeCliente = clientes.ObtenerTodos()
                .Any(c => string.Equals(
                    c.NombreUsuario.Trim(),
                    nombre,
                    StringComparison.OrdinalIgnoreCase));

            bool existePersonal =
                entrenadoresPersonales.ObtenerTodos()
                .Any(e => string.Equals(
                    e.NombreUsuario.Trim(),
                    nombre,
                    StringComparison.OrdinalIgnoreCase));

            bool existeGimnasio =
                entrenadoresGimnasio.ObtenerTodos()
                .Any(e => string.Equals(
                    e.NombreUsuario.Trim(),
                    nombre,
                    StringComparison.OrdinalIgnoreCase));

            return existeCliente ||
                   existePersonal ||
                   existeGimnasio;
        }
    }
}
