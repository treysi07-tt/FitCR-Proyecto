
using System;
using System.Collections.Generic;
using FitCR.Interfaces;
using FitCR.Models.Usuarios;

namespace FitCR.Controllers
{
    public class ControladorClientes
    {
        private readonly IRepositorio<Cliente> repositorio;

        public ControladorClientes(IRepositorio<Cliente> repositorioRecibido)
        {
            repositorio = repositorioRecibido;
        }

        public List<Cliente> ObtenerClientes()
        {
            return repositorio.ObtenerTodos();
        }

        public void RegistrarCliente(Cliente nuevoCliente)
        {
            // Validaciones 
            if (string.IsNullOrWhiteSpace(nuevoCliente.Nombre))
            {
                throw new ArgumentException("El nombre es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(nuevoCliente.NombreUsuario))
            {
                throw new ArgumentException("El nombre de usuario es obligatorio.");
            }

            if (nuevoCliente.Edad <= 0)
            {
                throw new ArgumentException("La edad debe ser mayor a cero.");
            }

            if (nuevoCliente.Peso <= 0 || nuevoCliente.Estatura <= 0)
            {
                throw new ArgumentException(
                    "El peso y la estatura deben ser mayores a cero."
                );
            }

            List<Cliente> clientes = repositorio.ObtenerTodos();

            foreach (Cliente cliente in clientes)
            {
                if (cliente.NombreUsuario.Equals(
                    nuevoCliente.NombreUsuario,
                    StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException(
                        "Este nombre de usuario ya esta registrado. Ingrese otro nombre."
                    );
                }
            }

            int siguienteId = 1;

            foreach (Cliente cliente in clientes)
            {
                if (cliente.Id >= siguienteId)
                {
                    siguienteId = cliente.Id + 1;
                }
            }

            nuevoCliente.Id = siguienteId;

            clientes.Add(nuevoCliente);
            repositorio.GuardarTodos(clientes);
        }
    }
}

