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
    public partial class FormEditarRutina : Form
    {
        private readonly Cliente cliente;
        private readonly ControladorRutinas controlador;
        private ComboBox comboRutinas;
        private TextBox textNombre;
        private TextBox textDescripcion;
        private ComboBox comboTipo;
        private NumericUpDown numeroDuracion;
        private NumericUpDown numeroSeries;
        private NumericUpDown numeroRepeticiones;
        private DateTimePicker fechaVencimiento;
        private Rutina? rutinaSeleccionada;

        private void buttonEliminar_Click(object? sender, EventArgs e)
        {
            if (rutinaSeleccionada == null)
            {
                MessageBox.Show(
                    "Primero debes seleccionar una rutina."
                );
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Estás seguro de que deseas eliminar la rutina: "
                + rutinaSeleccionada.Nombre + "?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                controlador.EliminarRutina(
                    rutinaSeleccionada.Id,
                    cliente.Id
                );

                MessageBox.Show(
                    "Rutina eliminada correctamente.",
                    "Fit.CR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                rutinaSeleccionada = null;

                CargarRutinas();
            }
            catch (ArgumentException error)
            {
                MessageBox.Show(error.Message);
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "No se pudo eliminar la rutina. Intente de nuevo"
                );
            }
        }

        public FormEditarRutina(Cliente clienteRecibido)
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

            Text = "Editar rutina";
            ClientSize = new Size(550, 720);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Editar rutina";
            labelTitulo.Location = new Point(170, 20);
            labelTitulo.Size = new Size(300, 45);
            labelTitulo.Font = new Font(
                "Segoe UI", 18, FontStyle.Bold
            );

            Label labelSeleccionar = new Label();
            labelSeleccionar.Text = "Seleccionar rutina:";
            labelSeleccionar.Location = new Point(50, 85);
            labelSeleccionar.AutoSize = true;

            comboRutinas = new ComboBox();
            comboRutinas.Location = new Point(50, 110);
            comboRutinas.Size = new Size(450, 30);
            comboRutinas.DropDownStyle =
                ComboBoxStyle.DropDownList;
            comboRutinas.SelectedIndexChanged +=
                comboRutinas_SelectedIndexChanged;

            Label labelNombre = new Label();
            labelNombre.Text = "Nombre:";
            labelNombre.Location = new Point(50, 160);
            labelNombre.AutoSize = true;

            textNombre = new TextBox();
            textNombre.Location = new Point(50, 185);
            textNombre.Size = new Size(450, 30);

            Label labelDescripcion = new Label();
            labelDescripcion.Text = "Descripción:";
            labelDescripcion.Location = new Point(50, 230);
            labelDescripcion.AutoSize = true;

            textDescripcion = new TextBox();
            textDescripcion.Location = new Point(50, 255);
            textDescripcion.Size = new Size(450, 60);
            textDescripcion.Multiline = true;

            Label labelTipo = new Label();
            labelTipo.Text = "Tipo de ejercicio:";
            labelTipo.Location = new Point(50, 330);
            labelTipo.AutoSize = true;

            comboTipo = new ComboBox();
            comboTipo.Location = new Point(50, 355);
            comboTipo.Size = new Size(450, 30);
            comboTipo.DropDownStyle =
                ComboBoxStyle.DropDownList;

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
            labelDuracion.Location = new Point(50, 405);
            labelDuracion.AutoSize = true;

            numeroDuracion = new NumericUpDown();
            numeroDuracion.Location = new Point(50, 430);
            numeroDuracion.Size = new Size(130, 30);
            numeroDuracion.Minimum = 1;
            numeroDuracion.Maximum = 300;

            Label labelSeries = new Label();
            labelSeries.Text = "Series:";
            labelSeries.Location = new Point(210, 405);
            labelSeries.AutoSize = true;

            numeroSeries = new NumericUpDown();
            numeroSeries.Location = new Point(210, 430);
            numeroSeries.Size = new Size(120, 30);
            numeroSeries.Minimum = 1;
            numeroSeries.Maximum = 20;

            Label labelRepeticiones = new Label();
            labelRepeticiones.Text = "Repeticiones:";
            labelRepeticiones.Location = new Point(360, 405);
            labelRepeticiones.AutoSize = true;

            numeroRepeticiones = new NumericUpDown();
            numeroRepeticiones.Location = new Point(360, 430);
            numeroRepeticiones.Size = new Size(140, 30);
            numeroRepeticiones.Minimum = 1;
            numeroRepeticiones.Maximum = 100;

            Label labelFecha = new Label();
            labelFecha.Text = "Fecha de vencimiento:";
            labelFecha.Location = new Point(50, 485);
            labelFecha.AutoSize = true;

            fechaVencimiento = new DateTimePicker();
            fechaVencimiento.Location = new Point(50, 510);
            fechaVencimiento.Size = new Size(450, 30);
            fechaVencimiento.Format =
                DateTimePickerFormat.Short;


            // Boton eliminar
            Button buttonEliminar = new Button();
            buttonEliminar.Text = "Eliminar rutina";
            buttonEliminar.Location = new Point(180, 565);
            buttonEliminar.Size = new Size(190, 35);
            buttonEliminar.Click += buttonEliminar_Click;

            Button buttonGuardar = new Button();
            buttonGuardar.Text = "Guardar cambios";
            buttonGuardar.Location = new Point(70, 605);
            buttonGuardar.Size = new Size(190, 45);
            buttonGuardar.Click += buttonGuardar_Click;

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver";
            buttonVolver.Location = new Point(290, 605);
            buttonVolver.Size = new Size(190, 45);
            buttonVolver.Click += (sender, e) => Close();

            Controls.Add(labelTitulo);
            Controls.Add(labelSeleccionar);
            Controls.Add(comboRutinas);
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
            Controls.Add(buttonEliminar);
            CargarRutinas();
        }

        private void CargarRutinas()
        {
            try
            {
                var rutinas = controlador.ObtenerRutinas(cliente.Id);
                comboRutinas.DataSource = null;
                comboRutinas.DataSource = rutinas;
                comboRutinas.DisplayMember = "Nombre";
                comboRutinas.ValueMember = "Id";

                if (rutinas.Count == 0)
                {
                    MessageBox.Show(
                        "No tienes rutinas para editar."
                    );
                }
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "No se pudieron cargar las rutinas. Intente de nuevo"
                );
            }
        }

        private void comboRutinas_SelectedIndexChanged(
            object? sender, EventArgs e)
        {
            if (comboRutinas.SelectedItem is not Rutina rutina)
            {
                rutinaSeleccionada = null;
                return;
            }

            rutinaSeleccionada = rutina;
            textNombre.Text = rutina.Nombre;
            textDescripcion.Text = rutina.Descripcion;
            comboTipo.SelectedItem = rutina.TipoEjercicio;

            numeroDuracion.Value = Math.Clamp(
                rutina.Duracion,
                (int)numeroDuracion.Minimum,
                (int)numeroDuracion.Maximum
            );

            numeroSeries.Value = Math.Clamp(
                rutina.Series,
                (int)numeroSeries.Minimum,
                (int)numeroSeries.Maximum
            );

            numeroRepeticiones.Value = Math.Clamp(
                rutina.Repeticiones,
                (int)numeroRepeticiones.Minimum,
                (int)numeroRepeticiones.Maximum
            );

            fechaVencimiento.Value =
                rutina.FechaVencimiento >= fechaVencimiento.MinDate
                && rutina.FechaVencimiento <= fechaVencimiento.MaxDate
                ? rutina.FechaVencimiento
                : DateTime.Today;
        }

        private void buttonGuardar_Click(object? sender, EventArgs e)
        {
            if (rutinaSeleccionada == null)
            {
                MessageBox.Show(
                    "Primero debes seleccionar una rutina."
                );
                return;
            }

            try
            {
                Rutina rutinaEditada = new Rutina
                {
                    Id = rutinaSeleccionada.Id,
                    IdCliente = cliente.Id,
                    Nombre = textNombre.Text.Trim(),
                    Descripcion = textDescripcion.Text.Trim(),
                    TipoEjercicio =
                        comboTipo.SelectedItem?.ToString() ?? "",
                    Duracion = (int)numeroDuracion.Value,
                    Series = (int)numeroSeries.Value,
                    Repeticiones =
                        (int)numeroRepeticiones.Value,
                    FechaVencimiento =
                        fechaVencimiento.Value.Date
                };

                controlador.EditarRutina(rutinaEditada);

                MessageBox.Show(
                    "Rutina actualizada correctamente.",
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
                    "No se pudieron guardar los cambios. Intente de nuevo"
                );
            }
        }
    }
}

