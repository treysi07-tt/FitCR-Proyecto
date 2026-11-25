using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FitCR.Controllers;
using FitCR.Models.Usuarios;
using FitCR.Models.Insignias;
using FitCR.Models.Rutinas;
using FitCR.Models.Visitas;
using FitCR.Models.Objetivos;
using FitCR.Models.Medidas;
using FitCR.Repositories;

namespace FitCR.Views.Clientes
{
    public partial class FormInsignias : Form
    {
        private readonly Cliente cliente;
        private readonly ControladorInsignias controlador;
        private DataGridView tablaObtenidas = new DataGridView();
        private DataGridView tablaPendientes = new DataGridView();
        private DataGridView tablaObservaciones = new DataGridView();
        private Label labelResumen = new Label();
        private TextBox textDetalle = new TextBox();
        public FormInsignias(Cliente clienteRecibido)
        {
            InitializeComponent();
            cliente = clienteRecibido;

            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data"
            );

            controlador = new ControladorInsignias(
                new RepositorioJson<InsigniaObtenida>(
                    Path.Combine(ruta, "insignias_obtenidas.json")),

                new RepositorioJson<Rutina>(
                    Path.Combine(ruta, "rutinas.json")),

                new RepositorioJson<RegistroVisita>(
                    Path.Combine(ruta, "visitas.json")),

                new RepositorioJson<Objetivo>(
                    Path.Combine(ruta, "objetivos.json")),

                new RepositorioJson<RegistroMedida>(
                    Path.Combine(ruta, "medidas.json")),

                new RepositorioJson<RegistroInvitado>(
                    Path.Combine(ruta, "invitados.json"))
            );
            ConfigurarFormulario();
            CargarInsignias();
        }

        private void ConfigurarFormulario()
        {
            Controls.Clear();

            Text = "Mis insignias";
            Size = new Size(1200, 800);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
            AutoScroll = true;

            Label titulo = new Label();
            titulo.Text = "Mis insignias y reconocimientos";
            titulo.Font = new Font(
                "Segoe UI", 20, FontStyle.Bold);
            titulo.Location = new Point(30, 20);
            titulo.Size = new Size(1120, 55);
            titulo.TextAlign = ContentAlignment.MiddleCenter;

            labelResumen.Location = new Point(30, 85);
            labelResumen.Size = new Size(1120, 50);
            labelResumen.Font = new Font("Segoe UI", 11);
            labelResumen.TextAlign =
                ContentAlignment.MiddleCenter;

            TabControl pestañas = new TabControl();
            pestañas.Location = new Point(30, 150);
            pestañas.Size = new Size(1120, 370);
            pestañas.Font = new Font("Segoe UI", 11);

            TabPage paginaObtenidas =
                new TabPage("Insignias obtenidas");

            TabPage paginaPendientes =
                new TabPage("Por desbloquear");

            TabPage paginaObservaciones =
                new TabPage("Observaciones personales");

            ConfigurarTabla(tablaObtenidas);
            ConfigurarTabla(tablaPendientes);
            ConfigurarTabla(tablaObservaciones);

            paginaObtenidas.Controls.Add(tablaObtenidas);
            paginaPendientes.Controls.Add(tablaPendientes);
            paginaObservaciones.Controls.Add(tablaObservaciones);

            pestañas.TabPages.Add(paginaObtenidas);
            pestañas.TabPages.Add(paginaPendientes);
            pestañas.TabPages.Add(paginaObservaciones);

            pestañas.SelectedIndexChanged += (sender, e) =>
            {
                textDetalle.Clear();

                foreach (TabPage pagina in pestañas.TabPages)
                {
                    foreach (Control control in pagina.Controls)
                    {
                        if (control is DataGridView tabla)
                        {
                            tabla.ClearSelection();
                        }
                    }
                }
            };

            Label labelDetalle = new Label();
            labelDetalle.Text = "Detalle del reconocimiento";
            labelDetalle.Location = new Point(30, 535);
            labelDetalle.Size = new Size(1120, 30);
            labelDetalle.Font = new Font(
                "Segoe UI", 12, FontStyle.Bold);

            textDetalle.Location = new Point(30, 570);
            textDetalle.Size = new Size(1120, 100);
            textDetalle.Multiline = true;
            textDetalle.ReadOnly = true;
            textDetalle.ScrollBars = ScrollBars.Vertical;
            textDetalle.Font = new Font("Segoe UI", 11);
            textDetalle.BackColor = Color.White;

            Button buttonActualizar = new Button();
            buttonActualizar.Text = "Actualizar insignias";
            buttonActualizar.Location = new Point(300, 690);
            buttonActualizar.Size = new Size(250, 45);
            buttonActualizar.Font = new Font("Segoe UI", 11);
            buttonActualizar.Click += buttonActualizar_Click;

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver";
            buttonVolver.Location = new Point(650, 690);
            buttonVolver.Size = new Size(250, 45);
            buttonVolver.Font = new Font("Segoe UI", 11);
            buttonVolver.Click += (sender, e) => Close();

            Controls.Add(titulo);
            Controls.Add(labelResumen);
            Controls.Add(pestañas);
            Controls.Add(labelDetalle);
            Controls.Add(textDetalle);
            Controls.Add(buttonActualizar);
            Controls.Add(buttonVolver);
        }

