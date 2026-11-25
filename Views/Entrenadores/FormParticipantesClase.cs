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
    public partial class FormParticipantesClase : Form
    {
        private readonly Entrenador entrenador;
        private readonly int idClase;
        private readonly ControladorClases controlador;
        private DataGridView tablaParticipantes =
            new DataGridView();
        private Label labelInformacion = new Label();
        private Label labelEstado = new Label();
        private Label labelCantidad = new Label();
        private Button buttonRegistrarAsistencia = new Button();

        public FormParticipantesClase(
            Entrenador entrenadorRecibido,
            int idClaseRecibido)
        {
            InitializeComponent();

            entrenador = entrenadorRecibido;
            idClase = idClaseRecibido;

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
            CargarParticipantes();
        }

        private void ConfigurarFormulario()
        {
            Text = "Participantes y asistencia";
            ClientSize = new Size(1100, 750);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
            AutoScroll = true;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Participantes y asistencia";
            labelTitulo.Location = new Point(30, 5);
            labelTitulo.Size = new Size(1040, 65);
            labelTitulo.Font = new Font(
                "Segoe UI", 24, FontStyle.Bold
            );
            labelTitulo.TextAlign =
                ContentAlignment.MiddleCenter;

            Label labelEntrenador = new Label();
            labelEntrenador.Text =
                "Entrenador: " + entrenador.Nombre;
            labelEntrenador.Location = new Point(30, 80);
            labelEntrenador.Size = new Size(1000, 30);
            labelEntrenador.Font = new Font("Segoe UI", 11);

            labelInformacion.Location = new Point(30, 120);
            labelInformacion.Size = new Size(1040, 65);
            labelInformacion.Font = new Font("Segoe UI", 11);

            labelEstado.Location = new Point(30, 190);
            labelEstado.Size = new Size(1040, 30);
            labelEstado.Font = new Font(
                "Segoe UI", 11, FontStyle.Bold
            );

            labelCantidad.Location = new Point(30, 230);
            labelCantidad.Size = new Size(1040, 35);
            labelCantidad.Font = new Font(
                "Segoe UI", 11, FontStyle.Bold
            );

            tablaParticipantes.Location =
                new Point(30, 280);

            tablaParticipantes.Size =
                new Size(1040, 320);

            tablaParticipantes.ReadOnly = true;
            tablaParticipantes.AllowUserToAddRows = false;
            tablaParticipantes.AllowUserToDeleteRows = false;
            tablaParticipantes.RowHeadersVisible = false;
            tablaParticipantes.MultiSelect = false;

            tablaParticipantes.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            tablaParticipantes.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            buttonRegistrarAsistencia.Text =
                "Registrar asistencia";

            buttonRegistrarAsistencia.Location =
                new Point(30, 640);

            buttonRegistrarAsistencia.Size =
                new Size(300, 55);

            buttonRegistrarAsistencia.Font =
                new Font("Segoe UI", 11);

            buttonRegistrarAsistencia.Click +=
                buttonRegistrarAsistencia_Click;

            Button buttonActualizar = new Button();
            buttonActualizar.Text = "Actualizar";
            buttonActualizar.Location = new Point(400, 640);
            buttonActualizar.Size = new Size(300, 55);
            buttonActualizar.Font = new Font("Segoe UI", 11);
            buttonActualizar.Click += (sender, e) =>
                CargarParticipantes();

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver";
            buttonVolver.Location = new Point(770, 640);
            buttonVolver.Size = new Size(300, 55);
            buttonVolver.Font = new Font("Segoe UI", 11);
            buttonVolver.Click += (sender, e) => Close();

            Controls.Add(labelTitulo);
            Controls.Add(labelEntrenador);
            Controls.Add(labelInformacion);
            Controls.Add(labelEstado);
            Controls.Add(labelCantidad);
            Controls.Add(tablaParticipantes);
            Controls.Add(buttonRegistrarAsistencia);
            Controls.Add(buttonActualizar);
            Controls.Add(buttonVolver);
        }

        private void CargarParticipantes()
        {
            buttonRegistrarAsistencia.Enabled = false;

            try
            {
                ClaseGimnasio clase =
                    controlador.ObtenerClase(idClase);

                var participantes = controlador
                    .ObtenerParticipantes(
                        idClase,
                        entrenador.Id
                    );

                var asistencias = controlador
                    .ObtenerAsistencias(idClase);

                var idsAsistencias = asistencias
                    .Select(a => a.IdCliente)
                    .ToHashSet();

                int cantidadAsistentes = participantes
                    .Count(c => idsAsistencias.Contains(c.Id));

                string estado =
                    controlador.ObtenerEstadoClase(clase);

                labelInformacion.Text =
                    "Clase: " + clase.Nombre +
                    " | Fecha: " +
                    clase.FechaHora.ToString("dd/MM/yyyy HH:mm") +
                    " | Duración: " +
                    clase.Duracion + " minutos" +
                    Environment.NewLine +
                    "Descripción: " + clase.Descripcion;

                labelEstado.Text =
                    "Estado de la clase: " + estado;

                labelCantidad.Text =
                    "Inscritos: " + participantes.Count +
                    " | Cupo máximo: " + clase.CupoMaximo +
                    " | Asistencias registradas: " +
                    cantidadAsistentes +
                    " | Sin registrar: " +
                    (participantes.Count - cantidadAsistentes);

                tablaParticipantes.DataSource = null;

                tablaParticipantes.DataSource =
                    participantes.Select(c => new
                    {
                        c.Id,
                        c.Nombre,
                        c.NombreUsuario,
                        c.Edad,

                        Asistencia = idsAsistencias.Contains(c.Id)
                            ? "Registrada"
                            : "Sin registrar",

                        FechaAsistencia = asistencias
                            .FirstOrDefault(a =>
                                a.IdCliente == c.Id)
                            ?.FechaAsistencia
                            .ToString("dd/MM/yyyy HH:mm") ?? "-"
                    })
                    .ToList();

                CambiarTituloColumna(
                    "NombreUsuario",
                    "Nombre de usuario"
                );

                CambiarTituloColumna(
                    "FechaAsistencia",
                    "Fecha de asistencia"
                );

                foreach (DataGridViewRow fila
                    in tablaParticipantes.Rows)
                {
                    string asistencia = Convert.ToString(
                        fila.Cells["Asistencia"].Value
                    ) ?? "";

                    if (asistencia == "Registrada")
                    {
                        fila.DefaultCellStyle.BackColor =
                            Color.Honeydew;
                    }
                }

                buttonRegistrarAsistencia.Enabled =
                    !clase.Cancelada &&
                    clase.FechaHora <= DateTime.Now &&
                    cantidadAsistentes < participantes.Count;

                tablaParticipantes.ClearSelection();
            }
            catch (Exception error)
            {
                tablaParticipantes.DataSource = null;

                MessageBox.Show(
                    error.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void buttonRegistrarAsistencia_Click(
            object? sender, EventArgs e)
        {
            if (tablaParticipantes.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Debes seleccionar un participante.",
                    "Participante requerido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            int idCliente = Convert.ToInt32(
                tablaParticipantes.SelectedRows[0]
                    .Cells["Id"].Value
            );

            string nombreCliente = Convert.ToString(
                tablaParticipantes.SelectedRows[0]
                    .Cells["Nombre"].Value
            ) ?? "";

            DialogResult respuesta = MessageBox.Show(
                "¿Deseas registrar la asistencia de " +
                nombreCliente + "?",
                "Confirmar asistencia",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                controlador.RegistrarAsistencia(
                    idClase,
                    idCliente,
                    entrenador.Id
                );

                MessageBox.Show(
                    "Asistencia registrada correctamente.",
                    "Fit.CR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarParticipantes();
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
                tablaParticipantes.Columns[nombreColumna];

            if (columna != null)
            {
                columna.HeaderText = titulo;
            }
        }
    }
}
