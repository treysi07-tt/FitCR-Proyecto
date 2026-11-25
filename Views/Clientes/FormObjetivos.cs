using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FitCR.Controllers;
using FitCR.Models.Usuarios;
using FitCR.Models.Objetivos;
using FitCR.Models.Rutinas;
using FitCR.Models.Medidas;
using FitCR.Models.Visitas;
using FitCR.Repositories;

namespace FitCR.Views.Clientes
{
    public partial class FormObjetivos : Form
    {
        private readonly Cliente cliente;
        private readonly ControladorObjetivos controlador;
        private TextBox textNombre;
        private TextBox textDescripcion;
        private ComboBox comboTipo;
        private NumericUpDown numeroMeta;
        private DateTimePicker fechaLimite;
        private DataGridView tablaObjetivos;
        private Label labelCompletados;

        public FormObjetivos(Cliente clienteRecibido)
        {
            InitializeComponent();

            cliente = clienteRecibido;

            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "objetivos.json"
            );

            RepositorioJson<Objetivo> repositorio =
                new RepositorioJson<Objetivo>(ruta);

            controlador = new ControladorObjetivos(repositorio);

            Text = "Mis objetivos";
            ClientSize = new Size(1000, 840);
            AutoScroll = true;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Mis objetivos personales";
            labelTitulo.Location = new Point(310, 20);
            labelTitulo.Size = new Size(450, 45);
            labelTitulo.Font = new Font(
                "Segoe UI", 18, FontStyle.Bold
            );

            Label labelCliente = new Label();
            labelCliente.Text = "Cliente: " + cliente.Nombre;
            labelCliente.Location = new Point(50, 80);
            labelCliente.Size = new Size(700, 30);

            Label labelNombre = new Label();
            labelNombre.Text = "Nombre del objetivo:";
            labelNombre.Location = new Point(50, 130);
            labelNombre.AutoSize = true;

            textNombre = new TextBox();
            textNombre.Location = new Point(50, 155);
            textNombre.Size = new Size(420, 30);
            textNombre.MaxLength = 100;

            Label labelTipo = new Label();
            labelTipo.Text = "Tipo de objetivo:";
            labelTipo.Location = new Point(520, 130);
            labelTipo.AutoSize = true;

            comboTipo = new ComboBox();
            comboTipo.Location = new Point(520, 155);
            comboTipo.Size = new Size(420, 30);
            comboTipo.DropDownStyle =
                ComboBoxStyle.DropDownList;
            comboTipo.Items.AddRange(new object[]
            {
                "Peso",
                "Rutinas completadas",
                "Visitas al gimnasio"
            });

            Label labelDescripcion = new Label();
            labelDescripcion.Text = "Descripción:";
            labelDescripcion.Location = new Point(50, 205);
            labelDescripcion.AutoSize = true;

            textDescripcion = new TextBox();
            textDescripcion.Location = new Point(50, 230);
            textDescripcion.Size = new Size(890, 55);
            textDescripcion.Multiline = true;

            Label labelMeta = new Label();
            labelMeta.Text = "Valor de la meta:";
            labelMeta.Location = new Point(50, 305);
            labelMeta.AutoSize = true;

            numeroMeta = new NumericUpDown();
            numeroMeta.Location = new Point(50, 330);
            numeroMeta.Size = new Size(200, 30);
            numeroMeta.Minimum = 1;
            numeroMeta.Maximum = 10000;
            numeroMeta.DecimalPlaces = 2;
            numeroMeta.Increment = 1;

            Label labelFecha = new Label();
            labelFecha.Text = "Fecha límite:";
            labelFecha.Location = new Point(320, 305);
            labelFecha.AutoSize = true;

            fechaLimite = new DateTimePicker();
            fechaLimite.Location = new Point(320, 330);
            fechaLimite.Size = new Size(250, 30);
            fechaLimite.Format = DateTimePickerFormat.Short;
            fechaLimite.MinDate = DateTime.Today;
            fechaLimite.Value = DateTime.Today.AddDays(30);

