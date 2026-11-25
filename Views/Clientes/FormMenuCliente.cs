using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FitCR.Models.Usuarios;
using FitCR.Models.Notificaciones;
using FitCR.Controllers;
using FitCR.Repositories;
using FitCR.Services;

namespace FitCR.Views.Clientes
{
    public partial class FormMenuCliente : Form
    {
        private readonly Cliente cliente;
        private readonly ServicioNotificaciones servicioNotificaciones;
        private readonly ControladorNotificaciones controladorNotificaciones;
        private readonly Button botonNotificaciones;

        public FormMenuCliente(Cliente clienteRecibido)
        {
            InitializeComponent();

            cliente = clienteRecibido;

            servicioNotificaciones =
                new ServicioNotificaciones();

            string rutaData = Path.Combine(
                AppContext.BaseDirectory,
                "Data"
            );

            controladorNotificaciones =
                new ControladorNotificaciones(
                    new RepositorioJson<Notificacion>(
                        Path.Combine(
                            rutaData,
                            "notificaciones.json"
                        )
                    )
                );

            Text = "Fit.CR - Menu principal";
            ClientSize = new Size(650, 920);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            AutoScroll = true;

            Label labelBienvenida = new Label();
            labelBienvenida.Text =
                "Bienvenido a Fit.CR " + cliente.Nombre;

            labelBienvenida.Location = new Point(60, 30);
            labelBienvenida.Size = new Size(530, 45);
            labelBienvenida.Font = new Font(
                "Segoe UI", 16, FontStyle.Bold
            );

            labelBienvenida.TextAlign =
                ContentAlignment.MiddleCenter;

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
            buttonMedidas.Click += buttonMedidas_Click;

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
            buttonCerrarSesion.Location = new Point(225, 780);
            buttonCerrarSesion.Size = new Size(200, 45);

            Button buttonInvitados = new Button();
            buttonInvitados.Text = "Mis invitados";
            buttonInvitados.Location = new Point(175, 410);
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

            botonNotificaciones = new Button();
            botonNotificaciones.Name = "buttonNotificaciones";
            botonNotificaciones.Text = "Notificaciones";
            botonNotificaciones.Location = new Point(175, 605);
            botonNotificaciones.Size = new Size(300, 45);
            botonNotificaciones.Font = new Font("Segoe UI", 12);
            botonNotificaciones.Click += buttonNotificaciones_Click;

            Button buttonClases = new Button();
            buttonClases.Name = "buttonClases";
            buttonClases.Text = "Clases del gimnasio";
            buttonClases.Location = new Point(175, 665);
            buttonClases.Size = new Size(300, 50);
            buttonClases.Font = new Font("Segoe UI", 11);
            buttonClases.Click += buttonClases_Click;

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
            Controls.Add(botonNotificaciones);
            Controls.Add(buttonClases);

            buttonCerrarSesion.Click += (sender, e) =>
            {
                Close();
            };

            Shown += (sender, e) =>
            {
                SincronizarMenu();
            };
        }

        private void SincronizarMenu()
        {
            try
            {
                servicioNotificaciones
                    .SincronizarNotificaciones(cliente);

                ActualizarContadorNotificaciones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No fue posible actualizar las notificaciones. " +
                    ex.Message,
                    "Fit.CR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void ActualizarContadorNotificaciones()
        {
            int cantidad = controladorNotificaciones
                .ContarNotificacionesSinLeer(cliente.Id);

            if (cantidad > 0)
            {
                botonNotificaciones.Text =
                    "Notificaciones (" + cantidad + ")";

                botonNotificaciones.Font = new Font(
                    "Segoe UI", 12, FontStyle.Bold
                );
            }
            else
            {
                botonNotificaciones.Text = "Notificaciones";

                botonNotificaciones.Font = new Font(
                    "Segoe UI", 12, FontStyle.Regular
                );
            }
        }

        private void AbrirFormulario(Form formulario)
        {
            using (formulario)
            {
                formulario.ShowDialog(this);
            }

            SincronizarMenu();
        }

        private void buttonRutinas_Click(
            object? sender, EventArgs e)
        {
            AbrirFormulario(
                new FormRutinas(cliente)
            );
        }

        private void buttonMedidas_Click(
            object? sender, EventArgs e)
        {
            AbrirFormulario(
                new FormMedidas(cliente)
            );
        }

        private void buttonObjetivos_Click(
            object? sender, EventArgs e)
        {
            AbrirFormulario(
                new FormObjetivos(cliente)
            );
        }

        private void buttonProgreso_Click(
            object? sender, EventArgs e)
        {
            AbrirFormulario(
                new FormProgreso(cliente)
            );
        }

        private void buttonInvitados_Click(
            object? sender, EventArgs e)
        {
            AbrirFormulario(
                new FormInvitados(cliente)
            );
        }

        private void buttonVisitas_Click(
            object? sender, EventArgs e)
        {
            AbrirFormulario(
                new FormVisitas(cliente)
            );
        }

        private void buttonInsignias_Click(
            object? sender, EventArgs e)
        {
            AbrirFormulario(
                new FormInsignias(cliente)
            );
        }

        private void buttonNotificaciones_Click(
            object? sender, EventArgs e)
        {
            AbrirFormulario(
                new FormNotificaciones(cliente)
            );
        }

        private void buttonClases_Click(
            object? sender, EventArgs e)
        {
            AbrirFormulario(
                new FormClasesCliente(cliente)
            );
        }
    }
}