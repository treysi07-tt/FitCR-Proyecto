using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FitCR.Controllers;
using FitCR.Models.Clases;
using FitCR.Models.Usuarios;
using FitCR.Repositories;

namespace FitCR.Views.Clientes
{
    public partial class FormClasesCliente : Form
    {
        private readonly Cliente cliente;
        private readonly ControladorClases controlador;
        private DataGridView tablaDisponibles = new DataGridView();
        private DataGridView tablaInscripciones = new DataGridView();
        private Label labelDisponibles = new Label();
        private Label labelInscripciones = new Label();

        public FormClasesCliente(Cliente clienteRecibido)
        {
            InitializeComponent();

            cliente = clienteRecibido;

            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data"
            );

            controlador = new ControladorClases(
                new RepositorioJson<ClaseGimnasio>(
                    Path.Combine(ruta, "clases.json")
                ),
                new RepositorioJson<InscripcionClase>(
                    Path.Combine(ruta, "inscripciones_clases.json")
                ),
                new RepositorioJson<AsistenciaClase>(
                    Path.Combine(ruta, "asistencias_clases.json")
                ),
                new RepositorioJson<Cliente>(
                    Path.Combine(ruta, "clientes.json")
                )
            );

            ConfigurarFormulario();
            CargarClases();
        }

        private void ConfigurarFormulario()
        {
            Text = "Clases del gimnasio";
            ClientSize = new Size(1150, 850);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
            AutoScroll = true;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Clases del gimnasio";
            labelTitulo.Location = new Point(30, 5);
            labelTitulo.Size = new Size(1090, 65);
            labelTitulo.Font = new Font(
                "Segoe UI", 24, FontStyle.Bold
            );
            labelTitulo.TextAlign =
                ContentAlignment.MiddleCenter;

            Label labelCliente = new Label();
            labelCliente.Text = "Cliente: " + cliente.Nombre;
            labelCliente.Location = new Point(30, 80);
            labelCliente.Size = new Size(900, 30);
            labelCliente.Font = new Font("Segoe UI", 11);

            labelDisponibles.Location = new Point(30, 120);
            labelDisponibles.Size = new Size(900, 35);
            labelDisponibles.Font = new Font(
                "Segoe UI", 13, FontStyle.Bold
            );

            tablaDisponibles.Location = new Point(30, 165);
            tablaDisponibles.Size = new Size(1090, 230);

            ConfigurarTabla(tablaDisponibles);

            Button buttonInscribir = new Button();
            buttonInscribir.Text = "Inscribirme en la clase";
            buttonInscribir.Location = new Point(30, 410);
            buttonInscribir.Size = new Size(300, 50);
            buttonInscribir.Font = new Font("Segoe UI", 10);
            buttonInscribir.Click += buttonInscribir_Click;

            labelInscripciones.Location =
                new Point(30, 480);
            labelInscripciones.Size = new Size(900, 35);
            labelInscripciones.Font = new Font(
                "Segoe UI", 13, FontStyle.Bold
            );

            tablaInscripciones.Location =
                new Point(30, 525);
            tablaInscripciones.Size = new Size(1090, 230);

            ConfigurarTabla(tablaInscripciones);

            Button buttonCancelar = new Button();
            buttonCancelar.Text = "Cancelar inscripción";
            buttonCancelar.Location = new Point(30, 775);
            buttonCancelar.Size = new Size(300, 50);
            buttonCancelar.Font = new Font("Segoe UI", 10);
            buttonCancelar.Click += buttonCancelar_Click;

            Button buttonActualizar = new Button();
            buttonActualizar.Text = "Actualizar";
            buttonActualizar.Location = new Point(450, 775);
            buttonActualizar.Size = new Size(250, 50);
            buttonActualizar.Font = new Font("Segoe UI", 10);
            buttonActualizar.Click += (sender, e) =>
                CargarClases();

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver";
            buttonVolver.Location = new Point(870, 775);
            buttonVolver.Size = new Size(250, 50);
            buttonVolver.Font = new Font("Segoe UI", 10);
            buttonVolver.Click += (sender, e) => Close();

            Controls.Add(labelTitulo);
            Controls.Add(labelCliente);
            Controls.Add(labelDisponibles);
            Controls.Add(tablaDisponibles);
            Controls.Add(buttonInscribir);
            Controls.Add(labelInscripciones);
            Controls.Add(tablaInscripciones);
            Controls.Add(buttonCancelar);
            Controls.Add(buttonActualizar);
            Controls.Add(buttonVolver);
        }