            Button buttonRegistrar = new Button();
            buttonRegistrar.Text = "Registrar objetivo";
            buttonRegistrar.Location = new Point(680, 320);
            buttonRegistrar.Size = new Size(260, 45);
            buttonRegistrar.Click += buttonRegistrar_Click;

            labelCompletados = new Label();
            labelCompletados.Location = new Point(50, 390);
            labelCompletados.Size = new Size(600, 30);
            labelCompletados.Font = new Font(
                "Segoe UI", 11, FontStyle.Bold
            );

            Label labelHistorial = new Label();
            labelHistorial.Text = "Mis objetivos registrados";
            labelHistorial.Location = new Point(50, 435);
            labelHistorial.Size = new Size(450, 35);
            labelHistorial.Font = new Font(
                "Segoe UI", 14, FontStyle.Bold
            );

            tablaObjetivos = new DataGridView();
            tablaObjetivos.Location = new Point(50, 480);
            tablaObjetivos.Size = new Size(890, 170);
            tablaObjetivos.ReadOnly = true;
            tablaObjetivos.AllowUserToAddRows = false;
            tablaObjetivos.AllowUserToDeleteRows = false;
            tablaObjetivos.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            tablaObjetivos.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            tablaObjetivos.RowHeadersVisible = false;
            tablaObjetivos.MultiSelect = false;

            Button buttonEditar = new Button();
            buttonEditar.Text = "Editar objetivo";
            buttonEditar.Location = new Point(50, 680);
            buttonEditar.Size = new Size(200, 45);
            buttonEditar.Click += buttonEditar_Click;

            Button buttonEliminar = new Button();
            buttonEliminar.Text = "Eliminar objetivo";
            buttonEliminar.Location = new Point(270, 680);
            buttonEliminar.Size = new Size(200, 45);
            buttonEliminar.Click += buttonEliminar_Click;

            Button buttonCompletar = new Button();
            buttonCompletar.Text = "Actualizar";
            buttonCompletar.Size = new Size(210, 50);
            buttonCompletar.Location = new Point(50, 680);
            buttonCompletar.Font = new Font("Segoe UI", 10);
            buttonCompletar.TextAlign = ContentAlignment.MiddleCenter;
            buttonCompletar.Click += buttonCompletar_Click;

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver al menú";
            buttonVolver.Location = new Point(730, 680);
            buttonVolver.Size = new Size(210, 50);
            buttonVolver.Click += (sender, e) => Close();

            Button buttonProgresoObjetivos = new Button();
            buttonProgresoObjetivos.Text = "Progreso de mis objetivos";
            buttonProgresoObjetivos.Location = new Point(350, 750);
            buttonProgresoObjetivos.Size = new Size(300, 45);
            buttonProgresoObjetivos.Click += buttonProgresoObjetivos_Click;

            Controls.Add(labelTitulo);
            Controls.Add(labelCliente);
            Controls.Add(labelNombre);
            Controls.Add(textNombre);
            Controls.Add(labelTipo);
            Controls.Add(comboTipo);
            Controls.Add(labelDescripcion);
            Controls.Add(textDescripcion);
            Controls.Add(labelMeta);
            Controls.Add(numeroMeta);
            Controls.Add(labelFecha);
            Controls.Add(fechaLimite);
            Controls.Add(buttonRegistrar);
            Controls.Add(labelCompletados);
            Controls.Add(labelHistorial);
            Controls.Add(tablaObjetivos);
            Controls.Add(buttonCompletar);
            Controls.Add(buttonVolver);
            Controls.Add(buttonEditar);
            Controls.Add(buttonEliminar);
            Controls.Add(buttonProgresoObjetivos);
            CargarObjetivos();
        }

        private int SincronizarObjetivos()
        {
            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data"
            );

            var repositorioObjetivos =
                new RepositorioJson<Objetivo>(
                    Path.Combine(ruta, "objetivos.json")
                );

            var repositorioRutinas =
                new RepositorioJson<Rutina>(
                    Path.Combine(ruta, "rutinas.json")
                );

