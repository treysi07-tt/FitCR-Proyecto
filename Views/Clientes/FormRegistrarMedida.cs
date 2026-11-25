
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FitCR.Controllers;
using FitCR.Models.Medidas;
using FitCR.Models.Usuarios;
using FitCR.Repositories;

namespace FitCR.Views.Clientes
{
    public partial class FormRegistrarMedida : Form
    {
        private readonly Cliente cliente;
        private readonly ControladorMedidas controlador;

        private TextBox textPeso;
        private TextBox textEstatura;

        public FormRegistrarMedida(Cliente clienteRecibido)
        {
            InitializeComponent();

            cliente = clienteRecibido;

            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "medidas.json"
            );

            RepositorioJson<RegistroMedida> repositorio =
                new RepositorioJson<RegistroMedida>(ruta);

            controlador = new ControladorMedidas(repositorio);

            Text = "Fit.CR - Registrar medidas";
            ClientSize = new Size(500, 420);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Registrar nuevas medidas";
            labelTitulo.Location = new Point(55, 25);
            labelTitulo.Size = new Size(390, 45);
            labelTitulo.Font = new Font(
                "Segoe UI", 16, FontStyle.Bold
            );

            Label labelCliente = new Label();
            labelCliente.Text = "Cliente: " + cliente.Nombre;
            labelCliente.Location = new Point(60, 90);
            labelCliente.Size = new Size(380, 30);

            Label labelPeso = new Label();
            labelPeso.Text = "Nuevo peso (kg):";
            labelPeso.Location = new Point(60, 140);
            labelPeso.AutoSize = true;

            textPeso = new TextBox();
            textPeso.Location = new Point(60, 170);
            textPeso.Size = new Size(370, 31);

            Label labelEstatura = new Label();
            labelEstatura.Text = "Estatura (m):";
            labelEstatura.Location = new Point(60, 220);
            labelEstatura.AutoSize = true;

            textEstatura = new TextBox();
            textEstatura.Location = new Point(60, 250);
            textEstatura.Size = new Size(370, 31);

            Button buttonGuardar = new Button();
            buttonGuardar.Text = "Guardar medidas";
            buttonGuardar.Location = new Point(60, 325);
            buttonGuardar.Size = new Size(175, 45);
            buttonGuardar.Click += buttonGuardar_Click;

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver";
            buttonVolver.Location = new Point(255, 325);
            buttonVolver.Size = new Size(175, 45);
            buttonVolver.Click += (sender, e) => Close();

            Controls.Add(labelTitulo);
            Controls.Add(labelCliente);
            Controls.Add(labelPeso);
            Controls.Add(textPeso);
            Controls.Add(labelEstatura);
            Controls.Add(textEstatura);
            Controls.Add(buttonGuardar);
            Controls.Add(buttonVolver);
        }

        private void buttonGuardar_Click(object? sender, EventArgs e)
        {
            if (!double.TryParse(textPeso.Text, out double peso))
            {
                MessageBox.Show("Ingrese un peso válido.");
                return;
            }

            if (!double.TryParse(textEstatura.Text, out double estatura))
            {
                MessageBox.Show("Ingrese una estatura válida.");
                return;
            }

            try
            {
                controlador.RegistrarMedida(
                    cliente.Id,
                    peso,
                    estatura
                );

                MessageBox.Show("Medidas guardadas correctamente.");

                textPeso.Clear();
                textEstatura.Clear();
            }
            catch (ArgumentException error)
            {
                MessageBox.Show(error.Message);
            }
            catch (IOException)
            {
                MessageBox.Show("Error al guardar las medidas.");
            }
        }
    }
}