        private void ConfigurarTabla(DataGridView tabla)
        {
            tabla.ReadOnly = true;
            tabla.AllowUserToAddRows = false;
            tabla.AllowUserToDeleteRows = false;
            tabla.RowHeadersVisible = false;
            tabla.MultiSelect = false;

            tabla.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            tabla.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarClases()
        {
            try
            {
                var disponibles = controlador
                    .ObtenerClasesDisponibles()
                    .Where(c => !controlador.EstaInscrito(
                        c.Id, cliente.Id))
                    .Select(c => new
                    {
                        c.Id,
                        c.Nombre,
                        c.Descripcion,
                        c.FechaHora,
                        c.Duracion,

                        CuposDisponibles = controlador
                            .ObtenerCuposDisponibles(c.Id)
                    })
                    .ToList();

                var asistencias = controlador
                    .ObtenerAsistenciasCliente(cliente.Id)
                    .Select(a => a.IdClase)
                    .ToHashSet();

                var inscripciones = controlador
                    .ObtenerClasesCliente(cliente.Id)
                    .Select(c => new
                    {
                        c.Id,
                        c.Nombre,
                        c.Descripcion,
                        c.FechaHora,
                        c.Duracion,

                        Estado = controlador
                            .ObtenerEstadoClase(c),

                        Asistencia = asistencias.Contains(c.Id)
                            ? "Registrada"
                            : "Sin registrar"
                    })
                    .ToList();

                tablaDisponibles.DataSource = null;
                tablaDisponibles.DataSource = disponibles;

                tablaInscripciones.DataSource = null;
                tablaInscripciones.DataSource = inscripciones;

                labelDisponibles.Text =
                    "Clases disponibles: " + disponibles.Count;

                labelInscripciones.Text =
                    "Mis inscripciones: " + inscripciones.Count;

                FormatearTabla(tablaDisponibles);
                FormatearTabla(tablaInscripciones);

                CambiarTituloColumna(
                    tablaDisponibles,
                    "CuposDisponibles",
                    "Cupos disponibles"
                );

                tablaDisponibles.ClearSelection();
                tablaInscripciones.ClearSelection();
            }
            catch (Exception error)
            {
                MessageBox.Show(
                    error.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void buttonInscribir_Click(
            object? sender, EventArgs e)
        {
            if (tablaDisponibles.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Debes seleccionar una clase disponible."
                );
                return;
            }

            int idClase = Convert.ToInt32(
                tablaDisponibles.SelectedRows[0]
                    .Cells["Id"].Value
            );

            DialogResult respuesta = MessageBox.Show(
                "¿Deseas inscribirte en esta clase?",
                "Confirmar inscripción",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                controlador.InscribirCliente(
                    idClase,
                    cliente.Id
                );

                MessageBox.Show(
                    "Inscripción realizada correctamente.",
                    "Fit.CR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarClases();
            }
            catch (Exception error)
            {
                MessageBox.Show(
                    error.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void buttonCancelar_Click(
            object? sender, EventArgs e)
        {
            if (tablaInscripciones.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Debes seleccionar una inscripción."
                );
                return;
            }

            int idClase = Convert.ToInt32(
                tablaInscripciones.SelectedRows[0]
                    .Cells["Id"].Value
            );

            DialogResult respuesta = MessageBox.Show(
                "¿Deseas cancelar tu inscripción?",
                "Confirmar cancelación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                controlador.CancelarInscripcion(
                    idClase,
                    cliente.Id
                );

                MessageBox.Show(
                    "Inscripción cancelada correctamente.",
                    "Fit.CR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarClases();
            }
            catch (Exception error)
            {
                MessageBox.Show(
                    error.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void FormatearTabla(DataGridView tabla)
        {
            CambiarTituloColumna(
                tabla, "Descripcion", "Descripción"
            );

            CambiarTituloColumna(
                tabla, "FechaHora", "Fecha y hora"
            );

            CambiarTituloColumna(
                tabla, "Duracion", "Duración (min)"
            );

            DataGridViewColumn? columnaFecha =
                tabla.Columns["FechaHora"];

            if (columnaFecha != null)
            {
                columnaFecha.DefaultCellStyle.Format =
                    "dd/MM/yyyy HH:mm";
            }
        }

        private void CambiarTituloColumna(
            DataGridView tabla,
            string nombreColumna,
            string titulo)
        {
            DataGridViewColumn? columna =
                tabla.Columns[nombreColumna];

            if (columna != null)
            {
                columna.HeaderText = titulo;
            }
        }
    }
}
