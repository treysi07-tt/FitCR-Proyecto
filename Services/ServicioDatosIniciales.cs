using System;
using System.IO;
using FitCR.Controllers;
using FitCR.Models.Usuarios;
using FitCR.Repositories;

namespace FitCR.Services
{
    public class ServicioDatosIniciales
    {
        public void CrearEntrenadoresPrueba()
        {
            string rutaData = Path.Combine(
                AppContext.BaseDirectory, "Data");

            RepositorioJson<Cliente> clientes =
                new RepositorioJson<Cliente>(
                    Path.Combine(rutaData, "clientes.json"));

            RepositorioJson<EntrenadorPersonal> personales =
                new RepositorioJson<EntrenadorPersonal>(
                    Path.Combine(
                        rutaData,
                        "entrenadores_personales.json"));

            RepositorioJson<EntrenadorGimnasio> gimnasio =
                new RepositorioJson<EntrenadorGimnasio>(
                    Path.Combine(
                        rutaData,
                        "entrenadores_gimnasio.json"));

            ControladorEntrenadores controlador =
                new ControladorEntrenadores(
                    personales,
                    gimnasio,
                    clientes);

            ServicioUsuarios servicioUsuarios =
                new ServicioUsuarios();

            if (!servicioUsuarios.ExisteNombreUsuario(
                "carlosfit"))
            {
                EntrenadorPersonal entrenadorPersonal =
                    new EntrenadorPersonal
                    {
                        Nombre = "Carlos Vargas",
                        NombreUsuario = "carlosfit",
                        Edad = 28,
                        ClaseImpartida = "Ninguna"
                    };

                controlador.RegistrarEntrenadorPersonal(
                    entrenadorPersonal,
                    "FitCR1234");
            }

            if (!servicioUsuarios.ExisteNombreUsuario(
                "andreafit"))
            {
                EntrenadorGimnasio entrenadorGimnasio =
                    new EntrenadorGimnasio
                    {
                        Nombre = "Andrea Mora",
                        NombreUsuario = "andreafit",
                        Edad = 30,
                        ClaseImpartida = "Yoga"
                    };

                controlador.RegistrarEntrenadorGimnasio(
                    entrenadorGimnasio,
                    "FitCR1234");
            }
        }
    }
}
