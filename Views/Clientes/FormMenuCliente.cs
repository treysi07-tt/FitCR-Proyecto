using System;
using System.Drawing;
using System.Windows.Forms;
using FitCR.Models.Usuarios;


namespace FitCR.Views.Clientes
{
    public partial class FormMenuCliente : Form
    {
        private void buttonRutinas_Click(object? sender, EventArgs e)
        {
            using (FormRutinas formulario = new FormRutinas(cliente))
            {
                formulario.ShowDialog();
            }
        }

        private readonly Cliente cliente;
        public FormMenuCliente(Cliente clienteRecibido)
        {
            InitializeComponent();
            cliente = clienteRecibido;
            Text = "Fit.CR - Menu principal";
            ClientSize = new Size(650, 850);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            AutoScroll = true;

            Label labelBienvenida = new Label();
            labelBienvenida.Text = "Bienvenido a Fit.CR " + cliente.Nombre;
            labelBienvenida.Location = new Point(60, 30);
            labelBienvenida.Size = new Size(530, 45);
            labelBienvenida.Font = new Font(
                "Segoe UI", 16, FontStyle.Bold
            );
            labelBienvenida.TextAlign = ContentAlignment.MiddleCenter;

            Label labelMenu = new Label();
            labelMenu.Text = "Menu principal";
            labelMenu.Location = new Point(175, 90);
            labelMenu.AutoSize = true;

            Button buttonRutinas = new Button();
            buttonRutinas.Text = "Mis rutinas";
            buttonRutinas.Location = new Point(100, 150);
            buttonRutinas.Size = new Size(200, 65);
            buttonRutinas.Click += buttonRutinas_Click;

            Button buttonMedidas = new Button();
            buttonMedidas.Text = "Mis medidas";
            buttonMedidas.Location = new Point(350, 150);
            buttonMedidas.Size = new Size(200, 65);


            buttonMedidas.Click += (sender, e) =>
            {
                using (FormMedidas medidas = new FormMedidas(cliente))
                {
                    medidas.ShowDialog();
                }
            };


            Button buttonObjetivos = new Button();
            buttonObjetivos.Text = "Mis objetivos";
            buttonObjetivos.Location = new Point(100, 250);
            buttonObjetivos.Size = new Size(200, 65);
            buttonObjetivos.Click += buttonObjetivos_Click;

            Button buttonProgreso = new Button();
            buttonProgreso.Text = "Mi progreso";
            buttonProgreso.Location = new Point(350, 250);
            buttonProgreso.Size = new Size(200, 65);
            buttonProgreso.Click += buttonProgreso_Click;

            Button buttonCerrarSesion = new Button();
            buttonCerrarSesion.Text = "Cerrar sesión";
            buttonCerrarSesion.Location = new Point(225, 700);
            buttonCerrarSesion.Size = new Size(200, 45);

            Button buttonInvitados = new Button();
            buttonInvitados.Text = "Mis invitados";
            buttonInvitados.Location = new Point(175, 4100);
            buttonInvitados.Size = new Size(300, 35);
            buttonInvitados.Click += buttonInvitados_Click;

            Button buttonVisitas = new Button();
            buttonVisitas.Text = "Mis visitas al gimnasio";
            buttonVisitas.Location = new Point(175, 475);
            buttonVisitas.Size = new Size(300, 45);
            buttonVisitas.Click += buttonVisitas_Click;

            Button buttonInsignias = new Button();
            buttonInsignias.Text = "Mis insignias";
            buttonInsignias.Size = new Size(300, 45);
            buttonInsignias.Location = new Point(175, 540);
            buttonInsignias.Font = new Font("Segoe UI", 12);
            buttonInsignias.Click += buttonInsignias_Click;

            Button buttonNotificaciones = new Button();

            buttonNotificaciones.Name = "buttonNotificaciones";
            buttonNotificaciones.Text = "Notificaciones";
            buttonNotificaciones.Location = new Point(175, 605);
            buttonNotificaciones.Size = new Size(300, 45);
            buttonNotificaciones.Font = new Font("Segoe UI", 12);

            buttonNotificaciones.Click += buttonNotificaciones_Click;

            Controls.Add(labelBienvenida);
            Controls.Add(labelMenu);
            Controls.Add(buttonRutinas);
            Controls.Add(buttonMedidas);
            Controls.Add(buttonObjetivos);
            Controls.Add(buttonProgreso);
            Controls.Add(buttonCerrarSesion);
            Controls.Add(buttonInvitados);
            Controls.Add(buttonVisitas);
            Controls.Add(buttonInsignias);
            Controls.Add(buttonNotificaciones);

            buttonCerrarSesion.Click += (sender, e) =>
            {
                Close();
            };
        }

        private void buttonNotificaciones_Click(
            object? sender, EventArgs e)
        {
            using (FormNotificaciones formulario =
                new FormNotificaciones(cliente))
            {
                formulario.ShowDialog();
            }
        }

        private void buttonInsignias_Click(
            object? sender, EventArgs e)
        {
            using (FormInsignias formulario =
                new FormInsignias(cliente))
            {
                formulario.ShowDialog();
            }
        }

        private void buttonProgreso_Click(object? sender, EventArgs e)
        {
            using (FormProgreso formulario =
                new FormProgreso(cliente))
            {
                formulario.ShowDialog();
            }
        }

        private void buttonObjetivos_Click(object? sender, EventArgs e)
        {
            using (FormObjetivos formulario =
                new FormObjetivos(cliente))
            {
                formulario.ShowDialog();
            }
        }

        private void buttonVisitas_Click(object? sender, EventArgs e)
        {
            using (FormVisitas formulario =
                new FormVisitas(cliente))
            {
                formulario.ShowDialog();
            }
        }

        private void buttonInvitados_Click(object? sender, EventArgs e)
        {
            using (FormInvitados formulario =
                new FormInvitados(cliente))
            {
                formulario.ShowDialog();
            }
        }


    }
}

