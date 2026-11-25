using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FitCR.Controllers;
using FitCR.Models.Usuarios;
using FitCR.Repositories;
using FitCR.Views.Clientes;

namespace FitCR.Views.Acceso
{
    public partial class FormLogin : Form
    {
        private readonly ControladorAcceso controlador;

        private TextBox textUsuario = new TextBox();
        private TextBox textContrasena = new TextBox();

        public FormLogin()
        {
            InitializeComponent();

            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "clientes.json"
            );

            RepositorioJson<Cliente> repositorio =
                new RepositorioJson<Cliente>(ruta);

            controlador = new ControladorAcceso(repositorio);

            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            Controls.Clear();

            Text = "Inicio de sesión";
            Size = new Size(500, 500);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            Label labelTitulo = new Label();
            labelTitulo.Text = "Fit.CR";
            labelTitulo.Font = new Font(
                "Segoe UI", 26, FontStyle.Bold);
            labelTitulo.Location = new Point(50, 20);
            labelTitulo.Size = new Size(380, 60);
            labelTitulo.TextAlign =
                ContentAlignment.MiddleCenter;

            Label labelSubtitulo = new Label();
            labelSubtitulo.Text = "Inicio de sesión";
            labelSubtitulo.Font = new Font("Segoe UI", 16);
            labelSubtitulo.Location = new Point(50, 85);
            labelSubtitulo.Size = new Size(380, 40);
            labelSubtitulo.TextAlign =
                ContentAlignment.MiddleCenter;

            Label labelUsuario = new Label();
            labelUsuario.Text = "Nombre de usuario";
            labelUsuario.Font = new Font("Segoe UI", 11);
            labelUsuario.Location = new Point(90, 145);
            labelUsuario.Size = new Size(300, 25);

            textUsuario.Name = "textUsuario";
            textUsuario.Location = new Point(90, 175);
            textUsuario.Size = new Size(300, 30);
            textUsuario.Font = new Font("Segoe UI", 11);

            Label labelContrasena = new Label();
            labelContrasena.Text = "Contraseña";
            labelContrasena.Font = new Font("Segoe UI", 11);
            labelContrasena.Location = new Point(90, 220);
            labelContrasena.Size = new Size(300, 25);

            textContrasena.Name = "textContrasena";
            textContrasena.Location = new Point(90, 250);
            textContrasena.Size = new Size(300, 30);
            textContrasena.Font = new Font("Segoe UI", 11);
            textContrasena.UseSystemPasswordChar = true;

            Button buttonIngresar = new Button();
            buttonIngresar.Name = "buttonIngresar";
            buttonIngresar.Text = "Iniciar sesión";
            buttonIngresar.Location = new Point(90, 310);
            buttonIngresar.Size = new Size(300, 45);
            buttonIngresar.Font = new Font("Segoe UI", 12);
            buttonIngresar.Click += buttonIngresar_Click;

            Button buttonRegistrar = new Button();
            buttonRegistrar.Name = "buttonRegistrar";
            buttonRegistrar.Text = "Registrarse";
            buttonRegistrar.Location = new Point(90, 375);
            buttonRegistrar.Size = new Size(300, 45);
            buttonRegistrar.Font = new Font("Segoe UI", 12);
            buttonRegistrar.Click += buttonRegistrar_Click;

            Controls.Add(labelTitulo);
            Controls.Add(labelSubtitulo);
            Controls.Add(labelUsuario);
            Controls.Add(textUsuario);
            Controls.Add(labelContrasena);
            Controls.Add(textContrasena);
            Controls.Add(buttonIngresar);
            Controls.Add(buttonRegistrar);

            AcceptButton = buttonIngresar;
        }

        private void buttonIngresar_Click(
            object? sender, EventArgs e)
        {
            try
            {
                string usuario = textUsuario.Text.Trim();
                string contrasena = textContrasena.Text;

                if (string.IsNullOrWhiteSpace(usuario) ||
                    string.IsNullOrWhiteSpace(contrasena))
                {
                    MessageBox.Show(
                        "Debe ingresar el usuario y la contraseña.",
                        "Campos requeridos",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                Cliente? cliente = controlador.IniciarSesion(
                    usuario,
                    contrasena
                );

                if (cliente == null)
                {
                    MessageBox.Show(
                        "El usuario o la contraseña son incorrectos.",
                        "Error de inicio de sesión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                MessageBox.Show(
                    "Inicio de sesión correcto. Bienvenido/a, " +
                    cliente.Nombre,
                    "Inicio de sesión exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                this.Hide();

                try
                {
                    using (FormMenuCliente menu =
                        new FormMenuCliente(cliente))
                    {
                        menu.ShowDialog();
                    }
                }
                finally
                {
                    textUsuario.Clear();
                    textContrasena.Clear();
                    this.Show();
                }
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

        private void buttonRegistrar_Click(
            object? sender, EventArgs e)
        {
            this.Hide();

            using (FormRegistro formulario = new FormRegistro())
            {
                formulario.ShowDialog();
            }

            this.Show();
        }
    }
}
