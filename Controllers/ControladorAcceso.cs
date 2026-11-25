using System;
using System.Collections.Generic;
using FitCR.Interfaces;
using FitCR.Models.Usuarios;
using FitCR.Services;

namespace FitCR.Controllers
{
	public class ControladorAcceso
	{
		private readonly IRepositorio<Cliente> repositorio;
		private readonly Contrasenas servicio;

		public ControladorAcceso(IRepositorio<Cliente> repositorioRecibido)
		{
			repositorio = repositorioRecibido;
			servicio = new Contrasenas();
		}

		public Cliente? IniciarSesion(string nombreUsuario, string contrasena)
		{
			List<Cliente> clientes = repositorio.ObtenerTodos();

			foreach (Cliente cliente in clientes)
			{
				// Validamos
				if (cliente.NombreUsuario.Equals(
					nombreUsuario.Trim(),
					StringComparison.OrdinalIgnoreCase))
				{
					if (string.IsNullOrEmpty(cliente.ContrasenaHash))
					{
						return null;
					}

					bool contrasenaCorrecta = servicio.VerificarContrasena(
						contrasena,
						cliente.ContrasenaHash
					);

					if (contrasenaCorrecta)
					{
						return cliente;
					}
				}
			}

			return null;
		}
	}
}

