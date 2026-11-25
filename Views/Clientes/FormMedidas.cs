using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FitCR.Controllers;
using FitCR.Models.Medidas;
using FitCR.Models.Usuarios;
using FitCR.Repositories;

namespace FitCR.Views.Clientes
{
    public partial class FormMedidas : Form
    {
        private readonly Cliente cliente;
        private readonly ControladorMedidas controlador;
        private Label labelPeso;
        private Label labelEstatura;
        private Label labelIMC;
        public FormMedidas(Cliente clienteRecibido)
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

            Text = "Mis medidas";
            ClientSize = new Size(550, 520);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Mis medidas corporales";
            labelTitulo.Location = new Point(110, 25);
            labelTitulo.Size = new Size(380, 45);
            labelTitulo.Font = new Font(
                "Segoe UI", 16, FontStyle.Bold
            );

            Label labelCliente = new Label();
            labelCliente.Text = "Cliente: " + cliente.Nombre;
            labelCliente.Location = new Point(65, 90);
            labelCliente.Size = new Size(420, 30);

            labelPeso = new Label();
            labelPeso.Location = new Point(65, 145);
            labelPeso.Size = new Size(420, 35);
            labelPeso.Font = new Font(
                "Segoe UI", 11, FontStyle.Regular
            );

            labelEstatura = new Label();
            labelEstatura.Location = new Point(65, 190);
            labelEstatura.Size = new Size(420, 35);
            labelEstatura.Font = new Font(
                "Segoe UI", 11, FontStyle.Regular
            );

            labelIMC = new Label();
            labelIMC.Location = new Point(65, 235);
            labelIMC.Size = new Size(420, 35);
            labelIMC.Font = new Font(
                "Segoe UI", 11, FontStyle.Regular
            );

            Button buttonRegistrar = new Button();
            buttonRegistrar.Text = "Registrar nuevas medidas";
            buttonRegistrar.Location = new Point(125, 305);
            buttonRegistrar.Size = new Size(300, 45);
            buttonRegistrar.Click += buttonRegistrar_Click;

            Button buttonHistorial = new Button();
            buttonHistorial.Text = "Ver historial de medidas";
            buttonHistorial.Location = new Point(125, 365);
            buttonHistorial.Size = new Size(300, 45);
            buttonHistorial.Click += buttonHistorial_Click;

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver al menú";
            buttonVolver.Location = new Point(175, 440);
            buttonVolver.Size = new Size(200, 45);
            buttonVolver.Click += (sender, e) => Close();

            Controls.Add(labelTitulo);
            Controls.Add(labelCliente);
            Controls.Add(labelPeso);
            Controls.Add(labelEstatura);
            Controls.Add(labelIMC);
            Controls.Add(buttonRegistrar);
            Controls.Add(buttonHistorial);
            Controls.Add(buttonVolver);
            CargarMedidasActuales();
        }

        private void CargarMedidasActuales()
        {
            try
            {
                List<RegistroMedida> historial =
                    controlador.ObtenerHistorial(cliente.Id);

                double pesoActual = cliente.Peso;
                double estaturaActual = cliente.Estatura;
                double imcActual = cliente.CalcularIMC();

                if (historial.Count > 0)
                {
                    RegistroMedida ultimaMedida = historial[0];

                    pesoActual = ultimaMedida.Peso;
                    estaturaActual = ultimaMedida.Estatura;
                    imcActual = ultimaMedida.IMC;
                }

                labelPeso.Text =
                    "Peso actual: " + pesoActual.ToString("F2") + " kg";

                labelEstatura.Text =
                    "Estatura actual: " + estaturaActual.ToString("F2") + " m";

                labelIMC.Text =
                    "IMC actual: " + imcActual.ToString("F2");
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "No se pudieron cargar las medidas actuales."
                );
            }
        }

        private void buttonRegistrar_Click(object? sender, EventArgs e)
        {
            using (FormRegistrarMedida formulario =
                new FormRegistrarMedida(cliente))
            {
                formulario.ShowDialog();
            }

            CargarMedidasActuales();
        }

        private void buttonHistorial_Click(object? sender, EventArgs e)
        {
            using (FormHistorialMedidas formulario =
                new FormHistorialMedidas(cliente))
            {
                formulario.ShowDialog();
            }
        }
    }
}



