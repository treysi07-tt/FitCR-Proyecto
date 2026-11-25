using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FitCR.Controllers;
using FitCR.Models.Usuarios;
using FitCR.Repositories;
using FitCR.Views.Clientes;

namespace FitCR.Views.Entrenadores
{
    public partial class FormClientesAsignados : Form
    {
        private readonly EntrenadorPersonal entrenador;
        private readonly ControladorEntrenadores controlador;
        private ComboBox comboClientes = new ComboBox();
        private DataGridView tablaClientes = new DataGridView();
        private Label labelCantidad = new Label();

        public FormClientesAsignados(
            EntrenadorPersonal entrenadorRecibido)
        {
            InitializeComponent();

            entrenador = entrenadorRecibido;

            string rutaData = Path.Combine(
                AppContext.BaseDirectory, "Data");

            controlador = new ControladorEntrenadores(
                new RepositorioJson<EntrenadorPersonal>(
                    Path.Combine(rutaData,
                    "entrenadores_personales.json")),

                new RepositorioJson<EntrenadorGimnasio>(
                    Path.Combine(rutaData,
                    "entrenadores_gimnasio.json")),

                new RepositorioJson<Cliente>(
                    Path.Combine(rutaData,
                    "clientes.json"))
            );

            ConfigurarFormulario();
            CargarClientes();
        }