        private void ConfigurarTabla(DataGridView tabla)
        {
            tabla.Dock = DockStyle.Fill;
            tabla.ReadOnly = true;
            tabla.AllowUserToAddRows = false;
            tabla.AllowUserToDeleteRows = false;
            tabla.AllowUserToResizeRows = false;
            tabla.MultiSelect = false;

            tabla.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            tabla.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            tabla.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.AllCells;

            tabla.DefaultCellStyle.WrapMode =
                DataGridViewTriState.True;

            tabla.BackgroundColor = Color.White;
            tabla.RowHeadersVisible = false;

            tabla.DefaultCellStyle.Font =
                new Font("Segoe UI", 10);

            tabla.SelectionChanged += tabla_SelectionChanged;
        }
        private void CargarInsignias()
        {
            try
            {
                var nuevasInsignias =
                    controlador.EvaluarInsignias(cliente.Id);

                var catalogo =
                    controlador.ObtenerCatalogo();

                var registros =
                    controlador.ObtenerInsigniasCliente(cliente.Id);

                var reconocimientos = registros
                    .Join(
                        catalogo,
                        registro => registro.IdInsignia,
                        insignia => insignia.Id,
                        (registro, insignia) => new
                        {
                            Registro = registro,
                            Insignia = insignia
                        })
                    .ToList();

                var obtenidas = reconocimientos
                    .Where(r => !r.Insignia.EsNegativa)
                    .Select(r => new
                    {
                        Id = r.Insignia.Id,
                        r.Insignia.Nombre,
                        r.Insignia.Categoria,
                        r.Insignia.Descripcion,

                        FechaObtencion =
                            r.Registro.FechaObtencion
                                .ToString("dd/MM/yyyy"),

                        Periodo = r.Registro.PeriodoInicio.HasValue
                            ? r.Registro.PeriodoInicio.Value
                                .ToString("MM/yyyy")
                            : "Permanente",

                        r.Insignia.Mensaje
                    })
                    .ToList();

                var pendientes = catalogo
                    .Where(i => !i.EsNegativa &&
                        (i.EsPeriodica
                            ? !registros.Any(r =>
                                r.IdInsignia == i.Id &&
                                r.PeriodoInicio.HasValue &&
                                r.PeriodoInicio.Value.Month ==
                                    DateTime.Today.Month &&
                                r.PeriodoInicio.Value.Year ==
                                    DateTime.Today.Year)
                            : !registros.Any(r =>
                                r.IdInsignia == i.Id)))
                    .Select(i => new
                    {
                        i.Id,
                        i.Nombre,
                        i.Categoria,
                        i.Descripcion,

                        Estado = EstaDisponible(i.Id)
                            ? "Por desbloquear"
                            : "Próximamente",

                        Tipo = i.EsPeriodica
                            ? "Periódica"
                            : "Permanente"
                    })
                    .ToList();

                var observaciones = reconocimientos
                    .Where(r => r.Insignia.EsNegativa)
                    .Select(r => new
                    {
                        Id = r.Insignia.Id,
                        r.Insignia.Nombre,
                        r.Insignia.Descripcion,

                        FechaObtencion =
                            r.Registro.FechaObtencion
                                .ToString("dd/MM/yyyy"),

                        Periodo = r.Registro.PeriodoInicio.HasValue
                            ? r.Registro.PeriodoInicio.Value
                                .ToString("MM/yyyy")
                            : "Sin período",

                        r.Insignia.Mensaje
                    })
                    .ToList();

                tablaObtenidas.DataSource = obtenidas;
                tablaPendientes.DataSource = pendientes;
                tablaObservaciones.DataSource = observaciones;

                FormatearColumnas(tablaObtenidas);
                FormatearColumnas(tablaPendientes);
                FormatearColumnas(tablaObservaciones);

                labelResumen.Text =
                    "Insignias obtenidas: " + obtenidas.Count +
                    "     |     Por desbloquear: " +
                    pendientes.Count(p => p.Estado == "Por desbloquear") +
                    "     |     Próximamente: " +
                    pendientes.Count(p => p.Estado == "Próximamente") +
                    "     |     Observaciones: " +
                    observaciones.Count;

                tablaObtenidas.ClearSelection();
                tablaPendientes.ClearSelection();
                tablaObservaciones.ClearSelection();

                textDetalle.Clear();

                if (nuevasInsignias.Count > 0)
                {
                    string mensaje = string.Join(
                        Environment.NewLine + Environment.NewLine,
                        nuevasInsignias.Select(i =>
                            i.Nombre + Environment.NewLine +
                            i.Mensaje));

                    MessageBox.Show(
                        mensaje,
                        "Nuevos reconocimientos",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(
                    error.Message,
                    "Error al cargar insignias",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private bool EstaDisponible(int idInsignia)
        {
            return idInsignia != 8 &&
                   idInsignia != 16 &&
                   idInsignia != 17 &&
                   idInsignia != 25 &&
                   idInsignia != 33 &&
                   idInsignia != 34 &&
                   idInsignia != 37 &&
                   idInsignia != 38;
        }

        private void FormatearColumnas(DataGridView tabla)
        {
            if (tabla.Columns["Id"] is DataGridViewColumn columnaId)
            {
                columnaId.Visible = false;
            }

            if (tabla.Columns["Mensaje"] is DataGridViewColumn columnaMensaje)
            {
                columnaMensaje.Visible = false;
            }

            if (tabla.Columns["FechaObtencion"] is DataGridViewColumn columnaFecha)
            {
                columnaFecha.HeaderText = "Fecha de obtención";
            }

            if (tabla.Columns["Descripcion"] is DataGridViewColumn columnaDescripcion)
            {
                columnaDescripcion.HeaderText = "Descripción";
                columnaDescripcion.FillWeight = 180;
            }

            if (tabla.Columns["Categoria"] is DataGridViewColumn columnaCategoria)
            {
                columnaCategoria.HeaderText = "Categoría";
            }

            if (tabla.Columns["Periodo"] is DataGridViewColumn columnaPeriodo)
            {
                columnaPeriodo.HeaderText = "Período";
            }
        }
        private void tabla_SelectionChanged(
            object? sender, EventArgs e)
        {
            if (sender is not DataGridView tabla)
            {
                return;
            }

            if (tabla.SelectedRows.Count == 0 ||
                !tabla.Columns.Contains("Id"))
            {
                return;
            }

            var fila = tabla.SelectedRows[0];

            if (fila.Cells["Id"].Value == null)
            {
                return;
            }

            int idInsignia =
                Convert.ToInt32(fila.Cells["Id"].Value);

            var insignia = controlador.ObtenerCatalogo()
                .FirstOrDefault(i => i.Id == idInsignia);

            if (insignia == null)
            {
                return;
            }

            string detalle =
                insignia.Nombre + Environment.NewLine +
                insignia.Descripcion;

            if (tabla == tablaPendientes)
            {
                detalle += Environment.NewLine +
                    (EstaDisponible(insignia.Id)
                        ? "Estado: Por desbloquear"
                        : "Estado: Próximamente");
            }
            else
            {
                detalle += Environment.NewLine +
                    insignia.Mensaje;
            }

            textDetalle.Text = detalle;
        }

        private void buttonActualizar_Click(
            object? sender, EventArgs e)
        {
            CargarInsignias();
        }
    }
}