            var repositorioMedidas =
                new RepositorioJson<RegistroMedida>(
                    Path.Combine(ruta, "medidas.json")
                );

            var repositorioVisitas =
                new RepositorioJson<RegistroVisita>(
                    Path.Combine(ruta, "visitas.json")
                );

            var controladorProgreso =
                new ControladorProgresoObjetivos(
                    repositorioObjetivos,
                    repositorioRutinas,
                    repositorioMedidas,
                    repositorioVisitas
                );

            return controladorProgreso.SincronizarObjetivos(
                cliente.Id
            );
        }


        private void buttonProgresoObjetivos_Click(
            object? sender, EventArgs e)
        {
            using (FormProgresoObjetivos formulario =
                new FormProgresoObjetivos(cliente))
            {
                formulario.ShowDialog();
            }

            CargarObjetivos();
        }

        private int ObtenerIdSeleccionado()
        {
            if (tablaObjetivos.SelectedRows.Count == 0)
            {
                throw new ArgumentException(
                    "Primero debes seleccionar un objetivo."
                );
            }

            DataGridViewRow fila =
                tablaObjetivos.SelectedRows[0];

            if (fila.Cells["Id"].Value == null)
            {
                throw new ArgumentException(
                    "No se pudo identificar el objetivo."
                );
            }

            return Convert.ToInt32(
                fila.Cells["Id"].Value
            );
        }

        private void buttonEditar_Click(object? sender, EventArgs e)
        {
            try
            {
                int idObjetivo = ObtenerIdSeleccionado();

                Objetivo? objetivo = controlador
                    .ObtenerObjetivos(cliente.Id)
                    .Find(o => o.Id == idObjetivo);

                if (objetivo == null)
                {
                    throw new ArgumentException(
                        "No se encontró el objetivo."
                    );
                }

                textNombre.Text = objetivo.Nombre;
                textDescripcion.Text = objetivo.Descripcion;
                comboTipo.SelectedItem = objetivo.TipoObjetivo;

                decimal valorMeta = (decimal)objetivo.ValorMeta;

                if (valorMeta >= numeroMeta.Minimum &&
                    valorMeta <= numeroMeta.Maximum)
                {
                    numeroMeta.Value = valorMeta;
                }

                if (objetivo.FechaLimite >= fechaLimite.MinDate &&
                    objetivo.FechaLimite <= fechaLimite.MaxDate)
                {
                    fechaLimite.Value = objetivo.FechaLimite;
                }
                else
                {
                    throw new ArgumentException(
                        "La fecha límite del objetivo está fuera del rango permitido."
                    );
                }

                DialogResult respuesta = MessageBox.Show(
                    "Los datos del objetivo se cargaron en los campos.\n\n" +
                    "Modifica los valores que necesites y presiona Aceptar " +
                    "para guardar los cambios.",
                    "Editar objetivo",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information
                );

                if (respuesta != DialogResult.OK)
                {
                    return;
                }

                using (Form ventanaEditar = new Form())
                {
                    ventanaEditar.Text = "Editar objetivo";
                    ventanaEditar.ClientSize = new Size(450, 340);
                    ventanaEditar.StartPosition =
                        FormStartPosition.CenterParent;
                    ventanaEditar.FormBorderStyle =
                        FormBorderStyle.FixedDialog;
                    ventanaEditar.MaximizeBox = false;
                    ventanaEditar.MinimizeBox = false;

                    Label labelNombreEditar = new Label();
                    labelNombreEditar.Text = "Nombre:";
                    labelNombreEditar.Location = new Point(25, 20);
                    labelNombreEditar.AutoSize = true;

                    TextBox textNombreEditar = new TextBox();
                    textNombreEditar.Location = new Point(25, 45);
                    textNombreEditar.Size = new Size(390, 25);
                    textNombreEditar.MaxLength = 100;
                    textNombreEditar.Text = objetivo.Nombre;

                    Label labelDescripcionEditar = new Label();
                    labelDescripcionEditar.Text = "Descripción:";
                    labelDescripcionEditar.Location = new Point(25, 85);
                    labelDescripcionEditar.AutoSize = true;

                    TextBox textDescripcionEditar = new TextBox();
                    textDescripcionEditar.Location = new Point(25, 110);
                    textDescripcionEditar.Size = new Size(390, 45);
                    textDescripcionEditar.Multiline = true;
                    textDescripcionEditar.Text = objetivo.Descripcion;

                    Label labelTipoEditar = new Label();
                    labelTipoEditar.Text = "Tipo:";
                    labelTipoEditar.Location = new Point(25, 170);
                    labelTipoEditar.AutoSize = true;

                    ComboBox comboTipoEditar = new ComboBox();
                    comboTipoEditar.Location = new Point(25, 195);
                    comboTipoEditar.Size = new Size(190, 25);
                    comboTipoEditar.DropDownStyle =
                        ComboBoxStyle.DropDownList;
                    comboTipoEditar.Items.AddRange(new object[]
                    {
                        "Peso",
                        "Rutinas completadas",
                        "Visitas al gimnasio"
                    });
                    comboTipoEditar.SelectedItem =
                        objetivo.TipoObjetivo;

                    Label labelMetaEditar = new Label();
                    labelMetaEditar.Text = "Meta:";
                    labelMetaEditar.Location = new Point(245, 170);
                    labelMetaEditar.AutoSize = true;

                    NumericUpDown numeroMetaEditar = new NumericUpDown();
                    numeroMetaEditar.Location = new Point(245, 195);
                    numeroMetaEditar.Size = new Size(170, 25);
                    numeroMetaEditar.Minimum = 1;
                    numeroMetaEditar.Maximum = 10000;
                    numeroMetaEditar.DecimalPlaces = 2;

                    decimal metaActual = (decimal)objetivo.ValorMeta;
                    numeroMetaEditar.Value = Math.Clamp(
                        metaActual,
                        numeroMetaEditar.Minimum,
                        numeroMetaEditar.Maximum
                    );

                    Label labelFechaEditar = new Label();
                    labelFechaEditar.Text = "Fecha límite:";
                    labelFechaEditar.Location = new Point(25, 235);
                    labelFechaEditar.AutoSize = true;

                    DateTimePicker fechaEditar = new DateTimePicker();
                    fechaEditar.Location = new Point(120, 230);
                    fechaEditar.Size = new Size(190, 25);
                    fechaEditar.Format = DateTimePickerFormat.Short;
                    fechaEditar.Value = objetivo.FechaLimite;

                    Button buttonGuardar = new Button();
                    buttonGuardar.Text = "Guardar";
                    buttonGuardar.Location = new Point(100, 280);
                    buttonGuardar.Size = new Size(110, 35);
                    buttonGuardar.DialogResult = DialogResult.OK;

                    Button buttonCancelar = new Button();
                    buttonCancelar.Text = "Cancelar";
                    buttonCancelar.Location = new Point(240, 280);
                    buttonCancelar.Size = new Size(110, 35);
                    buttonCancelar.DialogResult = DialogResult.Cancel;

                    ventanaEditar.Controls.Add(labelNombreEditar);
                    ventanaEditar.Controls.Add(textNombreEditar);
                    ventanaEditar.Controls.Add(labelDescripcionEditar);
                    ventanaEditar.Controls.Add(textDescripcionEditar);
                    ventanaEditar.Controls.Add(labelTipoEditar);
                    ventanaEditar.Controls.Add(comboTipoEditar);
                    ventanaEditar.Controls.Add(labelMetaEditar);
                    ventanaEditar.Controls.Add(numeroMetaEditar);
                    ventanaEditar.Controls.Add(labelFechaEditar);
                    ventanaEditar.Controls.Add(fechaEditar);
                    ventanaEditar.Controls.Add(buttonGuardar);
                    ventanaEditar.Controls.Add(buttonCancelar);

                    ventanaEditar.AcceptButton = buttonGuardar;
                    ventanaEditar.CancelButton = buttonCancelar;

                    if (ventanaEditar.ShowDialog(this) !=
                        DialogResult.OK)
                    {
                        return;
                    }

                    Objetivo objetivoEditado = new Objetivo
                    {
                        Id = objetivo.Id,
                        IdCliente = cliente.Id,
                        Nombre = textNombreEditar.Text.Trim(),
                        Descripcion =
                            textDescripcionEditar.Text.Trim(),
                        TipoObjetivo =
                            comboTipoEditar.SelectedItem?.ToString() ?? "",
                        ValorMeta = (double)numeroMetaEditar.Value,
                        FechaLimite = fechaEditar.Value.Date,
                        FechaCreacion = objetivo.FechaCreacion,
                        Completado = objetivo.Completado
                    };

                    controlador.EditarObjetivo(objetivoEditado);
                }

                MessageBox.Show(
                    "Objetivo actualizado correctamente.",
                    "Fit.CR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarObjetivos();
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
                    "No se pudo actualizar el objetivo."
                );
            }
        }

        private void buttonEliminar_Click(object? sender, EventArgs e)
        {
            try
            {
                int idObjetivo = ObtenerIdSeleccionado();

                DialogResult respuesta = MessageBox.Show(
                    "¿Estás seguro de que deseas eliminar este objetivo?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (respuesta != DialogResult.Yes)
                {
                    return;
                }

                controlador.EliminarObjetivo(
                    idObjetivo,
                    cliente.Id
                );

                MessageBox.Show(
                    "Objetivo eliminado correctamente.",
                    "Fit.CR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarObjetivos();
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
                    "No se pudo eliminar el objetivo."
                );
            }
        }


        private void buttonRegistrar_Click(object? sender, EventArgs e)
        {
            try
            {
                Objetivo nuevoObjetivo = new Objetivo
                {
                    IdCliente = cliente.Id,
                    Nombre = textNombre.Text.Trim(),
                    Descripcion = textDescripcion.Text.Trim(),
                    TipoObjetivo =
                        comboTipo.SelectedItem?.ToString() ?? "",
                    ValorMeta = (double)numeroMeta.Value,
                    FechaLimite = fechaLimite.Value.Date
                };

                controlador.RegistrarObjetivo(nuevoObjetivo);

                MessageBox.Show(
                    "Objetivo registrado correctamente.",
                    "Fit.CR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                textNombre.Clear();
                textDescripcion.Clear();
                comboTipo.SelectedIndex = -1;
                numeroMeta.Value = 1;

                CargarObjetivos();
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
                    "No se pudo guardar el objetivo. Intente de nuevo"
                );
            }
        }

        private void CargarObjetivos()
        {
            try
            {
                SincronizarObjetivos();
                var objetivos =
                    controlador.ObtenerObjetivos(cliente.Id);

                tablaObjetivos.DataSource = objetivos
                    .ConvertAll(o => new
                    {
                        o.Id,
                        o.Nombre,
                        o.TipoObjetivo,
                        o.ValorMeta,
                        o.FechaLimite,
                        Estado = o.Completado
                            ? "Completado"
                            : "Pendiente"
                    });

                DataGridViewColumn? columnaFecha =
                    tablaObjetivos.Columns["FechaLimite"];

                if (columnaFecha != null)
                {
                    columnaFecha.HeaderText = "Fecha límite";
                    columnaFecha.DefaultCellStyle.Format =
                        "dd/MM/yyyy";
                }

                labelCompletados.Text =
                    "Objetivos completados: " +
                    controlador.ContarObjetivosCompletados(
                        cliente.Id
                    );

                tablaObjetivos.ClearSelection();
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "No se pudieron cargar los objetivos."
                );
            }
        }

        private void buttonCompletar_Click(
            object? sender, EventArgs e)
        {
            try
            {
                int cantidad = SincronizarObjetivos();

                CargarObjetivos();

                string mensaje;

                if (cantidad > 0)
                {
                    mensaje =
                        "Objetivos actualizados: " + cantidad;
                }
                else
                {
                    mensaje =
                        "Todos los objetivos están actualizados.";
                }

                MessageBox.Show(
                    mensaje,
                    "Fit.CR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
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

    }
}

