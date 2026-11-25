using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FitCR.Controllers;
using FitCR.Models.Usuarios;
using FitCR.Models.Rutinas;
using FitCR.Models.Visitas;
using FitCR.Models.Medidas;
using FitCR.Models.Objetivos;
using FitCR.Models.Clases;
using FitCR.Models.Estadisticas;
using FitCR.Repositories;

namespace FitCR.Views.Entrenadores
{
    public partial class FormEstadisticas : Form
    {
        private readonly ControladorEstadisticas controlador;
        private DateTimePicker fechaInicio = new DateTimePicker();
        private DateTimePicker fechaFin = new DateTimePicker();
        private Label labelResumen = new Label();
        private TabControl pestanas = new TabControl();
        private DataGridView tablaMayor = new DataGridView();
        private DataGridView tablaMenor = new DataGridView();
        private DataGridView tablaPeso = new DataGridView();
        private DataGridView tablaObjetivos = new DataGridView();
        private DataGridView tablaRutinas = new DataGridView();
        private DataGridView tablaClases = new DataGridView();
        private DataGridView tablaClientes = new DataGridView();
        public FormEstadisticas()
        {
            InitializeComponent();

            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data"
            );

            controlador = new ControladorEstadisticas(
                new RepositorioJson<Cliente>(
                    Path.Combine(ruta, "clientes.json")
                ),
                new RepositorioJson<Rutina>(
                    Path.Combine(ruta, "rutinas.json")
                ),
                new RepositorioJson<RegistroVisita>(
                    Path.Combine(ruta, "visitas.json")
                ),
                new RepositorioJson<RegistroMedida>(
                    Path.Combine(ruta, "medidas.json")
                ),
                new RepositorioJson<Objetivo>(
                    Path.Combine(ruta, "objetivos.json")
                ),
                new RepositorioJson<RegistroInvitado>(
                    Path.Combine(ruta, "invitados.json")
                ),
                new RepositorioJson<ClaseGimnasio>(
                    Path.Combine(ruta, "clases.json")
                ),
                new RepositorioJson<InscripcionClase>(
                    Path.Combine(ruta, "inscripciones_clases.json")
                ),
                new RepositorioJson<AsistenciaClase>(
                    Path.Combine(ruta, "asistencias_clases.json")
                )
            );

            ConfigurarFormulario();
            CargarEstadisticas();
        }

