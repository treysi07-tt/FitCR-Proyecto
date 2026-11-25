using System;
using System.IO;

namespace FitCR.Services
{
    public class ServicioDistribucionDatos
    {
        public void PrepararDatos()
        {
            string rutaInicial = Path.Combine(
                AppContext.BaseDirectory,
                "DatosIniciales");

            string rutaDestino = Path.Combine(
                AppContext.BaseDirectory,
                "Data");

            if (!Directory.Exists(rutaInicial))
            {
                throw new DirectoryNotFoundException(
                    "No se encontraron los datos iniciales de Fit.CR.");
            }

            string[] archivos = Directory.GetFiles(
                rutaInicial, "*.json");

            if (archivos.Length != 13)
            {
                throw new InvalidOperationException(
                    "Los datos iniciales están incompletos.");
            }

            Directory.CreateDirectory(rutaDestino);

            if (Directory.GetFiles(
                rutaDestino, "*.json").Length > 0)
            {
                return;
            }

            foreach (string archivo in archivos)
            {
                string nombreArchivo =
                    Path.GetFileName(archivo);

                string destino = Path.Combine(
                    rutaDestino, nombreArchivo);

                File.Copy(archivo, destino, false);
            }
        }
    }
}