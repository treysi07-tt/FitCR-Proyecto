
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FitCR.Interfaces;
using System.Text.Encodings.Web;

namespace FitCR.Repositories
{
    public class RepositorioJson<T> : IRepositorio<T>
    {
        private readonly string rutaArchivo;

        public RepositorioJson(string rutaRecibida)
        {
            rutaArchivo = rutaRecibida;
        }

        public List<T> ObtenerTodos()
        {
            if (!File.Exists(rutaArchivo))
            {
                return new List<T>();
            }

            string contenido = File.ReadAllText(rutaArchivo);

            List<T>? elementos = JsonSerializer.Deserialize<List<T>>(contenido);

            if (elementos == null)
            {
                return new List<T>();
            }
            else
            {
                return elementos;
            }
        }

        public void GuardarTodos(List<T> elementos)
        {
            JsonSerializerOptions opciones = new JsonSerializerOptions();

            opciones.WriteIndented = true;
            opciones.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            string contenido = JsonSerializer.Serialize(elementos, opciones);

            string? carpeta = Path.GetDirectoryName(rutaArchivo);

            if (!string.IsNullOrEmpty(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            File.WriteAllText(rutaArchivo, contenido);
        }
    }
}

