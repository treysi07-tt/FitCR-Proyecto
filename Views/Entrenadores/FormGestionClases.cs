using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FitCR.Controllers;
using FitCR.Models.Clases;
using FitCR.Models.Usuarios;
using FitCR.Repositories;

namespace FitCR.Views.Entrenadores
{
    public partial class FormGestionClases : Form
    {
        private readonly Entrenador entrenador;
        private readonly ControladorClases controlador;
        private TextBox textDescripcion = new TextBox();
        private DateTimePicker fechaClase = new DateTimePicker();
        private DateTimePicker horaClase = new DateTimePicker();
        private NumericUpDown numeroDuracion = new NumericUpDown();
        private NumericUpDown numeroCupo = new NumericUpDown();
        private DataGridView tablaClases = new DataGridView();
        private Label labelCantidad = new Label();

        public FormGestionClases(Entrenador entrenadorRecibido)
        {
            InitializeComponent();

            entrenador = entrenadorRecibido;

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
            Text = "Gestión de clases";
            ClientSize = new Size(1150, 800);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
            AutoScroll = true;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Gestión de clases";
            labelTitulo.Location = new Point(30, 5);
            labelTitulo.Size = new Size(1090, 65);
            labelTitulo.Font = new Font(
                "Segoe UI", 24, FontStyle.Bold
            );
            labelTitulo.TextAlign =
                ContentAlignment.MiddleCenter;

            Label labelEntrenador = new Label();
            labelEntrenador.Text =
                "Entrenador: " + entrenador.Nombre;
            labelEntrenador.Location = new Point(30, 80);
            labelEntrenador.Size = new Size(600, 30);
            labelEntrenador.Font = new Font("Segoe UI", 11);

            Label labelClase = new Label();
            labelClase.Text =
                "Clase impartida: " + entrenador.ClaseImpartida;
            labelClase.Location = new Point(30, 115);
            labelClase.Size = new Size(600, 30);
            labelClase.Font = new Font("Segoe UI", 11);

            Label labelDescripcion = new Label();
            labelDescripcion.Text = "Descripción:";
            labelDescripcion.Location = new Point(30, 160);
            labelDescripcion.Size = new Size(300, 25);

            textDescripcion.Location = new Point(30, 190);
            textDescripcion.Size = new Size(500, 90);
            textDescripcion.Multiline = true;
            textDescripcion.MaxLength = 500;

            Label labelFecha = new Label();
            labelFecha.Text = "Fecha de la clase:";
            labelFecha.Location = new Point(570, 160);
            labelFecha.Size = new Size(250, 25);

            fechaClase.Location = new Point(570, 190);
            fechaClase.Size = new Size(250, 35);
            fechaClase.Format = DateTimePickerFormat.Short;
            fechaClase.MinDate = DateTime.Today;
            fechaClase.Value = DateTime.Today.AddDays(1);

            Label labelHora = new Label();
            labelHora.Text = "Hora de inicio:";
            labelHora.Location = new Point(850, 160);
            labelHora.Size = new Size(250, 25);

            horaClase.Location = new Point(850, 190);
            horaClase.Size = new Size(250, 35);
            horaClase.Format = DateTimePickerFormat.Custom;
            horaClase.CustomFormat = "HH:mm";
            horaClase.ShowUpDown = true;
            horaClase.Value = DateTime.Today.AddHours(8);

            Label labelDuracion = new Label();
            labelDuracion.Text = "Duración en minutos:";
            labelDuracion.Location = new Point(570, 235);
            labelDuracion.Size = new Size(250, 25);

            numeroDuracion.Location = new Point(570, 265);
            numeroDuracion.Size = new Size(250, 35);
            numeroDuracion.Minimum = 15;
            numeroDuracion.Maximum = 180;
            numeroDuracion.Increment = 15;
            numeroDuracion.Value = 60;

            Label labelCupo = new Label();
            labelCupo.Text = "Cupo máximo:";
            labelCupo.Location = new Point(850, 235);
            labelCupo.Size = new Size(250, 25);

            numeroCupo.Location = new Point(850, 265);
            numeroCupo.Size = new Size(250, 35);
            numeroCupo.Minimum = 1;
            numeroCupo.Maximum = 100;
            numeroCupo.Value = 20;

            Button buttonProgramar = new Button();
            buttonProgramar.Text = "Programar clase";
            buttonProgramar.Location = new Point(30, 310);
            buttonProgramar.Size = new Size(250, 45);
            buttonProgramar.Font = new Font("Segoe UI", 10);
            buttonProgramar.Click += buttonProgramar_Click;

            buttonProgramar.Enabled =
                entrenador.ClaseImpartida != "Ninguna";

            labelCantidad.Location = new Point(30, 375);
            labelCantidad.Size = new Size(900, 30);
            labelCantidad.Font = new Font(
                "Segoe UI", 11, FontStyle.Bold
            );

            tablaClases.Location = new Point(30, 415);
            tablaClases.Size = new Size(1090, 280);
            tablaClases.ReadOnly = true;
            tablaClases.AllowUserToAddRows = false;
            tablaClases.AllowUserToDeleteRows = false;
            tablaClases.RowHeadersVisible = false;
            tablaClases.MultiSelect = false;
            tablaClases.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            tablaClases.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            Button buttonCancelar = new Button();
            buttonCancelar.Text = "Cancelar clase";
            buttonCancelar.Location = new Point(30, 720);
            buttonCancelar.Size = new Size(250, 50);
            buttonCancelar.Font = new Font("Segoe UI", 10);
            buttonCancelar.Click += buttonCancelar_Click;

            Button buttonActualizar = new Button();
            buttonActualizar.Text = "Actualizar";
            buttonActualizar.Location = new Point(320, 720);
            buttonActualizar.Size = new Size(250, 50);
            buttonActualizar.Font = new Font("Segoe UI", 10);
            buttonActualizar.Click += (sender, e) =>
                CargarClases();

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver";
            buttonVolver.Location = new Point(870, 720);
            buttonVolver.Size = new Size(250, 50);
            buttonVolver.Font = new Font("Segoe UI", 10);
            buttonVolver.Click += (sender, e) => Close();

            Button buttonParticipantes = new Button();
            buttonParticipantes.Name = "buttonParticipantes";
            buttonParticipantes.Text = "Ver participantes";
            buttonParticipantes.Location = new Point(600, 720);
            buttonParticipantes.Size = new Size(250, 50);
            buttonParticipantes.Font = new Font("Segoe UI", 10);
            buttonParticipantes.Click += buttonParticipantes_Click;

            Controls.Add(labelTitulo);
            Controls.Add(labelEntrenador);
            Controls.Add(labelClase);
            Controls.Add(labelDescripcion);
            Controls.Add(textDescripcion);
            Controls.Add(labelFecha);
            Controls.Add(fechaClase);
            Controls.Add(labelHora);
            Controls.Add(horaClase);
            Controls.Add(labelDuracion);
            Controls.Add(numeroDuracion);
            Controls.Add(labelCupo);
            Controls.Add(numeroCupo);
            Controls.Add(buttonProgramar);
            Controls.Add(labelCantidad);
            Controls.Add(tablaClases);
            Controls.Add(buttonCancelar);
            Controls.Add(buttonActualizar);
            Controls.Add(buttonVolver);
            Controls.Add(buttonParticipantes);
        }