        private void ConfigurarFormulario()
        {
            Text = "Estadísticas generales";
            ClientSize = new Size(1250, 850);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
            AutoScroll = true;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Estadísticas generales";
            labelTitulo.Location = new Point(30, 5);
            labelTitulo.Size = new Size(1190, 65);
            labelTitulo.Font = new Font(
                "Segoe UI", 26, FontStyle.Bold
            );
            labelTitulo.TextAlign =
                ContentAlignment.MiddleCenter;

            Label labelInicio = new Label();
            labelInicio.Text = "Fecha inicial";
            labelInicio.Location = new Point(30, 85);
            labelInicio.Size = new Size(250, 30);
            labelInicio.Font = new Font("Segoe UI", 11);

            fechaInicio.Location = new Point(30, 120);
            fechaInicio.Size = new Size(250, 35);
            fechaInicio.Font = new Font("Segoe UI", 11);
            fechaInicio.Format = DateTimePickerFormat.Short;
            fechaInicio.Value = new DateTime(
                DateTime.Today.Year,
                DateTime.Today.Month,
                1
            );

            Label labelFin = new Label();
            labelFin.Text = "Fecha final";
            labelFin.Location = new Point(310, 85);
            labelFin.Size = new Size(250, 30);
            labelFin.Font = new Font("Segoe UI", 11);

            fechaFin.Location = new Point(310, 120);
            fechaFin.Size = new Size(250, 35);
            fechaFin.Font = new Font("Segoe UI", 11);
            fechaFin.Format = DateTimePickerFormat.Short;
            fechaFin.Value = DateTime.Today;

            Button buttonConsultar = new Button();
            buttonConsultar.Text = "Consultar";
            buttonConsultar.Location = new Point(600, 110);
            buttonConsultar.Size = new Size(280, 45);
            buttonConsultar.Font = new Font("Segoe UI", 11);
            buttonConsultar.Click += (sender, e) =>
                CargarEstadisticas();

            Button buttonMesActual = new Button();
            buttonMesActual.Text = "Este mes";
            buttonMesActual.Location = new Point(920, 110);
            buttonMesActual.Size = new Size(280, 45);
            buttonMesActual.Font = new Font("Segoe UI", 11);
            buttonMesActual.Click += buttonMesActual_Click;

            labelResumen.Location = new Point(30, 180);
            labelResumen.Size = new Size(1190, 160);
            labelResumen.Font = new Font(
                "Segoe UI", 11, FontStyle.Bold
            );
            labelResumen.BackColor = Color.White;
            labelResumen.BorderStyle =
                BorderStyle.FixedSingle;
            labelResumen.Padding = new Padding(15);

            pestanas.Location = new Point(30, 365);
            pestanas.Size = new Size(1190, 380);
            pestanas.Font = new Font("Segoe UI", 10);

            CrearPestana(
                "Mayor asistencia",
                tablaMayor
            );

            CrearPestana(
                "Menor asistencia",
                tablaMenor
            );

            CrearPestana(
                "Peso",
                tablaPeso
            );

            CrearPestana(
                "Objetivos",
                tablaObjetivos
            );

            CrearPestana(
                "Rutinas",
                tablaRutinas
            );

            CrearPestana(
                "Clases",
                tablaClases
            );

            CrearPestana(
                "Todos los clientes",
                tablaClientes
            );

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver";
            buttonVolver.Location = new Point(920, 775);
            buttonVolver.Size = new Size(280, 50);
            buttonVolver.Font = new Font("Segoe UI", 11);
            buttonVolver.Click += (sender, e) => Close();

            Controls.Add(labelTitulo);
            Controls.Add(labelInicio);
            Controls.Add(labelFin);
            Controls.Add(fechaInicio);
            Controls.Add(fechaFin);
            Controls.Add(buttonConsultar);
            Controls.Add(buttonMesActual);
            Controls.Add(labelResumen);
            Controls.Add(pestanas);
            Controls.Add(buttonVolver);
        }

        private void CrearPestana(
            string titulo,
            DataGridView tabla)
        {
            TabPage pestana = new TabPage(titulo);

            tabla.Dock = DockStyle.Fill;
            tabla.ReadOnly = true;
            tabla.AllowUserToAddRows = false;
            tabla.AllowUserToDeleteRows = false;
            tabla.RowHeadersVisible = false;
            tabla.MultiSelect = false;

            tabla.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            tabla.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            tabla.BackgroundColor = Color.White;

            pestana.Controls.Add(tabla);
            pestanas.TabPages.Add(pestana);
        }

