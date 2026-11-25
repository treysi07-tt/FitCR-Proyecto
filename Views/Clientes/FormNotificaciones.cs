using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FitCR.Controllers;
using FitCR.Models.Usuarios;
using FitCR.Models.Notificaciones;
using FitCR.Repositories;
using FitCR.Services;

namespace FitCR.Views.Clientes
{
    public partial class FormNotificaciones : Form
    {
        private readonly Cliente cliente;
        private readonly ControladorNotificaciones controlador;
        private readonly ServicioNotificaciones servicio;
        private Label labelTotal = new Label();
        private Label labelSinLeer = new Label();
        private Label labelLeidas = new Label();
        private ComboBox comboEstado = new ComboBox();
        private ComboBox comboTipo = new ComboBox();
        private DataGridView tablaNotificaciones = new DataGridView();
        private TextBox textMensaje = new TextBox();

        public FormNotificaciones(Cliente clienteRecibido)
        {
            InitializeComponent();

            cliente = clienteRecibido;

            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "notificaciones.json");

            RepositorioJson<Notificacion> repositorio =
                new RepositorioJson<Notificacion>(ruta);

            controlador =
                new ControladorNotificaciones(repositorio);

            servicio = new ServicioNotificaciones();

            ConfigurarFormulario();
            ActualizarNotificaciones();
        }

