using System;
using System.Collections.Generic;
using FitCR.Interfaces;
using FitCR.Models.Usuarios;
using FitCR.Services;

namespace FitCR.Controllers
{
    public class ControladorAcceso
    {
        private readonly IRepositorio<Cliente>repositorioClientes;
        private readonly IRepositorio<EntrenadorPersonal>repositorioPersonales;
        private readonly IRepositorio<EntrenadorGimnasio>repositorioGimnasio;
        private readonly Contrasenas servicio;

        public ControladorAcceso(
            IRepositorio<Cliente> clientes,
            IRepositorio<EntrenadorPersonal> personales,
            IRepositorio<EntrenadorGimnasio> gimnasio)
        {
            repositorioClientes = clientes;
            repositorioPersonales = personales;
            repositorioGimnasio = gimnasio;

            servicio = new Contrasenas();
        }

        public Usuario? IniciarSesion(
            string nombreUsuario,
            string contrasena)
        {
            List<Usuario> usuarios = new List<Usuario>();

            usuarios.AddRange(
                repositorioClientes.ObtenerTodos());

            usuarios.AddRange(
                repositorioPersonales.ObtenerTodos());

            usuarios.AddRange(
                repositorioGimnasio.ObtenerTodos());

            foreach (Usuario usuario in usuarios)
            {
                if (usuario.NombreUsuario.Equals(
                    nombreUsuario.Trim(),
                    StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(
                        usuario.ContrasenaHash))
                    {
                        return null;
                    }

                    bool contrasenaCorrecta =
                        servicio.VerificarContrasena(
                            contrasena,
                            usuario.ContrasenaHash);

                    if (contrasenaCorrecta)
                    {
                        return usuario;
                    }
                }
            }

            return null;
        }
    }
}