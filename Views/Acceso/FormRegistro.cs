using System.IO;
using FitCR.Controllers;
using FitCR.Models.Usuarios;
using FitCR.Repositories;
using FitCR.Services;

namespace FitCR.Views.Acceso
{
    public partial class FormRegistro : Form
    {
        private readonly ControladorClientes controlador;
        private Label labelContrasena;
        private TextBox textContrasena;

        private Label labelConfirmar;
        private TextBox textConfirmar;
        public FormRegistro()
        {
            InitializeComponent();

            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "clientes.json"
            );

            labelContrasena = new Label();
            labelContrasena.Text = "Contraseña";
            labelContrasena.Location = new Point(300, 62);
            labelContrasena.AutoSize = true;

            textContrasena = new TextBox();
            textContrasena.Location = new Point(300, 90);
            textContrasena.Size = new Size(200, 31);
            textContrasena.UseSystemPasswordChar = true;

            labelConfirmar = new Label();
            labelConfirmar.Text = "Confirmar contraseña";
            labelConfirmar.Location = new Point(300, 145);
            labelConfirmar.AutoSize = true;

            textConfirmar = new TextBox();
            textConfirmar.Location = new Point(300, 173);
            textConfirmar.Size = new Size(200, 31);
            textConfirmar.UseSystemPasswordChar = true;

            Controls.Add(labelContrasena);
            Controls.Add(textContrasena);
            Controls.Add(labelConfirmar);
            Controls.Add(textConfirmar);

            labelTitulo.Location = new Point(300, 25);

            labelNombre.Location = new Point(50, 90);
            textNombre.Location = new Point(50, 120);
            textNombre.Size = new Size(300, 31);

            labelUsuario.Location = new Point(50, 175);
            textUsuario.Location = new Point(50, 205);
            textUsuario.Size = new Size(300, 31);

            labelEdad.Location = new Point(50, 260);
            textEdad.Location = new Point(50, 290);
            textEdad.Size = new Size(300, 31);

            labelPeso.Location = new Point(50, 345);
            textPeso.Location = new Point(50, 375);
            textPeso.Size = new Size(300, 31);

            labelEstatura.Location = new Point(430, 90);
            textEstatura.Location = new Point(430, 120);
            textEstatura.Size = new Size(300, 31);

            labelContrasena.Location = new Point(430, 175);
            textContrasena.Location = new Point(430, 205);
            textContrasena.Size = new Size(300, 31);

            labelConfirmar.Location = new Point(430, 260);
            textConfirmar.Location = new Point(430, 290);
            textConfirmar.Size = new Size(300, 31);

            buttonRegistrar.Location = new Point(325, 445);
            buttonRegistrar.Size = new Size(150, 40);

            ClientSize = new Size(800, 520);


            RepositorioJson<Cliente> repositorio =
                new RepositorioJson<Cliente>(ruta);

            controlador = new ControladorClientes(repositorio);
            buttonRegistrar.Click += buttonRegistrar_Click;
        }

        private void buttonRegistrar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textContrasena.Text))
            {
                MessageBox.Show("Debe ingresar una contraseña.");
                return;
            }

            // Validamos
            if (textContrasena.Text.Length < 8)
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres.");
                return;
            }

            if (textContrasena.Text != textConfirmar.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden.");
                return;
            }

            if (!int.TryParse(textEdad.Text, out int edad))
            {
                MessageBox.Show("Ingrese una edad valida.");
                return;
            }

            if (!double.TryParse(textPeso.Text, out double peso))
            {
                MessageBox.Show("Ingrese un peso valido.");
                return;
            }

            if (!double.TryParse(textEstatura.Text, out double estatura))
            {
                MessageBox.Show("Ingrese una estatura valida.");
                return;
            }

            Cliente nuevoCliente = new Cliente();
            nuevoCliente.Nombre = textNombre.Text.Trim();
            nuevoCliente.NombreUsuario = textUsuario.Text.Trim();
            nuevoCliente.Edad = edad;
            nuevoCliente.Peso = peso;
            nuevoCliente.Estatura = estatura;
   
            FitCR.Services.Contrasenas servicio =
                new FitCR.Services.Contrasenas();

            nuevoCliente.ContrasenaHash =
                servicio.GenerarHash(textContrasena.Text);

            try
            {
                controlador.RegistrarCliente(nuevoCliente);

                MessageBox.Show(
                    "Cliente registrado correctamente.",
                    "Registro exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                this.Close();
                textNombre.Clear();
                textUsuario.Clear();
                textEdad.Clear();
                textPeso.Clear();
                textEstatura.Clear();
                textContrasena.Clear();
                textConfirmar.Clear();
            }
            catch (ArgumentException error)
            {
                MessageBox.Show(error.Message);
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "Ocurrio un error."
                );
            }
        }
    }
}