        private void ConfigurarFormulario()
        {
            Controls.Clear();

            Text = "Clientes asignados";
            Size = new Size(1000, 850);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
            AutoScroll = true;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Clientes asignados";
            labelTitulo.Font = new Font(
                "Segoe UI", 24, FontStyle.Bold);
            labelTitulo.Location = new Point(30, 5);
            labelTitulo.Size = new Size(920, 65);
            labelTitulo.AutoSize = false;
            labelTitulo.TextAlign = ContentAlignment.MiddleCenter;
            labelTitulo.TextAlign =
                ContentAlignment.MiddleCenter;

            Label labelEntrenador = new Label();
            labelEntrenador.Text =
                "Entrenador: " + entrenador.Nombre;
            labelEntrenador.Font =
                new Font("Segoe UI", 12);
            labelEntrenador.Location = new Point(30, 80);
            labelEntrenador.Size = new Size(900, 30);

            Label labelSeleccionar = new Label();
            labelSeleccionar.Text = "Seleccionar cliente";
            labelSeleccionar.Font =
                new Font("Segoe UI", 11);
            labelSeleccionar.Location =
                new Point(30, 125);
            labelSeleccionar.Size = new Size(400, 25);

            comboClientes.Name = "comboClientes";
            comboClientes.Location = new Point(30, 160);
            comboClientes.Size = new Size(600, 35);
            comboClientes.Font = new Font("Segoe UI", 11);
            comboClientes.DropDownStyle =
                ComboBoxStyle.DropDownList;

            Button buttonAsignar = new Button();
            buttonAsignar.Text = "Asignar cliente";
            buttonAsignar.Location = new Point(660, 155);
            buttonAsignar.Size = new Size(270, 40);
            buttonAsignar.Font = new Font("Segoe UI", 11);
            buttonAsignar.Click += buttonAsignar_Click;

            labelCantidad.Location = new Point(30, 220);
            labelCantidad.Size = new Size(900, 30);
            labelCantidad.Font = new Font(
                "Segoe UI", 12, FontStyle.Bold);

            tablaClientes.Name = "tablaClientes";
            tablaClientes.Location = new Point(30, 260);
            tablaClientes.Size = new Size(900, 270);
            tablaClientes.ReadOnly = true;
            tablaClientes.AllowUserToAddRows = false;
            tablaClientes.AllowUserToDeleteRows = false;
            tablaClientes.MultiSelect = false;
            tablaClientes.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            tablaClientes.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            tablaClientes.RowHeadersVisible = false;
            tablaClientes.BackgroundColor = Color.White;

            Button buttonRetirar = new Button();
            buttonRetirar.Text = "Retirar cliente";
            buttonRetirar.Location = new Point(30, 560);
            buttonRetirar.Size = new Size(270, 45);
            buttonRetirar.Font = new Font("Segoe UI", 11);
            buttonRetirar.Click += buttonRetirar_Click;

            Button buttonActualizar = new Button();
            buttonActualizar.Text = "Actualizar";
            buttonActualizar.Location = new Point(345, 560);
            buttonActualizar.Size = new Size(270, 45);
            buttonActualizar.Font =
                new Font("Segoe UI", 11);
            buttonActualizar.Click += buttonActualizar_Click;

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver";
            buttonVolver.Location = new Point(660, 560);
            buttonVolver.Size = new Size(270, 45);
            buttonVolver.Font = new Font("Segoe UI", 11);
            buttonVolver.Click += buttonVolver_Click;

            Button buttonProgreso = new Button();
            buttonProgreso.Name = "buttonProgreso";
            buttonProgreso.Text = "Ver progreso";
            buttonProgreso.Location = new Point(30, 625);
            buttonProgreso.Size = new Size(270, 45);
            buttonProgreso.Font = new Font("Segoe UI", 11);
            buttonProgreso.Click += buttonProgreso_Click;

            Button buttonObjetivos = new Button();
            buttonObjetivos.Name = "buttonObjetivos";
            buttonObjetivos.Text = "Ver progreso de objetivos";
            buttonObjetivos.Location = new Point(345, 625);
            buttonObjetivos.Size = new Size(300, 55);
            buttonObjetivos.Font = new Font("Segoe UI", 10);
            buttonObjetivos.TextAlign = ContentAlignment.MiddleCenter;
            buttonObjetivos.Click += buttonObjetivos_Click;

            Button buttonRutinas = new Button();
            buttonRutinas.Name = "buttonRutinas";
            buttonRutinas.Text = "Ver rutinas del cliente";
            buttonRutinas.Location = new Point(30, 700);
            buttonRutinas.Size = new Size(300, 55);
            buttonRutinas.Font = new Font("Segoe UI", 10);
            buttonRutinas.TextAlign = ContentAlignment.MiddleCenter;
            buttonRutinas.Click += buttonRutinas_Click;

            Button buttonAsignarRutina = new Button();
            buttonAsignarRutina.Name = "buttonAsignarRutina";
            buttonAsignarRutina.Text = "Asignar nueva rutina";
            buttonAsignarRutina.Location = new Point(345, 700);
            buttonAsignarRutina.Size = new Size(300, 55);
            buttonAsignarRutina.Font = new Font("Segoe UI", 10);
            buttonAsignarRutina.TextAlign = ContentAlignment.MiddleCenter;
            buttonAsignarRutina.Click += buttonAsignarRutina_Click;

            Controls.Add(labelTitulo);
            Controls.Add(labelEntrenador);
            Controls.Add(labelSeleccionar);
            Controls.Add(comboClientes);
            Controls.Add(buttonAsignar);
            Controls.Add(labelCantidad);
            Controls.Add(tablaClientes);
            Controls.Add(buttonRetirar);
            Controls.Add(buttonActualizar);
            Controls.Add(buttonVolver);
            Controls.Add(buttonProgreso);
            Controls.Add(buttonObjetivos);
            Controls.Add(buttonRutinas);
            Controls.Add(buttonAsignarRutina);
        }

