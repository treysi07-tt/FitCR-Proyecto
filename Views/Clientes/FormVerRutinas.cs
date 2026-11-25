using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FitCR.Controllers;
using FitCR.Models.Rutinas;
using FitCR.Models.Usuarios;
using FitCR.Repositories;

namespace FitCR.Views.Clientes
{
    public partial class FormVerRutinas : Form
    {
        private readonly Cliente cliente;
        private readonly ControladorRutinas controlador;
        private DataGridView tablaRutinas;
        private readonly bool mostrarUltimasCinco;

        private void buttonCompletar_Click(object? sender, EventArgs e)
        {
            if (tablaRutinas.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Primero debes seleccionar una rutina."
                );
                return;
            }

            DataGridViewRow fila = tablaRutinas.SelectedRows[0];

            if (fila.Cells["Id"].Value == null)
            {
                MessageBox.Show(
                    "No se pudo identificar la rutina. Intente de nuevo"
                );
                return;
            }

            int idRutina = Convert.ToInt32(
                fila.Cells["Id"].Value
            );

            string estado = fila.Cells["Estado"].Value?
                .ToString() ?? "";

            if (estado == "Completada")
            {
                MessageBox.Show(
                    "Esta rutina ya está completada."
                );
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Deseas marcar esta rutina como completada?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                controlador.CompletarRutina(
                    idRutina,
                    cliente.Id
                );

                MessageBox.Show(
                    "Rutina completada correctamente. Felicidades",
                    "Fit.CR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarRutinas();
            }
            catch (ArgumentException error)
            {
                MessageBox.Show(error.Message);
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "No se pudo actualizar la rutina. Intente de nuevo"
                );
            }
        }

        public FormVerRutinas(
            Cliente clienteRecibido,
            bool soloUltimasCinco = false)
        {
            InitializeComponent();

            cliente = clienteRecibido;
            mostrarUltimasCinco = soloUltimasCinco;


            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "rutinas.json"
            );

            RepositorioJson<Rutina> repositorio =
                new RepositorioJson<Rutina>(ruta);

            controlador = new ControladorRutinas(repositorio);

            Text = mostrarUltimasCinco
                ? "Últimas 5 rutinas"
                : "Ver mis rutinas";
            ClientSize = new Size(1200, 650);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label labelTitulo = new Label();
            labelTitulo.Text = mostrarUltimasCinco
                ? "Últimas 5 rutinas"
                : "Mis rutinas registradas";
            labelTitulo.Location = new Point(30, 20);
            labelTitulo.Size = new Size(500, 45);
            labelTitulo.Font = new Font(
                "Segoe UI", 17, FontStyle.Bold
            );

            Label labelCliente = new Label();
            labelCliente.Text = "Cliente: " + cliente.Nombre;
            labelCliente.Location = new Point(30, 75);
            labelCliente.Size = new Size(600, 30);

            tablaRutinas = new DataGridView();
            tablaRutinas.Location = new Point(30, 120);
            tablaRutinas.Size = new Size(1140, 400);

            tablaRutinas.RowHeadersVisible = false;
            tablaRutinas.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            tablaRutinas.ReadOnly = true;
            tablaRutinas.AllowUserToAddRows = false;
            tablaRutinas.AllowUserToDeleteRows = false;
            tablaRutinas.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            tablaRutinas.MultiSelect = false;
            tablaRutinas.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            Button buttonCompletar = new Button();
            buttonCompletar.Text = "Marcar como completada";
            buttonCompletar.Location = new Point(100, 555);
            buttonCompletar.Size = new Size(300, 50);
            buttonCompletar.Click += buttonCompletar_Click;

            Button buttonActualizar = new Button();
            buttonActualizar.Text = "Actualizar";
            buttonActualizar.Location = new Point(4500, 555);
            buttonActualizar.Size = new Size(250, 50);
            buttonActualizar.Click += (sender, e) =>
                CargarRutinas();

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver";
            buttonVolver.Location = new Point(750, 555);
            buttonVolver.Size = new Size(250, 50);
            buttonVolver.Click += (sender, e) => Close();

            Controls.Add(labelTitulo);
            Controls.Add(labelCliente);
            Controls.Add(tablaRutinas);
            Controls.Add(buttonActualizar);
            Controls.Add(buttonVolver);
            Controls.Add(buttonCompletar);
            CargarRutinas();
        }

        private void CargarRutinas()
        {
            try
            {
                var listaRutinas = mostrarUltimasCinco
                    ? controlador.ObtenerUltimasCinco(cliente.Id)
                    : controlador.ObtenerRutinas(cliente.Id);

                var rutinas = listaRutinas
                    .Select(r => new

                    {
                        r.Id,
                        r.Nombre,
                        r.Descripcion,
                        r.TipoEjercicio,
                        r.Duracion,
                        r.Series,
                        r.Repeticiones,
                        r.FechaCreacion,
                        r.FechaVencimiento,
                        Estado = r.Completada
                            ? "Completada"
                            : "Pendiente"
                    })
                    .ToList();

                tablaRutinas.DataSource = null;
                tablaRutinas.DataSource = rutinas;
                tablaRutinas.ClearSelection();

                CambiarTituloColumna(
                    "TipoEjercicio", "Tipo"
                );

                CambiarTituloColumna(
                    "Duracion", "Duración (min)"
                );

                CambiarTituloColumna(
                    "FechaCreacion", "Fecha de creación"
                );

                CambiarTituloColumna(
                    "FechaVencimiento", "Vencimiento"
                );

                FormatearFecha("FechaCreacion");
                FormatearFecha("FechaVencimiento");

                if (rutinas.Count == 0)
                {
                    MessageBox.Show(
                        "Todavía no tienes rutinas registradas. Registra una",
                        "Fit.CR",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "No se pudieron cargar las rutinas. Intenta de nuevo"
                );
            }
        }

        private void CambiarTituloColumna(
            string nombreColumna,
            string titulo)
        {
            DataGridViewColumn? columna =
                tablaRutinas.Columns[nombreColumna];

            if (columna != null)
            {
                columna.HeaderText = titulo;
            }
        }

        private void FormatearFecha(string nombreColumna)
        {
            DataGridViewColumn? columna =
                tablaRutinas.Columns[nombreColumna];

            if (columna != null)
            {
                columna.DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }
    }
}

