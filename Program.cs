using FitCR.Services;

namespace FitCR;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        try
        {
            ServicioDistribucionDatos servicioDatos =
                new ServicioDistribucionDatos();

            servicioDatos.PrepararDatos();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "No fue posible preparar los datos: " +
                ex.Message,
                "Fit.CR",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            return;
        }

        Application.Run(
            new FitCR.Views.Acceso.FormLogin());
    }
}