        private void CargarEstadisticas()
        {
            try
            {
                DateTime inicio = fechaInicio.Value.Date;
                DateTime fin = fechaFin.Value.Date;

                if (inicio > fin)
                {
                    MessageBox.Show(
                        "La fecha inicial no puede ser " +
                        "posterior a la fecha final.",
                        "Fechas incorrectas",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                CargarResumen(inicio, fin);
                CargarAsistencia(inicio, fin);
                CargarPeso(inicio, fin);
                CargarObjetivos(inicio, fin);
                CargarRutinas(inicio, fin);
                CargarClases(inicio, fin);
                CargarClientes(inicio, fin);
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

        private void CargarResumen(
            DateTime inicio,
            DateTime fin)
        {
            ResumenEstadisticas resumen =
                controlador.ObtenerResumen(inicio, fin);

            labelResumen.Text =
                "Período: " +
                inicio.ToString("dd/MM/yyyy") +
                " - " +
                fin.ToString("dd/MM/yyyy") +
                Environment.NewLine +

                "Clientes registrados: " +
                resumen.TotalClientes +
                "     |     Clientes activos: " +
                resumen.ClientesActivos +
                "     |     Visitas: " +
                resumen.TotalVisitas +
                "     |     Promedio de visitas: " +
                resumen.PromedioVisitas +
                Environment.NewLine +

                "Rutinas registradas: " +
                resumen.TotalRutinas +
                "     |     Rutinas completadas: " +
                resumen.RutinasCompletadas +
                "     |     Medidas: " +
                resumen.TotalMedidas +
                Environment.NewLine +

                "Objetivos registrados: " +
                resumen.TotalObjetivos +
                "     |     Objetivos cumplidos: " +
                resumen.ObjetivosCompletados +
                "     |     Invitados: " +
                resumen.TotalInvitados +
                Environment.NewLine +

                "Clases programadas: " +
                resumen.TotalClases +
                "     |     Finalizadas: " +
                resumen.ClasesFinalizadas +
                "     |     Canceladas: " +
                resumen.ClasesCanceladas +
                Environment.NewLine +

                "Inscripciones a clases: " +
                resumen.TotalInscripciones +
                "     |     Asistencias a clases: " +
                resumen.TotalAsistencias;
        }

        private void CargarAsistencia(
            DateTime inicio,
            DateTime fin)
        {
            var mayores = controlador
                .ObtenerClientesMayorAsistencia(
                    inicio, fin
                );

            tablaMayor.DataSource = mayores
                .Select((c, posicion) => new
                {
                    Posicion = posicion + 1,
                    c.IdCliente,
                    c.Nombre,
                    c.NombreUsuario,
                    Visitas = c.TotalVisitas
                })
                .ToList();

            var menores = controlador
                .ObtenerClientesMenorAsistencia(
                    inicio, fin
                );

            tablaMenor.DataSource = menores
                .Select((c, posicion) => new
                {
                    Posicion = posicion + 1,
                    c.IdCliente,
                    c.Nombre,
                    c.NombreUsuario,
                    Visitas = c.TotalVisitas
                })
                .ToList();

            CambiarTituloColumna(
                tablaMayor, "Posicion", "Posición"
            );

            CambiarTituloColumna(
                tablaMenor, "Posicion", "Posición"
            );

            FormatearColumnasCliente(tablaMayor);
            FormatearColumnasCliente(tablaMenor);

            tablaMayor.ClearSelection();
            tablaMenor.ClearSelection();
        }

        private void CargarPeso(
            DateTime inicio,
            DateTime fin)
        {
            tablaPeso.DataSource = controlador
                .ObtenerEstadisticasPeso(inicio, fin)
                .Select(p => new
                {
                    p.IdCliente,
                    p.Nombre,
                    p.PesoInicial,
                    p.PesoFinal,

                    FechaInicial = p.FechaInicial
                        .ToString("dd/MM/yyyy"),

                    FechaFinal = p.FechaFinal
                        .ToString("dd/MM/yyyy"),

                    p.CambioPeso,
                    p.Tendencia,
                    p.TotalRegistros
                })
                .ToList();

            FormatearColumnasCliente(tablaPeso);

            CambiarTituloColumna(
                tablaPeso, "PesoInicial", "Peso inicial (kg)"
            );

            CambiarTituloColumna(
                tablaPeso, "PesoFinal", "Peso final (kg)"
            );

            CambiarTituloColumna(
                tablaPeso, "CambioPeso", "Cambio (kg)"
            );

            CambiarTituloColumna(
                tablaPeso, "FechaInicial", "Fecha inicial"
            );

            CambiarTituloColumna(
                tablaPeso, "FechaFinal", "Fecha final"
            );

            CambiarTituloColumna(
                tablaPeso, "TotalRegistros", "Registros"
            );

            tablaPeso.ClearSelection();
        }

        private void CargarObjetivos(
            DateTime inicio,
            DateTime fin)
        {
            tablaObjetivos.DataSource = controlador
                .ObtenerClientesMejoresObjetivos(
                    inicio, fin
                )
                .Select(c => new
                {
                    c.IdCliente,
                    c.Nombre,
                    c.TotalObjetivos,
                    c.ObjetivosCompletados,

                    Pendientes = c.TotalObjetivos -
                        c.ObjetivosCompletados,

                    Cumplimiento = c.PorcentajeObjetivos
                })
                .ToList();

            FormatearColumnasCliente(tablaObjetivos);

            CambiarTituloColumna(
                tablaObjetivos,
                "TotalObjetivos",
                "Objetivos registrados"
            );

            CambiarTituloColumna(
                tablaObjetivos,
                "ObjetivosCompletados",
                "Objetivos cumplidos"
            );

            CambiarTituloColumna(
                tablaObjetivos,
                "Cumplimiento",
                "Cumplimiento (%)"
            );

            tablaObjetivos.ClearSelection();
        }

        private void CargarRutinas(
            DateTime inicio,
            DateTime fin)
        {
            tablaRutinas.DataSource = controlador
                .ObtenerClientesMejoresRutinas(
                    inicio, fin
                )
                .Select(c => new
                {
                    c.IdCliente,
                    c.Nombre,
                    c.TotalRutinas,
                    c.RutinasCompletadas,

                    SinCompletarEnPeriodo = c.TotalRutinas -
                        c.RutinasCompletadas,

                    Cumplimiento = c.PorcentajeRutinas
                })
                .ToList();

            FormatearColumnasCliente(tablaRutinas);

            CambiarTituloColumna(
                tablaRutinas,
                "TotalRutinas",
                "Rutinas registradas"
            );

            CambiarTituloColumna(
                tablaRutinas,
                "RutinasCompletadas",
                "Completadas en el período"
            );

            CambiarTituloColumna(
                tablaRutinas,
                "SinCompletarEnPeriodo",
                "No completadas en el período"
            );

            CambiarTituloColumna(
                tablaRutinas,
                "Cumplimiento",
                "Cumplimiento (%)"
            );

            tablaRutinas.ClearSelection();
        }

        private void CargarClases(
            DateTime inicio,
            DateTime fin)
        {
            tablaClases.DataSource = controlador
                .ObtenerEstadisticasClases(inicio, fin)
                .Select(c => new
                {
                    c.NombreClase,
                    c.TotalClases,
                    c.ClasesCanceladas,
                    c.ClasesFinalizadas,
                    c.TotalInscripciones,
                    c.TotalAsistencias,
                    c.PorcentajeAsistencia
                })
                .ToList();

            CambiarTituloColumna(
                tablaClases, "NombreClase", "Actividad"
            );

            CambiarTituloColumna(
                tablaClases, "TotalClases", "Clases"
            );

            CambiarTituloColumna(
                tablaClases, "ClasesCanceladas", "Canceladas"
            );

            CambiarTituloColumna(
                tablaClases, "ClasesFinalizadas", "Finalizadas"
            );

            CambiarTituloColumna(
                tablaClases, "TotalInscripciones", "Inscripciones"
            );

            CambiarTituloColumna(
                tablaClases, "TotalAsistencias", "Asistencias"
            );

            CambiarTituloColumna(
                tablaClases,
                "PorcentajeAsistencia",
                "Asistencia finalizadas (%)"
            );

            tablaClases.ClearSelection();
        }

        private void CargarClientes(
            DateTime inicio,
            DateTime fin)
        {
            tablaClientes.DataSource = controlador
                .ObtenerEstadisticasClientes(inicio, fin)
                .Select(c => new
                {
                    c.IdCliente,
                    c.Nombre,
                    Visitas = c.TotalVisitas,
                    Rutinas = c.TotalRutinas,
                    Completadas = c.RutinasCompletadas,
                    Objetivos = c.TotalObjetivos,
                    Cumplidos = c.ObjetivosCompletados,
                    Medidas = c.TotalMedidas,
                    Invitados = c.TotalInvitados,
                    c.AsistenciasClases
                })
                .ToList();

            FormatearColumnasCliente(tablaClientes);

            CambiarTituloColumna(
                tablaClientes,
                "AsistenciasClases",
                "Asistencias a clases"
            );

            tablaClientes.ClearSelection();
        }

        private static void FormatearColumnasCliente(
            DataGridView tabla)
        {
            CambiarTituloColumna(
                tabla, "IdCliente", "ID cliente"
            );

            CambiarTituloColumna(
                tabla, "NombreUsuario", "Nombre de usuario"
            );
        }

        private static void CambiarTituloColumna(
            DataGridView tabla,
            string nombre,
            string titulo)
        {
            DataGridViewColumn? columna =
                tabla.Columns[nombre];

            if (columna != null)
            {
                columna.HeaderText = titulo;
            }
        }

        private void buttonMesActual_Click(
            object? sender,
            EventArgs e)
        {
            fechaInicio.Value = new DateTime(
                DateTime.Today.Year,
                DateTime.Today.Month,
                1
            );

            fechaFin.Value = DateTime.Today;

            CargarEstadisticas();
        }
    }
}
