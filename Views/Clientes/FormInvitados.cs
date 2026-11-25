using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FitCR.Controllers;
using FitCR.Models.Usuarios;
using FitCR.Models.Visitas;
using FitCR.Repositories;

namespace FitCR.Views.Clientes
{
    public partial class FormInvitados : Form
    {
        private readonly Cliente cliente;
        private readonly ControladorInvitados controlador;
        private Label labelDisponibles;
        private Label labelUtilizadas;
        private TextBox textNombreInvitado;
        private DataGridView tablaInvitados;

        public FormInvitados(Cliente clienteRecibido)
        {
            InitializeComponent();

            cliente = clienteRecibido;

            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "invitados.json"
            );

            RepositorioJson<RegistroInvitado> repositorio =
                new RepositorioJson<RegistroInvitado>(ruta);

            controlador = new ControladorInvitados(repositorio);

            Text = "Mis invitados";
            ClientSize = new Size(850, 650);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Registro de invitados";
            labelTitulo.Location = new Point(250, 20);
            labelTitulo.Size = new Size(400, 45);
            labelTitulo.Font = new Font(
                "Segoe UI", 18, FontStyle.Bold
            );

            Label labelCliente = new Label();
            labelCliente.Text = "Cliente: " + cliente.Nombre;
            labelCliente.Location = new Point(50, 85);
            labelCliente.Size = new Size(650, 30);

            labelUtilizadas = new Label();
            labelUtilizadas.Location = new Point(50, 130);
            labelUtilizadas.Size = new Size(350, 30);
            labelUtilizadas.Font = new Font(
                "Segoe UI", 11, FontStyle.Bold
            );

            labelDisponibles = new Label();
            labelDisponibles.Location = new Point(430, 130);
            labelDisponibles.Size = new Size(350, 30);
            labelDisponibles.Font = new Font(
                "Segoe UI", 11, FontStyle.Bold
            );

            Label labelNombre = new Label();
            labelNombre.Text = "Nombre del invitado:";
            labelNombre.Location = new Point(50, 190);
            labelNombre.AutoSize = true;

            textNombreInvitado = new TextBox();
            textNombreInvitado.Location = new Point(50, 220);
            textNombreInvitado.Size = new Size(450, 30);
            textNombreInvitado.MaxLength = 100;

            Button buttonRegistrar = new Button();
            buttonRegistrar.Text = "Registrar invitado";
            buttonRegistrar.Location = new Point(540, 215);
            buttonRegistrar.Size = new Size(240, 40);
            buttonRegistrar.Click += buttonRegistrar_Click;

            Label labelHistorial = new Label();
            labelHistorial.Text = "Historial de invitados";
            labelHistorial.Location = new Point(50, 285);
            labelHistorial.Size = new Size(400, 35);
            labelHistorial.Font = new Font(
                "Segoe UI", 14, FontStyle.Bold
            );

            tablaInvitados = new DataGridView();
            tablaInvitados.Location = new Point(50, 330);
            tablaInvitados.Size = new Size(730, 220);
            tablaInvitados.ReadOnly = true;
            tablaInvitados.AllowUserToAddRows = false;
            tablaInvitados.AllowUserToDeleteRows = false;
            tablaInvitados.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            tablaInvitados.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            tablaInvitados.RowHeadersVisible = false;

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver al menú";
            buttonVolver.Location = new Point(325, 575);
            buttonVolver.Size = new Size(200, 45);
            buttonVolver.Click += (sender, e) => Close();

            Controls.Add(labelTitulo);
            Controls.Add(labelCliente);
            Controls.Add(labelUtilizadas);
            Controls.Add(labelDisponibles);
            Controls.Add(labelNombre);
            Controls.Add(textNombreInvitado);
            Controls.Add(buttonRegistrar);
            Controls.Add(labelHistorial);
            Controls.Add(tablaInvitados);
            Controls.Add(buttonVolver);
            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                int utilizadas =
                    controlador.ContarVisitasDelMes(cliente.Id);

                int disponibles =
                    controlador.ObtenerVisitasDisponibles(cliente.Id);

                labelUtilizadas.Text =
                    "Visitas de este mes: " + utilizadas + "/3";

                labelDisponibles.Text =
                    "Visitas disponibles: " + disponibles;

                var historial =
                    controlador.ObtenerHistorial(cliente.Id);

                tablaInvitados.DataSource = historial
                    .ConvertAll(i => new
                    {
                        i.Id,
                        Invitado = i.NombreInvitado,
                        Fecha = i.FechaVisita
                    });

                DataGridViewColumn? columnaFecha =
                    tablaInvitados.Columns["Fecha"];

                if (columnaFecha != null)
                {
                    columnaFecha.DefaultCellStyle.Format =
                        "dd/MM/yyyy HH:mm";
                }

                tablaInvitados.ClearSelection();
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "No se pudieron cargar los invitados."
                );
            }
        }

        private void buttonRegistrar_Click(object? sender, EventArgs e)
        {
            try
            {
                controlador.RegistrarInvitado(
                    cliente.Id,
                    textNombreInvitado.Text
                );

                MessageBox.Show(
                    "Invitado registrado correctamente.",
                    "Fit.CR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                textNombreInvitado.Clear();
                CargarDatos();
            }
            catch (ArgumentException error)
            {
                MessageBox.Show(
                    error.Message,
                    "Fit.CR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "No se pudo guardar el invitado."
                );
            }
        }
    }
}

