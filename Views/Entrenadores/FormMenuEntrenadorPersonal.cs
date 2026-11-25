using System;
using System.Drawing;
using System.Windows.Forms;
using FitCR.Models.Usuarios;

namespace FitCR.Views.Entrenadores
{
    public partial class FormMenuEntrenadorPersonal : Form
    {
        private readonly EntrenadorPersonal entrenador;

        public FormMenuEntrenadorPersonal(
            EntrenadorPersonal entrenadorRecibido)
        {
            entrenador = entrenadorRecibido;

            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            Text = "Entrenador personal";
            Size = new Size(650, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
            AutoScroll = true;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Fit.CR";
            labelTitulo.Font = new Font(
                "Segoe UI", 26, FontStyle.Bold);
            labelTitulo.Location = new Point(50, 20);
            labelTitulo.Size = new Size(530, 60);
            labelTitulo.TextAlign =
                ContentAlignment.MiddleCenter;

            Label labelBienvenida = new Label();
            labelBienvenida.Text =
                "Bienvenido/a, " + entrenador.Nombre;
            labelBienvenida.Font =
                new Font("Segoe UI", 16);
            labelBienvenida.Location =
                new Point(50, 85);
            labelBienvenida.Size = new Size(530, 40);
            labelBienvenida.TextAlign =
                ContentAlignment.MiddleCenter;

            Label labelTipo = new Label();
            labelTipo.Text =
                entrenador.ObtenerTipoUsuario();
            labelTipo.Font = new Font("Segoe UI", 12);
            labelTipo.Location = new Point(50, 130);
            labelTipo.Size = new Size(530, 30);
            labelTipo.TextAlign =
                ContentAlignment.MiddleCenter;

            Label labelClase = new Label();
            labelClase.Text =
                "Clase impartida: " +
                entrenador.ClaseImpartida;
            labelClase.Font = new Font("Segoe UI", 11);
            labelClase.Location = new Point(50, 165);
            labelClase.Size = new Size(530, 30);
            labelClase.TextAlign =
                ContentAlignment.MiddleCenter;

            Button buttonClientes = new Button();
            buttonClientes.Name = "buttonClientes";
            buttonClientes.Text = "Clientes asignados";
            buttonClientes.Location = new Point(175, 230);
            buttonClientes.Size = new Size(300, 45);
            buttonClientes.Font = new Font("Segoe UI", 12);
            buttonClientes.Click += buttonClientes_Click;

            Button buttonCerrarSesion = new Button();
            buttonCerrarSesion.Name = "buttonCerrarSesion";
            buttonCerrarSesion.Text = "Cerrar sesión";
            buttonCerrarSesion.Location =
                new Point(225, 510);
            buttonCerrarSesion.Size = new Size(180, 45);
            buttonCerrarSesion.Font =
                new Font("Segoe UI", 11);
            buttonCerrarSesion.Click +=
                buttonCerrarSesion_Click;

            Button buttonClases = new Button();
            buttonClases.Name = "buttonClases";
            buttonClases.Text = "Gestionar mis clases";
            buttonClases.Location = new Point(175, 300);
            buttonClases.Size = new Size(300, 50);
            buttonClases.Font = new Font("Segoe UI", 11);
            buttonClases.Enabled = entrenador.ClaseImpartida != "Ninguna";
            buttonClases.Click += buttonClases_Click;

            Button buttonEstadisticas = new Button();
            buttonEstadisticas.Name = "buttonEstadisticas";
            buttonEstadisticas.Text = "Estadísticas generales";
            buttonEstadisticas.Location = new Point(175, 370);
            buttonEstadisticas.Size = new Size(300, 50);
            buttonEstadisticas.Font = new Font("Segoe UI", 11);
            buttonEstadisticas.Click += buttonEstadisticas_Click;

            Controls.Add(buttonClases);

            Controls.Add(labelTitulo);
            Controls.Add(labelBienvenida);
            Controls.Add(labelTipo);
            Controls.Add(labelClase);
            Controls.Add(buttonCerrarSesion);
            Controls.Add(buttonClientes);
            Controls.Add(buttonEstadisticas);
        }

        private void buttonEstadisticas_Click(
            object? sender,
            EventArgs e)
        {
            using (FormEstadisticas formulario =
                new FormEstadisticas())
            {
                formulario.ShowDialog();
            }
        }

        private void buttonClases_Click(
            object? sender, EventArgs e)
        {
            using (FormGestionClases formulario =
                new FormGestionClases(entrenador))
            {
                formulario.ShowDialog();
            }
        }

        private void buttonClientes_Click(
            object? sender, EventArgs e)
        {
            using (FormClientesAsignados formulario =
                new FormClientesAsignados(entrenador))
            {
                formulario.ShowDialog();
            }
        }

        private void buttonCerrarSesion_Click(
            object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}