        private void buttonParticipantes_Click(
            object? sender, EventArgs e)
        {
            if (tablaClases.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Debes seleccionar una clase.",
                    "Clase requerida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            int idClase = Convert.ToInt32(
                tablaClases.SelectedRows[0]
                    .Cells["Id"].Value
            );

            try
            {
                using (FormParticipantesClase formulario =
                    new FormParticipantesClase(
                        entrenador,
                        idClase
                    ))
                {
                    formulario.ShowDialog();
                }

                CargarClases();
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

        private void CargarClases()
        {
            try
            {
                var clases = controlador
                    .ObtenerClasesEntrenador(entrenador.Id);

                tablaClases.DataSource = null;

                tablaClases.DataSource = clases
                    .Select(c => new
                    {
                        c.Id,
                        c.Nombre,
                        c.Descripcion,
                        c.FechaHora,
                        c.Duracion,
                        c.CupoMaximo,

                        Inscritos = controlador
                            .ObtenerInscripciones(c.Id).Count,

                        Disponibles = controlador
                            .ObtenerCuposDisponibles(c.Id),

                        Estado = controlador
                            .ObtenerEstadoClase(c)
                    })
                    .ToList();

                labelCantidad.Text =
                    "Total de clases: " + clases.Count +
                    " | Programadas: " +
                    clases.Count(c =>
                        controlador.ObtenerEstadoClase(c)
                            == "Programada") +
                    " | Canceladas: " +
                    clases.Count(c => c.Cancelada);

                CambiarTituloColumna(
                    "Descripcion", "Descripción"
                );

                CambiarTituloColumna(
                    "FechaHora", "Fecha y hora"
                );

                CambiarTituloColumna(
                    "Duracion", "Duración (min)"
                );

                CambiarTituloColumna(
                    "CupoMaximo", "Cupo máximo"
                );

                DataGridViewColumn? columnaFecha =
                    tablaClases.Columns["FechaHora"];

                if (columnaFecha != null)
                {
                    columnaFecha.DefaultCellStyle.Format =
                        "dd/MM/yyyy HH:mm";
                }

                tablaClases.ClearSelection();
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

        private void buttonProgramar_Click(
            object? sender, EventArgs e)
        {
            try
            {
                DateTime fechaHora =
                    fechaClase.Value.Date.Add(
                        new TimeSpan(
                            horaClase.Value.Hour,
                            horaClase.Value.Minute,
                            0
                        )
                    );

                ClaseGimnasio nuevaClase =
                    new ClaseGimnasio
                    {
                        Descripcion =
                            textDescripcion.Text,

                        FechaHora = fechaHora,

                        Duracion =
                            (int)numeroDuracion.Value,

                        CupoMaximo =
                            (int)numeroCupo.Value
                    };

                controlador.RegistrarClase(
                    nuevaClase,
                    entrenador
                );

                MessageBox.Show(
                    "Clase programada correctamente.",
                    "Fit.CR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                textDescripcion.Clear();
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
            if (tablaClases.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Debes seleccionar una clase."
                );
                return;
            }

            int idClase = Convert.ToInt32(
                tablaClases.SelectedRows[0]
                    .Cells["Id"].Value
            );

            DialogResult respuesta = MessageBox.Show(
                "¿Deseas cancelar esta clase?",
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
                controlador.CancelarClase(
                    idClase,
                    entrenador.Id
                );

                MessageBox.Show(
                    "Clase cancelada correctamente.",
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

        private void CambiarTituloColumna(
            string nombreColumna,
            string titulo)
        {
            DataGridViewColumn? columna =
                tablaClases.Columns[nombreColumna];

            if (columna != null)
            {
                columna.HeaderText = titulo;
            }
        }
    }
}
