
namespace FitCR.Views.Acceso
{
    partial class FormRegistro
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            labelTitulo = new Label();
            labelNombre = new Label();
            textNombre = new TextBox();
            labelUsuario = new Label();
            textUsuario = new TextBox();
            labelEdad = new Label();
            textEdad = new TextBox();
            labelPeso = new Label();
            textPeso = new TextBox();
            labelEstatura = new Label();
            textEstatura = new TextBox();
            buttonRegistrar = new Button();
            SuspendLayout();
            // 
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.Location = new Point(319, 9);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(165, 25);
            labelTitulo.TabIndex = 0;
            labelTitulo.Text = "Registro de clientes";
            // 
            // labelNombre
            // 
            labelNombre.AutoSize = true;
            labelNombre.Location = new Point(13, 34);
            labelNombre.Name = "labelNombre";
            labelNombre.Size = new Size(159, 25);
            labelNombre.TabIndex = 1;
            labelNombre.Text = "Nombre completo";
            // 
            // textNombre
            // 
            textNombre.Location = new Point(12, 62);
            textNombre.Name = "textNombre";
            textNombre.Size = new Size(759, 31);
            textNombre.TabIndex = 2;
            // 
            // labelUsuario
            // 
            labelUsuario.AutoSize = true;
            labelUsuario.Location = new Point(12, 96);
            labelUsuario.Name = "labelUsuario";
            labelUsuario.Size = new Size(166, 25);
            labelUsuario.TabIndex = 3;
            labelUsuario.Text = "Nombre de usuario";
            // 
            // textUsuario
            // 
            textUsuario.Location = new Point(12, 124);
            textUsuario.Name = "textUsuario";
            textUsuario.Size = new Size(759, 31);
            textUsuario.TabIndex = 4;
            // 
            // labelEdad
            // 
            labelEdad.AutoSize = true;
            labelEdad.Location = new Point(11, 165);
            labelEdad.Name = "labelEdad";
            labelEdad.Size = new Size(52, 25);
            labelEdad.TabIndex = 5;
            labelEdad.Text = "Edad";
            // 
            // textEdad
            // 
            textEdad.Location = new Point(11, 193);
            textEdad.Name = "textEdad";
            textEdad.Size = new Size(760, 31);
            textEdad.TabIndex = 6;
            // 
            // labelPeso
            // 
            labelPeso.AutoSize = true;
            labelPeso.Location = new Point(11, 242);
            labelPeso.Name = "labelPeso";
            labelPeso.Size = new Size(84, 25);
            labelPeso.TabIndex = 7;
            labelPeso.Text = "Peso (kg)";
            // 
            // textPeso
            // 
            textPeso.Location = new Point(11, 270);
            textPeso.Name = "textPeso";
            textPeso.Size = new Size(760, 31);
            textPeso.TabIndex = 8;
            // 
            // labelEstatura
            // 
            labelEstatura.AutoSize = true;
            labelEstatura.Location = new Point(13, 318);
            labelEstatura.Name = "labelEstatura";
            labelEstatura.Size = new Size(106, 25);
            labelEstatura.TabIndex = 9;
            labelEstatura.Text = "Estatura (m)";
            // 
            // textEstatura
            // 
            textEstatura.Location = new Point(11, 346);
            textEstatura.Name = "textEstatura";
            textEstatura.Size = new Size(760, 31);
            textEstatura.TabIndex = 10;
            // 
            // buttonRegistrar
            // 
            buttonRegistrar.Location = new Point(329, 448);
            buttonRegistrar.Name = "buttonRegistrar";
            buttonRegistrar.Size = new Size(146, 34);
            buttonRegistrar.TabIndex = 11;
            buttonRegistrar.Text = "Registrarme";
            buttonRegistrar.UseVisualStyleBackColor = true;
            // 
            // FormRegistro
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(804, 494);
            Controls.Add(buttonRegistrar);
            Controls.Add(textEstatura);
            Controls.Add(labelEstatura);
            Controls.Add(textPeso);
            Controls.Add(labelPeso);
            Controls.Add(textEdad);
            Controls.Add(labelEdad);
            Controls.Add(textUsuario);
            Controls.Add(labelUsuario);
            Controls.Add(textNombre);
            Controls.Add(labelNombre);
            Controls.Add(labelTitulo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormRegistro";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fit.CR - Registro de clientes";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTitulo;
        private Label labelNombre;
        private TextBox textNombre;
        private Label labelUsuario;
        private TextBox textUsuario;
        private Label labelEdad;
        private TextBox textEdad;
        private Label labelPeso;
        private TextBox textPeso;
        private Label labelEstatura;
        private TextBox textEstatura;

        private Button buttonRegistrar;
    }
}