        private void ConfigurarFormulario()
        {
            Controls.Clear();

            Text = "Notificaciones";
            Size = new Size(1150, 820);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
            AutoScroll = true;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Mis notificaciones";
            labelTitulo.Font = new Font(
                "Segoe UI", 24, FontStyle.Bold);
            labelTitulo.Location = new Point(40, 20);
            labelTitulo.Size = new Size(1050, 55);
            labelTitulo.TextAlign =
                ContentAlignment.MiddleCenter;

            Label labelSubtitulo = new Label();
            labelSubtitulo.Text =
                "Consulta tu historial de actividades en Fit.CR";
            labelSubtitulo.Font = new Font("Segoe UI", 12);
            labelSubtitulo.Location = new Point(40, 80);
            labelSubtitulo.Size = new Size(1050, 30);
            labelSubtitulo.TextAlign =
                ContentAlignment.MiddleCenter;

            labelTotal.Location = new Point(50, 130);
            labelTotal.Size = new Size(300, 35);
            labelTotal.Font = new Font(
                "Segoe UI", 13, FontStyle.Bold);

            labelSinLeer.Location = new Point(400, 130);
            labelSinLeer.Size = new Size(300, 35);
            labelSinLeer.Font = new Font(
                "Segoe UI", 13, FontStyle.Bold);

            labelLeidas.Location = new Point(750, 130);
            labelLeidas.Size = new Size(300, 35);
            labelLeidas.Font = new Font(
                "Segoe UI", 13, FontStyle.Bold);

            Label labelEstado = new Label();
            labelEstado.Text = "Estado:";
            labelEstado.Location = new Point(50, 185);
            labelEstado.Size = new Size(80, 30);
            labelEstado.Font = new Font("Segoe UI", 11);

            comboEstado.Location = new Point(140, 185);
            comboEstado.Size = new Size(250, 30);
            comboEstado.Font = new Font("Segoe UI", 11);
            comboEstado.DropDownStyle =
                ComboBoxStyle.DropDownList;

            comboEstado.Items.AddRange(new object[]
            {
                "Todas",
                "Sin leer",
                "Leídas"
            });

            comboEstado.SelectedIndex = 0;

            Label labelTipo = new Label();
            labelTipo.Text = "Categoría:";
            labelTipo.Location = new Point(430, 185);
            labelTipo.Size = new Size(100, 30);
            labelTipo.Font = new Font("Segoe UI", 11);

            comboTipo.Location = new Point(540, 185);
            comboTipo.Size = new Size(250, 30);
            comboTipo.Font = new Font("Segoe UI", 11);
            comboTipo.DropDownStyle =
                ComboBoxStyle.DropDownList;

            comboTipo.Items.AddRange(new object[]
            {
                "Todas",
                "Acceso",
                "Rutinas",
                "Visitas",
                "Invitados",
                "Medidas",
                "Objetivos",
                "Insignias",
                "Clases"
            }); 

            comboTipo.SelectedIndex = 0;

            Button buttonActualizar = new Button();
            buttonActualizar.Text = "Actualizar";
            buttonActualizar.Location = new Point(850, 180);
            buttonActualizar.Size = new Size(230, 40);
            buttonActualizar.Font = new Font("Segoe UI", 11);
            buttonActualizar.Click += buttonActualizar_Click;

            tablaNotificaciones.Location = new Point(50, 240);
            tablaNotificaciones.Size = new Size(1030, 300);
            tablaNotificaciones.ReadOnly = true;
            tablaNotificaciones.AllowUserToAddRows = false;
            tablaNotificaciones.AllowUserToDeleteRows = false;
            tablaNotificaciones.MultiSelect = false;
            tablaNotificaciones.RowHeadersVisible = false;
            tablaNotificaciones.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            tablaNotificaciones.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            tablaNotificaciones.BackgroundColor = Color.White;
            tablaNotificaciones.Font =
                new Font("Segoe UI", 10);

            tablaNotificaciones.SelectionChanged +=
                tablaNotificaciones_SelectionChanged;

            Label labelMensaje = new Label();
            labelMensaje.Text = "Detalle de la notificación";
            labelMensaje.Location = new Point(50, 560);
            labelMensaje.Size = new Size(500, 30);
            labelMensaje.Font = new Font(
                "Segoe UI", 12, FontStyle.Bold);

            textMensaje.Location = new Point(50, 595);
            textMensaje.Size = new Size(1030, 90);
            textMensaje.Multiline = true;
            textMensaje.ReadOnly = true;
            textMensaje.ScrollBars = ScrollBars.Vertical;
            textMensaje.BackColor = Color.White;
            textMensaje.Font = new Font("Segoe UI", 11);

            Button buttonMarcarLeida = new Button();
            buttonMarcarLeida.Text = "Marcar como leída";
            buttonMarcarLeida.Location = new Point(50, 710);
            buttonMarcarLeida.Size = new Size(300, 45);
            buttonMarcarLeida.Font = new Font("Segoe UI", 11);
            buttonMarcarLeida.Click += buttonMarcarLeida_Click;

            Button buttonMarcarTodas = new Button();
            buttonMarcarTodas.Text = "Marcar todas como leídas";
            buttonMarcarTodas.Location = new Point(370, 710);
            buttonMarcarTodas.Size = new Size(350, 45);
            buttonMarcarTodas.Font = new Font("Segoe UI", 11);
            buttonMarcarTodas.Click += buttonMarcarTodas_Click;

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver";
            buttonVolver.Location = new Point(850, 710);
            buttonVolver.Size = new Size(230, 45);
            buttonVolver.Font = new Font("Segoe UI", 11);
            buttonVolver.Click += buttonVolver_Click;

            comboEstado.SelectedIndexChanged +=
                comboFiltro_SelectedIndexChanged;

            comboTipo.SelectedIndexChanged +=
                comboFiltro_SelectedIndexChanged;

            Controls.Add(labelTitulo);
            Controls.Add(labelSubtitulo);
            Controls.Add(labelTotal);
            Controls.Add(labelSinLeer);
            Controls.Add(labelLeidas);
            Controls.Add(labelEstado);
            Controls.Add(comboEstado);
            Controls.Add(labelTipo);
            Controls.Add(comboTipo);
            Controls.Add(buttonActualizar);
            Controls.Add(tablaNotificaciones);
            Controls.Add(labelMensaje);
            Controls.Add(textMensaje);
            Controls.Add(buttonMarcarLeida);
            Controls.Add(buttonMarcarTodas);
            Controls.Add(buttonVolver);
        }

