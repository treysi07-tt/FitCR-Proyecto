using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FitCR.Controllers;
using FitCR.Models.Usuarios;
using FitCR.Models.Progreso;
using FitCR.Models.Rutinas;
using FitCR.Models.Medidas;
using FitCR.Models.Visitas;
using FitCR.Models.Objetivos;
using FitCR.Repositories;

namespace FitCR.Views.Clientes
{
    public partial class FormProgreso : Form
    {
        private readonly Cliente cliente;
        private readonly ControladorProgreso controlador;
        private Label labelRutinas;
        private Label labelVisitas;
        private Label labelObjetivos;
        private Label labelMedidas;
        private Label labelPesoInicial;
        private Label labelPesoActual;
        private Label labelCambioPeso;
        private Label labelPorcentajeRutinas;
        private Label labelPorcentajeObjetivos;
        private ProgressBar barraRutinas;
        private ProgressBar barraObjetivos;
        private DataGridView tablaPeso;

        public FormProgreso(Cliente clienteRecibido)
        {
            InitializeComponent();
            cliente = clienteRecibido;

            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data"
            );

            RepositorioJson<Rutina> repositorioRutinas =
                new RepositorioJson<Rutina>(
                    Path.Combine(ruta, "rutinas.json")
                );

            RepositorioJson<RegistroMedida> repositorioMedidas =
                new RepositorioJson<RegistroMedida>(
                    Path.Combine(ruta, "medidas.json")
                );

            RepositorioJson<RegistroVisita> repositorioVisitas =
                new RepositorioJson<RegistroVisita>(
                    Path.Combine(ruta, "visitas.json")
                );

            RepositorioJson<Objetivo> repositorioObjetivos =
                new RepositorioJson<Objetivo>(
                    Path.Combine(ruta, "objetivos.json")
                );

            controlador = new ControladorProgreso(
                repositorioRutinas,
                repositorioMedidas,
                repositorioVisitas,
                repositorioObjetivos
            );

            Text = "Mi progreso";
            ClientSize = new Size(1050, 760);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            AutoScroll = true;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Mi progreso personal";
            labelTitulo.Location = new Point(340, 20);
            labelTitulo.Size = new Size(450, 45);
            labelTitulo.Font = new Font(
                "Segoe UI", 20, FontStyle.Bold
            );

            Label labelCliente = new Label();
            labelCliente.Text = "Cliente: " + cliente.Nombre;
            labelCliente.Location = new Point(50, 80);
            labelCliente.Size = new Size(900, 30);
            labelCliente.Font = new Font("Segoe UI", 12);

            Label labelEstadisticas = new Label();
            labelEstadisticas.Text = "Estadísticas generales";
            labelEstadisticas.Location = new Point(50, 120);
            labelEstadisticas.Size = new Size(500, 35);
            labelEstadisticas.Font = new Font(
                "Segoe UI", 14, FontStyle.Bold
            );

            labelRutinas = new Label();
            labelRutinas.Location = new Point(50, 165);
            labelRutinas.Size = new Size(450, 55);

            labelVisitas = new Label();
            labelVisitas.Location = new Point(550, 165);
            labelVisitas.Size = new Size(450, 55);

            labelObjetivos = new Label();
            labelObjetivos.Location = new Point(50, 225);
            labelObjetivos.Size = new Size(450, 55);

            labelMedidas = new Label();
            labelMedidas.Location = new Point(550, 225);
            labelMedidas.Size = new Size(450, 55);

            labelPorcentajeRutinas = new Label();
            labelPorcentajeRutinas.Location =
                new Point(50, 290);
            labelPorcentajeRutinas.Size =
                new Size(450, 30);
            labelPorcentajeRutinas.Font = new Font(
                "Segoe UI", 11, FontStyle.Bold
            );

            barraRutinas = new ProgressBar();
            barraRutinas.Location = new Point(50, 325);
            barraRutinas.Size = new Size(450, 30);
            barraRutinas.Minimum = 0;
            barraRutinas.Maximum = 100;

            labelPorcentajeObjetivos = new Label();
            labelPorcentajeObjetivos.Location =
                new Point(550, 290);
            labelPorcentajeObjetivos.Size =
                new Size(450, 30);
            labelPorcentajeObjetivos.Font = new Font(
                "Segoe UI", 11, FontStyle.Bold
            );

            barraObjetivos = new ProgressBar();
            barraObjetivos.Location = new Point(550, 325);
            barraObjetivos.Size = new Size(450, 30);
            barraObjetivos.Minimum = 0;
            barraObjetivos.Maximum = 100;

            Label labelEvolucion = new Label();
            labelEvolucion.Text = "Evolución del peso";
            labelEvolucion.Location = new Point(50, 385);
            labelEvolucion.Size = new Size(450, 35);
            labelEvolucion.Font = new Font(
                "Segoe UI", 14, FontStyle.Bold
            );

            labelPesoInicial = new Label();
            labelPesoInicial.Location = new Point(50, 435);
            labelPesoInicial.Size = new Size(300, 35);

            labelPesoActual = new Label();
            labelPesoActual.Location = new Point(380, 435);
            labelPesoActual.Size = new Size(300, 35);

            labelCambioPeso = new Label();
            labelCambioPeso.Location = new Point(710, 435);
            labelCambioPeso.Size = new Size(290, 35);

