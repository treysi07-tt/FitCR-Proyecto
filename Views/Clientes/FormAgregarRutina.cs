using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FitCR.Controllers;
using FitCR.Models.Rutinas;
using FitCR.Models.Usuarios;
using FitCR.Repositories;

namespace FitCR.Views.Clientes
{
    public partial class FormAgregarRutina : Form
    {
        private readonly Cliente cliente;
        private readonly ControladorRutinas controlador;
        private TextBox textNombre;
        private TextBox textDescripcion;
        private ComboBox comboTipo;
        private NumericUpDown numeroDuracion;
        private NumericUpDown numeroSeries;
        private NumericUpDown numeroRepeticiones;
        private DateTimePicker fechaVencimiento;
        public FormAgregarRutina(Cliente clienteRecibido)
        {
            InitializeComponent();
            cliente = clienteRecibido;

            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "rutinas.json"
            );

            RepositorioJson<Rutina> repositorio =
                new RepositorioJson<Rutina>(ruta);

            controlador = new ControladorRutinas(repositorio);

            Text = "Agregar rutina";
            ClientSize = new Size(550, 650);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Agregar nueva rutina";
            labelTitulo.Location = new Point(110, 20);
            labelTitulo.Size = new Size(380, 45);
            labelTitulo.Font = new Font(
                "Segoe UI", 16, FontStyle.Bold
            );

            Label labelNombre = new Label();
            labelNombre.Text = "Nombre de la rutina:";
            labelNombre.Location = new Point(50, 85);
            labelNombre.AutoSize = true;

            textNombre = new TextBox();
            textNombre.Location = new Point(50, 110);
            textNombre.Size = new Size(450, 30);

            Label labelDescripcion = new Label();
            labelDescripcion.Text = "Descripción:";
            labelDescripcion.Location = new Point(50, 155);
            labelDescripcion.AutoSize = true;

            textDescripcion = new TextBox();
            textDescripcion.Location = new Point(50, 180);
            textDescripcion.Size = new Size(450, 60);
            textDescripcion.Multiline = true;

            Label labelTipo = new Label();
            labelTipo.Text = "Tipo de ejercicio:";
            labelTipo.Location = new Point(50, 255);
            labelTipo.AutoSize = true;

            comboTipo = new ComboBox();
            comboTipo.Location = new Point(50, 280);
            comboTipo.Size = new Size(450, 30);
            comboTipo.DropDownStyle = ComboBoxStyle.DropDownList;

            comboTipo.Items.AddRange(new object[]
            {
                "Fuerza",
                "Cardio",
                "Resistencia",
                "Flexibilidad",
                "Funcional",
                "Otro"
            });

            Label labelDuracion = new Label();
            labelDuracion.Text = "Duración (minutos):";
            labelDuracion.Location = new Point(50, 330);
            labelDuracion.AutoSize = true;

            numeroDuracion = new NumericUpDown();
            numeroDuracion.Location = new Point(50, 355);
            numeroDuracion.Size = new Size(130, 30);
            numeroDuracion.Minimum = 1;
            numeroDuracion.Maximum = 300;
            numeroDuracion.Value = 30;

            Label labelSeries = new Label();
            labelSeries.Text = "Series:";
            labelSeries.Location = new Point(210, 330);
            labelSeries.AutoSize = true;

            numeroSeries = new NumericUpDown();
            numeroSeries.Location = new Point(210, 355);
            numeroSeries.Size = new Size(120, 30);
            numeroSeries.Minimum = 1;
            numeroSeries.Maximum = 20;
            numeroSeries.Value = 3;

            Label labelRepeticiones = new Label();
            labelRepeticiones.Text = "Repeticiones:";
            labelRepeticiones.Location = new Point(360, 330);
            labelRepeticiones.AutoSize = true;

            numeroRepeticiones = new NumericUpDown();
            numeroRepeticiones.Location = new Point(360, 355);
            numeroRepeticiones.Size = new Size(140, 30);
            numeroRepeticiones.Minimum = 1;
            numeroRepeticiones.Maximum = 100;
            numeroRepeticiones.Value = 12;

            Label labelFecha = new Label();
            labelFecha.Text = "Fecha de vencimiento:";
            labelFecha.Location = new Point(50, 410);
            labelFecha.AutoSize = true;

            fechaVencimiento = new DateTimePicker();
            fechaVencimiento.Location = new Point(50, 435);
            fechaVencimiento.Size = new Size(450, 30);
            fechaVencimiento.Format = DateTimePickerFormat.Short;
            fechaVencimiento.MinDate = DateTime.Today;
            fechaVencimiento.Value = DateTime.Today.AddDays(30);

            Button buttonGuardar = new Button();
            buttonGuardar.Text = "Guardar rutina";
            buttonGuardar.Location = new Point(70, 525);
            buttonGuardar.Size = new Size(190, 45);
            buttonGuardar.Click += buttonGuardar_Click;

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver";
            buttonVolver.Location = new Point(290, 525);
            buttonVolver.Size = new Size(190, 45);
            buttonVolver.Click += (sender, e) => Close();

            Controls.Add(labelTitulo);
            Controls.Add(labelNombre);
            Controls.Add(textNombre);
            Controls.Add(labelDescripcion);
            Controls.Add(textDescripcion);
            Controls.Add(labelTipo);
            Controls.Add(comboTipo);
            Controls.Add(labelDuracion);
            Controls.Add(numeroDuracion);
            Controls.Add(labelSeries);
            Controls.Add(numeroSeries);
            Controls.Add(labelRepeticiones);
            Controls.Add(numeroRepeticiones);
            Controls.Add(labelFecha);
            Controls.Add(fechaVencimiento);
            Controls.Add(buttonGuardar);
            Controls.Add(buttonVolver);
        }

        private void buttonGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                Rutina nuevaRutina = new Rutina();

                nuevaRutina.IdCliente = cliente.Id;
                nuevaRutina.Nombre = textNombre.Text.Trim();
                nuevaRutina.Descripcion = textDescripcion.Text.Trim();
                nuevaRutina.TipoEjercicio =
                    comboTipo.SelectedItem?.ToString() ?? "";

                nuevaRutina.Duracion = (int)numeroDuracion.Value;
                nuevaRutina.Series = (int)numeroSeries.Value;
                nuevaRutina.Repeticiones =
                    (int)numeroRepeticiones.Value;

                nuevaRutina.FechaVencimiento =
                    fechaVencimiento.Value.Date;

                controlador.RegistrarRutina(nuevaRutina);

                MessageBox.Show(
                    "Rutina registrada correctamente.",
                    "Fit.CR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                Close();
            }
            catch (ArgumentException error)
            {
                MessageBox.Show(error.Message);
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "No se pudo guardar la rutina. Intente de nuevo"
                );
            }
        }
    }
}

