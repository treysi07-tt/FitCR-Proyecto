
using FitCR.Models.Usuarios;
using FitCR.Repositories;
using System.Collections.Generic;
using System.IO;

namespace FitCR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ProbarJson();
        }

        private void ProbarJson()
        {
            // Indicamos donde se guardara el archivo
            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "clientes.json"
            );

            // Creamos el repositorio de clientes
            RepositorioJson<Cliente> repositorio =
                new RepositorioJson<Cliente>(ruta);

            // Recuperamos los clientes guardados
            List<Cliente> clientes = repositorio.ObtenerTodos();

            // Comprobamos si Maria ya esta registrada
            bool existeMaria = false;

            foreach (Cliente cliente in clientes)
            {
                if (cliente.NombreUsuario == "maria01")
                {
                    existeMaria = true;
                }
            }

            // Si Maria no existe, la agregamos
            if (existeMaria == false)
            {
                Cliente maria = new Cliente();

                maria.Id = 1;
                maria.Nombre = "Maria";
                maria.NombreUsuario = "maria01";
                maria.Edad = 24;
                maria.Peso = 65;
                maria.Estatura = 1.65;

                clientes.Add(maria);

                // Guardamos la lista actualizada
                repositorio.GuardarTodos(clientes);

                MessageBox.Show("Maria fue guardada en el archivo JSON.");
            }
            else
            {
                MessageBox.Show("Maria ya estaba guardada en el archivo JSON.");
            }

            MessageBox.Show("Total de clientes: " + clientes.Count);
        }
    }
}