            tablaPeso = new DataGridView();
            tablaPeso.Location = new Point(50, 485);
            tablaPeso.Size = new Size(950, 175);
            tablaPeso.ReadOnly = true;
            tablaPeso.AllowUserToAddRows = false;
            tablaPeso.AllowUserToDeleteRows = false;
            tablaPeso.RowHeadersVisible = false;
            tablaPeso.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            tablaPeso.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            Button buttonActualizar = new Button();
            buttonActualizar.Text = "Actualizar progreso";
            buttonActualizar.Location = new Point(225, 690);
            buttonActualizar.Size = new Size(260, 45);
            buttonActualizar.Click +=
                (sender, e) => CargarProgreso();

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver al menú";
            buttonVolver.Location = new Point(565, 690);
            buttonVolver.Size = new Size(260, 45);
            buttonVolver.Click +=
                (sender, e) => Close();

            Controls.Add(labelTitulo);
            Controls.Add(labelCliente);
            Controls.Add(labelEstadisticas);
            Controls.Add(labelRutinas);
            Controls.Add(labelVisitas);
            Controls.Add(labelObjetivos);
            Controls.Add(labelMedidas);
            Controls.Add(labelPorcentajeRutinas);
            Controls.Add(barraRutinas);
            Controls.Add(labelPorcentajeObjetivos);
            Controls.Add(barraObjetivos);
            Controls.Add(labelEvolucion);
            Controls.Add(labelPesoInicial);
            Controls.Add(labelPesoActual);
            Controls.Add(labelCambioPeso);
            Controls.Add(tablaPeso);
            Controls.Add(buttonActualizar);
            Controls.Add(buttonVolver);
            CargarProgreso();
        }

        private void CargarProgreso()
        {
            try
            {
                ResumenProgreso resumen =
                    controlador.ObtenerProgreso(cliente.Id);
                labelRutinas.Text =
                    "Rutinas registradas: " +
                    resumen.TotalRutinas +
                    "\nRutinas completadas: " +
                    resumen.RutinasCompletadas +
                    "\nRutinas pendientes: " +
                    resumen.RutinasPendientes;

                labelVisitas.Text =
                    "Visitas totales: " +
                    resumen.TotalVisitas +
                    "\nVisitas del mes actual: " +
                    resumen.VisitasMesActual;

                labelObjetivos.Text =
                    "Objetivos registrados: " +
                    resumen.TotalObjetivos +
                    "\nObjetivos completados: " +
                    resumen.ObjetivosCompletados +
                    "\nObjetivos pendientes: " +
                    resumen.ObjetivosPendientes;

                labelMedidas.Text =
                    "Medidas registradas: " +
                    resumen.TotalMedidas;

                double porcentajeRutinas =
                    resumen.CalcularPorcentajeRutinas();

                labelPorcentajeRutinas.Text =
                    "Cumplimiento de rutinas: " +
                    porcentajeRutinas.ToString("F2") + "%";

                barraRutinas.Value = Math.Clamp(
                    (int)Math.Round(porcentajeRutinas),
                    0,
                    100
                );

                double porcentajeObjetivos =
                    resumen.CalcularPorcentajeObjetivos();

                labelPorcentajeObjetivos.Text =
                    "Cumplimiento de objetivos: " +
                    porcentajeObjetivos.ToString("F2") + "%";

                barraObjetivos.Value = Math.Clamp(
                    (int)Math.Round(porcentajeObjetivos),
                    0,
                    100
                );

                labelPesoInicial.Text =
                    resumen.PesoInicial.HasValue
                    ? "Peso inicial: " +
                      resumen.PesoInicial.Value.ToString("F2") +
                      " kg"
                    : "Peso inicial: No hay registros";

                labelPesoActual.Text =
                    resumen.PesoActual.HasValue
                    ? "Peso actual: " +
                      resumen.PesoActual.Value.ToString("F2") +
                      " kg"
                    : "Peso actual: No hay registros";

                double? cambioPeso =
                    resumen.CalcularCambioPeso();

                if (cambioPeso.HasValue)
                {
                    labelCambioPeso.Text =
                        "Variación: " +
                        cambioPeso.Value.ToString(
                            "+0.00;-0.00;0.00"
                        ) + " kg";
                }
                else
                {
                    labelCambioPeso.Text =
                        "Variación: No hay registros";
                }

                var historial =
                    controlador.ObtenerHistorialPeso(
                        cliente.Id
                    );

                tablaPeso.DataSource = historial
                    .ConvertAll(m => new
                    {
                        m.Fecha,
                        m.Peso
                    });

                DataGridViewColumn? columnaFecha =
                    tablaPeso.Columns["Fecha"];

                if (columnaFecha != null)
                {
                    columnaFecha.DefaultCellStyle.Format =
                        "dd/MM/yyyy";
                }

                DataGridViewColumn? columnaPeso =
                    tablaPeso.Columns["Peso"];

                if (columnaPeso != null)
                {
                    columnaPeso.HeaderText = "Peso (kg)";
                    columnaPeso.DefaultCellStyle.Format = "F2";
                }

                tablaPeso.ClearSelection();
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
                    "No se pudo cargar el progreso del cliente."
                );
            }
        }
    }
}

