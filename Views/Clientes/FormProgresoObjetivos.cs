using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FitCR.Controllers;
using FitCR.Models.Usuarios;
using FitCR.Models.Objetivos;
using FitCR.Models.Progreso;
using FitCR.Models.Rutinas;
using FitCR.Models.Medidas;
using FitCR.Models.Visitas;
using FitCR.Repositories;

namespace FitCR.Views.Clientes
{
    public partial class FormProgresoObjetivos : Form
    {
        private readonly Cliente cliente;
        private readonly ControladorProgresoObjetivos controlador;
        private DataGridView tablaProgreso;
        private Label labelResumen;
        private Label labelDetalle;
        private ProgressBar barraProgreso;

        public FormProgresoObjetivos(Cliente clienteRecibido)
        {
            InitializeComponent();

            cliente = clienteRecibido;

            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data"
            );

            RepositorioJson<Objetivo> repositorioObjetivos =
                new RepositorioJson<Objetivo>(
                    Path.Combine(ruta, "objetivos.json")
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

            controlador = new ControladorProgresoObjetivos(
                repositorioObjetivos,
                repositorioRutinas,
                repositorioMedidas,
                repositorioVisitas
            );

            Text = "Progreso de mis objetivos";
            ClientSize = new Size(1200, 700);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            AutoScroll = true;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Progreso de mis objetivos";
            labelTitulo.Location = new Point(50, 20);
            labelTitulo.Size = new Size(1100, 55);
            labelTitulo.Font = new Font(
                "Segoe UI", 20, FontStyle.Bold
            );
            labelTitulo.TextAlign = ContentAlignment.MiddleCenter;
            labelTitulo.AutoSize = false;

            Label labelCliente = new Label();
            labelCliente.Text = "Cliente: " + cliente.Nombre;
            labelCliente.Location = new Point(50, 85);
            labelCliente.Size = new Size(1000, 30);

            labelResumen = new Label();
            labelResumen.Location = new Point(50, 125);
            labelResumen.Size = new Size(1100, 40);
            labelResumen.Font = new Font(
                "Segoe UI", 12, FontStyle.Bold
            );

            tablaProgreso = new DataGridView();
            tablaProgreso.Location = new Point(50, 180);
            tablaProgreso.Size = new Size(1100, 280);
            tablaProgreso.ReadOnly = true;
            tablaProgreso.AllowUserToAddRows = false;
            tablaProgreso.AllowUserToDeleteRows = false;
            tablaProgreso.RowHeadersVisible = false;
            tablaProgreso.MultiSelect = false;

            tablaProgreso.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            tablaProgreso.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            tablaProgreso.SelectionChanged +=
                tablaProgreso_SelectionChanged;

            labelDetalle = new Label();
            labelDetalle.Text =
                "Selecciona un objetivo para ver su avance.";

            labelDetalle.Location = new Point(50, 480);
            labelDetalle.Size = new Size(1100, 55);
            labelDetalle.Font = new Font(
                "Segoe UI", 12, FontStyle.Bold
            );

            barraProgreso = new ProgressBar();
            barraProgreso.Location = new Point(50, 550);
            barraProgreso.Size = new Size(1100, 35);
            barraProgreso.Minimum = 0;
            barraProgreso.Maximum = 100;

            Button buttonActualizar = new Button();
            buttonActualizar.Text = "Actualizar objetivos";
            buttonActualizar.Location = new Point(280, 620);
            buttonActualizar.Size = new Size(280, 45);
            buttonActualizar.Click +=
                (sender, e) => CargarProgreso();

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver a mis objetivos";
            buttonVolver.Location = new Point(640, 620);
            buttonVolver.Size = new Size(280, 45);
            buttonVolver.Click +=
                (sender, e) => Close();

            Controls.Add(labelTitulo);
            Controls.Add(labelCliente);
            Controls.Add(labelResumen);
            Controls.Add(tablaProgreso);
            Controls.Add(labelDetalle);
            Controls.Add(barraProgreso);
            Controls.Add(buttonActualizar);
            Controls.Add(buttonVolver);
            CargarProgreso();
        }