        private void buttonAsignarRutina_Click(
    object? sender, EventArgs e)
        {
            try
            {
                Cliente? cliente = ObtenerClienteSeleccionado();

                if (cliente == null)
                {
                    return;
                }

                using (FormAgregarRutina formulario =
                    new FormAgregarRutina(cliente))
                {
                    formulario.ShowDialog();
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

        private void buttonRutinas_Click(
            object? sender, EventArgs e)
        {
            try
            {
                Cliente? cliente = ObtenerClienteSeleccionado();

                if (cliente == null)
                {
                    return;
                }

                using (FormVerRutinas formulario =
                    new FormVerRutinas(cliente, false, true))
                {
                    formulario.ShowDialog();
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

        private void buttonObjetivos_Click(
            object? sender, EventArgs e)
        {
            try
            {
                Cliente? cliente = ObtenerClienteSeleccionado();

                if (cliente == null)
                {
                    return;
                }

                using (FormProgresoObjetivos formulario =
                    new FormProgresoObjetivos(cliente))
                {
                    formulario.ShowDialog();
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

        private Cliente? ObtenerClienteSeleccionado()
        {
            if (tablaClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Debe seleccionar un cliente de la tabla.",
                    "Cliente requerido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return null;
            }

            int idCliente = Convert.ToInt32(
                tablaClientes.SelectedRows[0]
                .Cells["Id"].Value);

            Cliente? cliente =
                controlador.ObtenerClientesAsignados(
                    entrenador.Id)
                .FirstOrDefault(c => c.Id == idCliente);

            if (cliente == null)
            {
                MessageBox.Show(
                    "El cliente no está asignado a este entrenador.",
                    "Cliente no disponible",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            return cliente;
        }

        private void buttonProgreso_Click(
            object? sender, EventArgs e)
        {
            try
            {
                Cliente? cliente = ObtenerClienteSeleccionado();

                if (cliente == null)
                {
                    return;
                }

                using (FormProgreso formulario =
                    new FormProgreso(cliente))
                {
                    formulario.ShowDialog();
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

        private void CargarClientes()
        {
            var asignados =
                controlador.ObtenerClientesAsignados(
                    entrenador.Id);

            var idsAsignados =
                asignados.Select(c => c.Id).ToHashSet();

            var disponibles =
                controlador.ObtenerClientesDisponibles()
                .Where(c => !idsAsignados.Contains(c.Id))
                .Select(c => new
                {
                    c.Id,
                    NombreCompleto =
                        c.Nombre + " (" + c.NombreUsuario + ")"
                })
                .ToList();

            comboClientes.DataSource = null;
            comboClientes.DisplayMember = "NombreCompleto";
            comboClientes.ValueMember = "Id";
            comboClientes.DataSource = disponibles;
            comboClientes.SelectedIndex = -1;

            tablaClientes.DataSource =
                asignados.Select(c => new
                {
                    c.Id,
                    c.Nombre,
                    c.NombreUsuario,
                    c.Edad,
                    c.Peso,
                    c.Estatura
                })
                .ToList();

            DataGridViewColumn? columnaUsuario =
                tablaClientes.Columns["NombreUsuario"];

            if (columnaUsuario != null)
            {
                columnaUsuario.HeaderText =
                    "Nombre de usuario";
            }

            labelCantidad.Text =
                "Clientes asignados: " + asignados.Count;

            tablaClientes.ClearSelection();
        }

        private void buttonAsignar_Click(
            object? sender, EventArgs e)
        {
            try
            {
                if (comboClientes.SelectedValue
                    is not int idCliente)
                {
                    MessageBox.Show(
                        "Debe seleccionar un cliente.",
                        "Cliente requerido",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                controlador.AsignarCliente(
                    entrenador.Id,
                    idCliente);

                CargarClientes();

                MessageBox.Show(
                    "Cliente asignado correctamente.",
                    "Asignación exitosa",
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

        private void buttonRetirar_Click(
            object? sender, EventArgs e)
        {
            try
            {
                if (tablaClientes.SelectedRows.Count == 0)
                {
                    MessageBox.Show(
                        "Debe seleccionar un cliente de la tabla.",
                        "Cliente requerido",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                int idCliente = Convert.ToInt32(
                    tablaClientes.SelectedRows[0]
                    .Cells["Id"].Value);

                DialogResult respuesta = MessageBox.Show(
                    "¿Desea retirar la asignación de este cliente?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (respuesta != DialogResult.Yes)
                {
                    return;
                }

                controlador.RetirarCliente(
                    entrenador.Id,
                    idCliente);

                CargarClientes();

                MessageBox.Show(
                    "Asignación retirada correctamente.",
                    "Operación exitosa",
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

        private void buttonActualizar_Click(
            object? sender, EventArgs e)
        {
            CargarClientes();
        }

        private void buttonVolver_Click(
            object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}
