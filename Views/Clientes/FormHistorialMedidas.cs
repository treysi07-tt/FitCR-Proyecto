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
    public partial class FormHistorialMedidas : Form
    {
        private readonly Cliente cliente;
        private readonly ControladorMedidas controlador;
        private DataGridView tablaHistorial;
        public FormHistorialMedidas(Cliente clienteRecibido)
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

            Text = "Historial de medidas";
            ClientSize = new Size(850, 500);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Historial de medidas";
            labelTitulo.Location = new Point(260, 25);
            labelTitulo.Size = new Size(400, 45);
            labelTitulo.Font = new Font(
                "Segoe UI", 16, FontStyle.Bold
            );

            Label labelCliente = new Label();
            labelCliente.Text = "Cliente: " + cliente.Nombre;
            labelCliente.Location = new Point(50, 95);
            labelCliente.Size = new Size(700, 30);

            tablaHistorial = new DataGridView();
            tablaHistorial.Location = new Point(50, 145);
            tablaHistorial.Size = new Size(750, 250);
            tablaHistorial.ReadOnly = true;
            tablaHistorial.AllowUserToAddRows = false;
            tablaHistorial.AllowUserToDeleteRows = false;
            tablaHistorial.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            tablaHistorial.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver a mis medidas";
            buttonVolver.Location = new Point(310, 420);
            buttonVolver.Size = new Size(230, 45);
            buttonVolver.Click += (sender, e) => Close();

            Controls.Add(labelTitulo);
            Controls.Add(labelCliente);
            Controls.Add(tablaHistorial);
            Controls.Add(buttonVolver);
            CargarHistorial();
        }


        private void CargarHistorial()
        {
            try
            {
                List<RegistroMedida> historial =
                    controlador.ObtenerHistorial(cliente.Id);

                tablaHistorial.DataSource = null;
                tablaHistorial.DataSource = historial;
                DataGridViewColumn? columnaId =
                    tablaHistorial.Columns["Id"];

                if (columnaId != null)
                {
                    columnaId.Visible = false;
                }

                DataGridViewColumn? columnaCliente =
                    tablaHistorial.Columns["IdCliente"];

                if (columnaCliente != null)
                {
                    columnaCliente.Visible = false;
                }

                DataGridViewColumn? columnaFecha =
                    tablaHistorial.Columns["Fecha"];

                if (columnaFecha != null)
                {
                    columnaFecha.HeaderText = "Fecha";
                    columnaFecha.DefaultCellStyle.Format =
                        "dd/MM/yyyy HH:mm";
                }

                DataGridViewColumn? columnaPeso =
                    tablaHistorial.Columns["Peso"];

                if (columnaPeso != null)
                {
                    columnaPeso.HeaderText = "Peso (kg)";
                }

                DataGridViewColumn? columnaEstatura =
                    tablaHistorial.Columns["Estatura"];

                if (columnaEstatura != null)
                {
                    columnaEstatura.HeaderText = "Estatura (m)";
                }

                DataGridViewColumn? columnaIMC =
                    tablaHistorial.Columns["IMC"];

                if (columnaIMC != null)
                {
                    columnaIMC.HeaderText = "IMC";
                }
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "No se pudo cargar el historial de medidas."
                );
            }
        }

    }
}

