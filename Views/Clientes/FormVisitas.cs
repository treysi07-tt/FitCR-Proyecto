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
    public partial class FormVisitas : Form
    {
        private readonly Cliente cliente;
        private readonly ControladorVisitas controlador;
        private Label labelTotal;
        private DataGridView tablaVisitas;
        public FormVisitas(Cliente clienteRecibido)
        {
            InitializeComponent();

            cliente = clienteRecibido;

            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "visitas.json"
            );

            RepositorioJson<RegistroVisita> repositorio =
                new RepositorioJson<RegistroVisita>(ruta);

            controlador = new ControladorVisitas(repositorio);

            Text = "Mis visitas";
            ClientSize = new Size(750, 600);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Mis visitas al gimnasio";
            labelTitulo.Location = new Point(190, 25);
            labelTitulo.Size = new Size(450, 45);
            labelTitulo.Font = new Font(
                "Segoe UI", 18, FontStyle.Bold
            );

            Label labelCliente = new Label();
            labelCliente.Text = "Cliente: " + cliente.Nombre;
            labelCliente.Location = new Point(50, 95);
            labelCliente.Size = new Size(600, 30);

            labelTotal = new Label();
            labelTotal.Location = new Point(50, 145);
            labelTotal.Size = new Size(600, 35);
            labelTotal.Font = new Font(
                "Segoe UI", 12, FontStyle.Bold
            );

            Button buttonRegistrar = new Button();
            buttonRegistrar.Text = "Registrar visita de hoy";
            buttonRegistrar.Location = new Point(225, 200);
            buttonRegistrar.Size = new Size(300, 45);
            buttonRegistrar.Click += buttonRegistrar_Click;

            Label labelHistorial = new Label();
            labelHistorial.Text = "Historial de visitas";
            labelHistorial.Location = new Point(50, 275);
            labelHistorial.Size = new Size(350, 35);
            labelHistorial.Font = new Font(
                "Segoe UI", 14, FontStyle.Bold
            );

            tablaVisitas = new DataGridView();
            tablaVisitas.Location = new Point(50, 320);
            tablaVisitas.Size = new Size(650, 180);
            tablaVisitas.ReadOnly = true;
            tablaVisitas.AllowUserToAddRows = false;
            tablaVisitas.AllowUserToDeleteRows = false;
            tablaVisitas.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            tablaVisitas.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            tablaVisitas.RowHeadersVisible = false;

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver al menú";
            buttonVolver.Location = new Point(275, 525);
            buttonVolver.Size = new Size(200, 45);
            buttonVolver.Click += (sender, e) => Close();

            Controls.Add(labelTitulo);
            Controls.Add(labelCliente);
            Controls.Add(labelTotal);
            Controls.Add(buttonRegistrar);
            Controls.Add(labelHistorial);
            Controls.Add(tablaVisitas);
            Controls.Add(buttonVolver);

            CargarVisitas();
        }

        private void CargarVisitas()
        {
            try
            {
                int total = controlador.ContarVisitas(cliente.Id);

                labelTotal.Text =
                    "Total de visitas registradas: " + total;

                var historial =
                    controlador.ObtenerHistorial(cliente.Id);

                tablaVisitas.DataSource = historial
                    .ConvertAll(v => new
                    {
                        v.Id,
                        Fecha = v.FechaVisita
                    });

                DataGridViewColumn? columnaFecha =
                    tablaVisitas.Columns["Fecha"];

                if (columnaFecha != null)
                {
                    columnaFecha.HeaderText = "Fecha de visita";
                    columnaFecha.DefaultCellStyle.Format =
                        "dd/MM/yyyy HH:mm";
                }

                tablaVisitas.ClearSelection();
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "No se pudo cargar el historial de visitas."
                );
            }
        }

        private void buttonRegistrar_Click(object? sender, EventArgs e)
        {
            try
            {
                controlador.RegistrarVisita(cliente.Id);

                MessageBox.Show(
                    "Visita registrada correctamente.",
                    "Fit.CR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarVisitas();
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
                    "No se pudo guardar la visita."
                );
            }
        }
    }
}

