using System;
using System.Collections.Generic;
using System.Linq;
using FitCR.Interfaces;
using FitCR.Models.Usuarios;
using FitCR.Services;

namespace FitCR.Controllers
{
    public class ControladorEntrenadores
    {
        private readonly IRepositorio<EntrenadorPersonal>repositorioPersonales;
        private readonly IRepositorio<EntrenadorGimnasio>repositorioGimnasio;
        private readonly IRepositorio<Cliente>repositorioClientes;
        private readonly ServicioUsuarios servicioUsuarios;
        private readonly Contrasenas servicioContrasenas;

        public ControladorEntrenadores(
            IRepositorio<EntrenadorPersonal> personales,
            IRepositorio<EntrenadorGimnasio> gimnasio,
            IRepositorio<Cliente> clientes)
        {
            repositorioPersonales = personales;
            repositorioGimnasio = gimnasio;
            repositorioClientes = clientes;

            servicioUsuarios = new ServicioUsuarios();
            servicioContrasenas = new Contrasenas();
        }

        public List<EntrenadorPersonal> ObtenerEntrenadoresPersonales()
        {
            return repositorioPersonales.ObtenerTodos();
        }

        public List<EntrenadorGimnasio> ObtenerEntrenadoresGimnasio()
        {
            return repositorioGimnasio.ObtenerTodos();
        }

        public void RegistrarEntrenadorPersonal(
            EntrenadorPersonal entrenador,
            string contrasena)
        {
            ValidarEntrenador(entrenador, contrasena);

            List<EntrenadorPersonal> entrenadores =
                repositorioPersonales.ObtenerTodos();

            entrenador.Id = ObtenerSiguienteId();

            entrenador.ContrasenaHash =
                servicioContrasenas.GenerarHash(contrasena);

            entrenador.ClientesAsignados = new List<int>();

            entrenadores.Add(entrenador);

            repositorioPersonales.GuardarTodos(entrenadores);
        }

        public void RegistrarEntrenadorGimnasio(
            EntrenadorGimnasio entrenador,
            string contrasena)
        {
            ValidarEntrenador(entrenador, contrasena);

            List<EntrenadorGimnasio> entrenadores =
                repositorioGimnasio.ObtenerTodos();

            entrenador.Id = ObtenerSiguienteId();

            entrenador.ContrasenaHash =
                servicioContrasenas.GenerarHash(contrasena);

            entrenadores.Add(entrenador);

            repositorioGimnasio.GuardarTodos(entrenadores);
        }

        public List<Cliente> ObtenerClientesDisponibles()
        {
            return repositorioClientes.ObtenerTodos()
                .OrderBy(c => c.Nombre)
                .ToList();
        }

        public List<Cliente> ObtenerClientesAsignados(
            int idEntrenador)
        {
            EntrenadorPersonal? entrenador =
                repositorioPersonales.ObtenerTodos()
                .FirstOrDefault(e => e.Id == idEntrenador);

            if (entrenador == null)
            {
                throw new ArgumentException(
                    "No se encontró el entrenador.");
            }

            return repositorioClientes.ObtenerTodos()
                .Where(c => entrenador.ClientesAsignados
                    .Contains(c.Id))
                .OrderBy(c => c.Nombre)
                .ToList();
        }

        public void AsignarCliente(
            int idEntrenador,
            int idCliente)
        {
            List<EntrenadorPersonal> entrenadores =
                repositorioPersonales.ObtenerTodos();

            EntrenadorPersonal? entrenador =
                entrenadores.FirstOrDefault(
                    e => e.Id == idEntrenador);

            if (entrenador == null)
            {
                throw new ArgumentException(
                    "No se encontró el entrenador.");
            }

            bool existeCliente =
                repositorioClientes.ObtenerTodos()
                .Any(c => c.Id == idCliente);

            if (!existeCliente)
            {
                throw new ArgumentException(
                    "No se encontró el cliente.");
            }

            if (entrenador.ClientesAsignados.Contains(idCliente))
            {
                throw new ArgumentException(
                    "El cliente ya está asignado a este entrenador.");
            }

            entrenador.ClientesAsignados.Add(idCliente);

            repositorioPersonales.GuardarTodos(entrenadores);
        }

        public void RetirarCliente(
            int idEntrenador,
            int idCliente)
        {
            List<EntrenadorPersonal> entrenadores =
                repositorioPersonales.ObtenerTodos();

            EntrenadorPersonal? entrenador =
                entrenadores.FirstOrDefault(
                    e => e.Id == idEntrenador);

            if (entrenador == null)
            {
                throw new ArgumentException(
                    "No se encontró el entrenador.");
            }

            if (!entrenador.ClientesAsignados.Contains(idCliente))
            {
                throw new ArgumentException(
                    "El cliente no está asignado a este entrenador.");
            }

            entrenador.ClientesAsignados.Remove(idCliente);

            repositorioPersonales.GuardarTodos(entrenadores);
        }

        private int ObtenerSiguienteId()
        {
            int maximoPersonal =
                repositorioPersonales.ObtenerTodos()
                .Select(e => e.Id)
                .DefaultIfEmpty(0)
                .Max();

            int maximoGimnasio =
                repositorioGimnasio.ObtenerTodos()
                .Select(e => e.Id)
                .DefaultIfEmpty(0)
                .Max();

            return Math.Max(
                maximoPersonal,
                maximoGimnasio) + 1;
        }

        private void ValidarEntrenador(
            Entrenador entrenador,
            string contrasena)
        {
            if (string.IsNullOrWhiteSpace(entrenador.Nombre))
            {
                throw new ArgumentException(
                    "El nombre es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(
                entrenador.NombreUsuario))
            {
                throw new ArgumentException(
                    "El nombre de usuario es obligatorio.");
            }

            if (entrenador.Edad < 18)
            {
                throw new ArgumentException(
                    "El entrenador debe ser mayor de edad.");
            }

            if (string.IsNullOrWhiteSpace(contrasena) ||
                contrasena.Length < 8)
            {
                throw new ArgumentException(
                    "La contraseña debe contener al menos 8 caracteres.");
            }

            entrenador.Nombre = entrenador.Nombre.Trim();

            entrenador.NombreUsuario =
                entrenador.NombreUsuario.Trim();

            if (servicioUsuarios.ExisteNombreUsuario(
                entrenador.NombreUsuario))
            {
                throw new ArgumentException(
                    "Este nombre de usuario ya está registrado.");
            }

            if (string.IsNullOrWhiteSpace(
                entrenador.ClaseImpartida))
            {
                entrenador.ClaseImpartida = "Ninguna";
            }

            string[] clasesPermitidas =
            {
                "Ninguna",
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
                    "La clase seleccionada no es válida.");
            }
        }
    }
}
