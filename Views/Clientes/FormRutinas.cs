using System;
using System.Drawing;
using System.Windows.Forms;
using FitCR.Models.Usuarios;

namespace FitCR.Views.Clientes
{
    public partial class FormRutinas : Form
    {
        private readonly Cliente cliente;
        public FormRutinas(Cliente clienteRecibido)
        {
            InitializeComponent();
            cliente = clienteRecibido;

            Text = "Fit.CR - Mis rutinas";
            ClientSize = new Size(550, 550);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Mis rutinas";
            labelTitulo.Location = new Point(190, 25);
            labelTitulo.Size = new Size(250, 45);
            labelTitulo.Font = new Font(
                "Segoe UI", 18, FontStyle.Bold
            );

            Label labelCliente = new Label();
            labelCliente.Text = "Cliente: " + cliente.Nombre;
            labelCliente.Location = new Point(65, 95);
            labelCliente.Size = new Size(420, 30);

            Button buttonAgregar = new Button();
            buttonAgregar.Text = "Agregar nueva rutina";
            buttonAgregar.Location = new Point(125, 155);
            buttonAgregar.Size = new Size(300, 45);
            buttonAgregar.Click += buttonAgregar_Click;

            Button buttonVer = new Button();
            buttonVer.Text = "Ver mis rutinas";
            buttonVer.Location = new Point(125, 220);
            buttonVer.Size = new Size(300, 45);
            buttonVer.Click += buttonVer_Click;

            Button buttonAdministrar = new Button();
            buttonAdministrar.Text = "Editar o eliminar rutinas";
            buttonAdministrar.Location = new Point(125, 285);
            buttonAdministrar.Size = new Size(300, 45);
            buttonAdministrar.Click += buttonAdministrar_Click;

            Button buttonUltimas = new Button();
            buttonUltimas.Text = "Últimas 5 rutinas";
            buttonUltimas.Location = new Point(125, 350);
            buttonUltimas.Size = new Size(300, 45);
            buttonUltimas.Click += buttonUltimas_Click;


            Button buttonVolver = new Button();
            buttonVolver.Text = "Volver al menú";
            buttonVolver.Location = new Point(175, 445);
            buttonVolver.Size = new Size(200, 45);
            buttonVolver.Click += (sender, e) => Close();

            Controls.Add(labelTitulo);
            Controls.Add(labelCliente);
            Controls.Add(buttonAgregar);
            Controls.Add(buttonVer);
            Controls.Add(buttonAdministrar);
            Controls.Add(buttonUltimas);
            Controls.Add(buttonVolver);
        }

        private void buttonAgregar_Click(object? sender, EventArgs e)
        {
            using (FormAgregarRutina formulario =
                new FormAgregarRutina(cliente))
            {
                formulario.ShowDialog();
            }
        }

        private void buttonVer_Click(object? sender, EventArgs e)
        {
            using (FormVerRutinas formulario =
                new FormVerRutinas(cliente))
            {
                formulario.ShowDialog();
            }
        }


        private void buttonAdministrar_Click(object? sender, EventArgs e)
        {
            using (FormEditarRutina formulario =
                new FormEditarRutina(cliente))
            {
                formulario.ShowDialog();
            }
        }

        private void buttonUltimas_Click(object? sender, EventArgs e)
        {
            using (FormVerRutinas formulario =
                new FormVerRutinas(cliente, true))
            {
                formulario.ShowDialog();
            }
        }

    }
}