        private void CargarProgreso()
        {
            try
            {
                controlador.SincronizarObjetivos(cliente.Id);

                var resultados =
                    controlador.ObtenerProgresoObjetivos(
                        cliente.Id
                    );

                tablaProgreso.DataSource = resultados
                    .ConvertAll(p => new
                    {
                        p.IdObjetivo,
                        p.Nombre,
                        p.TipoObjetivo,
                        p.ValorInicial,
                        p.ValorActual,
                        p.ValorMeta,
                        p.CantidadRestante,
                        p.Unidad,
                        p.Porcentaje,
                        p.FechaLimite,
                        p.Estado
                    });


                // Encabezados y formatos de las columnas

                if (tablaProgreso.Columns["IdObjetivo"]
                    is DataGridViewColumn columnaId)
                {
                    columnaId.Visible = false;
                }

                if (tablaProgreso.Columns["TipoObjetivo"]
                    is DataGridViewColumn columnaTipo)
                {
                    columnaTipo.HeaderText = "Tipo";
                }

                if (tablaProgreso.Columns["ValorInicial"]
                    is DataGridViewColumn columnaInicial)
                {
                    columnaInicial.HeaderText = "Inicial";
                    columnaInicial.DefaultCellStyle.Format = "0.##";
                }

                if (tablaProgreso.Columns["ValorActual"]
                    is DataGridViewColumn columnaActual)
                {
                    columnaActual.HeaderText = "Actual";
                    columnaActual.DefaultCellStyle.Format = "0.##";
                }

                if (tablaProgreso.Columns["ValorMeta"]
                    is DataGridViewColumn columnaMeta)
                {
                    columnaMeta.HeaderText = "Meta";
                    columnaMeta.DefaultCellStyle.Format = "0.##";
                }

                if (tablaProgreso.Columns["CantidadRestante"]
                    is DataGridViewColumn columnaRestante)
                {
                    columnaRestante.HeaderText = "Restante";
                    columnaRestante.DefaultCellStyle.Format = "0.##";
                }

                if (tablaProgreso.Columns["Porcentaje"]
                    is DataGridViewColumn columnaPorcentaje)
                {
                    columnaPorcentaje.HeaderText = "Avance (%)";
                    columnaPorcentaje.DefaultCellStyle.Format = "F2";
                }

                if (tablaProgreso.Columns["FechaLimite"]
                    is DataGridViewColumn columnaFecha)
                {
                    columnaFecha.HeaderText = "Fecha límite";
                    columnaFecha.DefaultCellStyle.Format = "dd/MM/yyyy";
                }

                int alcanzados = resultados.FindAll(
                    p => p.MetaAlcanzada
                ).Count;

                int vencidos = resultados.FindAll(
                    p => p.Vencido
                ).Count;

                int pendientes =
                    resultados.Count - alcanzados - vencidos;

                labelResumen.Text =
                    "Total: " + resultados.Count +
                    "     |     Alcanzados: " + alcanzados +
                    "     |     Pendientes: " + pendientes +
                    "     |     Vencidos: " + vencidos;

                foreach (DataGridViewRow fila
                    in tablaProgreso.Rows)
                {
                    string estado =
                        fila.Cells["Estado"].Value?
                        .ToString() ?? "";

                    if (estado == "Completado")
                    {
                        fila.Cells["Estado"]
                            .Style.ForeColor = Color.DarkGreen;
                    }
                    else if (estado == "Vencido")
                    {
                        fila.Cells["Estado"]
                            .Style.ForeColor = Color.DarkRed;
                    }
                    else if (estado == "En progreso")
                    {
                        fila.Cells["Estado"]
                            .Style.ForeColor = Color.DarkOrange;
                    }
                }

                tablaProgreso.ClearSelection();

                barraProgreso.Value = 0;

                labelDetalle.Text =
                    "Selecciona un objetivo para ver su avance.";
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
                    "No se pudo cargar el progreso de los objetivos."
                );
            }
        }

        private void tablaProgreso_SelectionChanged(
            object? sender, EventArgs e)
        {
            if (barraProgreso == null ||
                labelDetalle == null ||
                tablaProgreso.SelectedRows.Count == 0 ||
                !tablaProgreso.Columns.Contains("Porcentaje") ||
                !tablaProgreso.Columns.Contains("Nombre"))
            {
                return;
            }

            DataGridViewRow fila =
                tablaProgreso.SelectedRows[0];

            if (fila.Cells["Porcentaje"].Value == null)
            {
                return;
            }

            double porcentaje = Convert.ToDouble(
                fila.Cells["Porcentaje"].Value
            );

            string nombre =
                fila.Cells["Nombre"].Value?
                .ToString() ?? "";

            barraProgreso.Value = Math.Clamp(
                (int)Math.Round(porcentaje),
                0,
                100
            );

            labelDetalle.Text =
                nombre +
                " - Avance: " +
                porcentaje.ToString("F2") + "%";
        }
    }
}

