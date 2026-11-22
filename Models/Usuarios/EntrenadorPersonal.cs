using System;
using System.Collections.Generic;
using System.Text;

namespace FitCR.Models.Usuarios
{
	public class EntrenadorPersonal : Entrenador
	{
		public List<int> ClientesAsignados { get; set; } = new List<int>();

		public override string ObtenerTipoUsuario()
		{
			return "Entrenador Personal";
		}
	}
}