        private void ActualizarNotificaciones()
        {
            try
            {
                servicio.SincronizarNotificaciones(cliente);
                CargarNotificaciones();
            }
            catch (Exception error)
            {
                MessageBox.Show(
                    error.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CargarNotificaciones()
        {
            var notificaciones =
                controlador.ObtenerNotificaciones(cliente.Id);

            int total = notificaciones.Count;
            int sinLeer = notificaciones.Count(n => !n.Leida);
            int leidas = notificaciones.Count(n => n.Leida);

            labelTotal.Text = "Total: " + total;
            labelSinLeer.Text = "Sin leer: " + sinLeer;
            labelLeidas.Text = "Leídas: " + leidas;

            string estado =
                comboEstado.SelectedItem?.ToString() ?? "Todas";

            string tipo =
                comboTipo.SelectedItem?.ToString() ?? "Todas";

            if (estado == "Sin leer")
            {
                notificaciones = notificaciones
                    .Where(n => !n.Leida)
                    .ToList();
            }
            else if (estado == "Leídas")
            {
                notificaciones = notificaciones
                    .Where(n => n.Leida)
                    .ToList();
            }

            if (tipo != "Todas")
            {
                notificaciones = notificaciones
                    .Where(n => n.Tipo == tipo)
                    .ToList();
            }

            tablaNotificaciones.DataSource =
                notificaciones.Select(n => new
                {
                    n.Id,
                    n.Fecha,
                    n.Titulo,
                    n.Tipo,
                    Estado = n.Leida ? "Leída" : "Sin leer",
                    n.Mensaje
                }).ToList();

            FormatearColumnas();

            tablaNotificaciones.ClearSelection();
            textMensaje.Clear();
        }

        private void FormatearColumnas()
        {
            DataGridViewColumn? columnaId =
                tablaNotificaciones.Columns["Id"];

            DataGridViewColumn? columnaMensaje =
                tablaNotificaciones.Columns["Mensaje"];

            DataGridViewColumn? columnaFecha =
                tablaNotificaciones.Columns["Fecha"];

            DataGridViewColumn? columnaTitulo =
                tablaNotificaciones.Columns["Titulo"];

            DataGridViewColumn? columnaTipo =
                tablaNotificaciones.Columns["Tipo"];

            if (columnaId != null)
            {
                columnaId.Visible = false;
            }

            if (columnaMensaje != null)
            {
                columnaMensaje.Visible = false;
            }

            if (columnaFecha != null)
            {
                columnaFecha.DefaultCellStyle.Format =
                    "dd/MM/yyyy HH:mm";
            }

            if (columnaTitulo != null)
            {
                columnaTitulo.HeaderText = "Título";
                columnaTitulo.FillWeight = 180;
            }

            if (columnaTipo != null)
            {
                columnaTipo.HeaderText = "Categoría";
            }

            foreach (DataGridViewRow fila in
                tablaNotificaciones.Rows)
            {
                string estado =
                    fila.Cells["Estado"].Value?.ToString() ?? "";

                if (estado == "Sin leer")
                {
                    fila.DefaultCellStyle.BackColor =
                        Color.AliceBlue;
                }
            }
        }

        private void tablaNotificaciones_SelectionChanged(
            object? sender, EventArgs e)
        {
            if (tablaNotificaciones.SelectedRows.Count == 0)
            {
                textMensaje.Clear();
                return;
            }

            if (tablaNotificaciones.Columns["Mensaje"] == null)
            {
                return;
            }

            textMensaje.Text =
                tablaNotificaciones.SelectedRows[0]
                .Cells["Mensaje"].Value?.ToString() ?? "";
        }

        private void comboFiltro_SelectedIndexChanged(
            object? sender, EventArgs e)
        {
            CargarNotificaciones();
        }

        private void buttonActualizar_Click(
            object? sender, EventArgs e)
        {
            ActualizarNotificaciones();
        }

        private void buttonMarcarLeida_Click(
            object? sender, EventArgs e)
        {
            try
            {
                if (tablaNotificaciones.SelectedRows.Count == 0)
                {
                    MessageBox.Show(
                        "Selecciona una notificación.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                int idNotificacion = Convert.ToInt32(
                    tablaNotificaciones.SelectedRows[0]
                    .Cells["Id"].Value);

                controlador.MarcarComoLeida(
                    idNotificacion, cliente.Id);

                CargarNotificaciones();

                MessageBox.Show(
                    "Notificación marcada como leída.",
                    "Notificaciones",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception error)
            {
                MessageBox.Show(
                    error.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void buttonMarcarTodas_Click(
            object? sender, EventArgs e)
        {
            try
            {
                int cantidad =
                    controlador.MarcarTodasComoLeidas(cliente.Id);

                CargarNotificaciones();

                if (cantidad == 0)
                {
                    MessageBox.Show(
                        "No tienes notificaciones pendientes.",
                        "Notificaciones",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "Notificaciones marcadas como leídas: " +
                        cantidad,
                        "Notificaciones",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(
                    error.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void buttonVolver_Click(
            object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}

