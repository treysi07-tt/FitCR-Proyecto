using System;
using System.Collections.Generic;
using System.Linq;
using FitCR.Interfaces;
using FitCR.Models.Objetivos;

namespace FitCR.Controllers
{
    public class ControladorObjetivos
    {
        private readonly IRepositorio<Objetivo> repositorio;

        public ControladorObjetivos(
            IRepositorio<Objetivo> repositorioRecibido)
        {
            repositorio = repositorioRecibido;
        }

        public List<Objetivo> ObtenerObjetivos(int idCliente)
        {
            return repositorio.ObtenerTodos()
                .Where(o => o.IdCliente == idCliente)
                .OrderByDescending(o => o.FechaCreacion)
                .ToList();
        }

        public void RegistrarObjetivo(Objetivo nuevoObjetivo)
        {
            ValidarObjetivo(nuevoObjetivo);

            List<Objetivo> objetivos =
                repositorio.ObtenerTodos();

            int nuevoId = 1;

            foreach (Objetivo objetivo in objetivos)
            {
                if (objetivo.Id >= nuevoId)
                {
                    nuevoId = objetivo.Id + 1;
                }
            }

            nuevoObjetivo.Id = nuevoId;
            nuevoObjetivo.FechaCreacion = DateTime.Now;
            nuevoObjetivo.Completado = false;

            objetivos.Add(nuevoObjetivo);
            repositorio.GuardarTodos(objetivos);
        }

        public void EditarObjetivo(Objetivo objetivoEditado)
        {
            ValidarObjetivo(objetivoEditado);

            List<Objetivo> objetivos =
                repositorio.ObtenerTodos();

            Objetivo? existente = objetivos.FirstOrDefault(o =>
                o.Id == objetivoEditado.Id &&
                o.IdCliente == objetivoEditado.IdCliente
            );

            if (existente == null)
            {
                throw new ArgumentException(
                    "No se encontró el objetivo. Intente de nuevo"
                );
            }

            existente.Nombre = objetivoEditado.Nombre;
            existente.Descripcion = objetivoEditado.Descripcion;
            existente.TipoObjetivo = objetivoEditado.TipoObjetivo;
            existente.ValorMeta = objetivoEditado.ValorMeta;
            existente.FechaLimite = objetivoEditado.FechaLimite;
            repositorio.GuardarTodos(objetivos);
        }

        public void EliminarObjetivo(
            int idObjetivo, int idCliente)
        {
            List<Objetivo> objetivos =
                repositorio.ObtenerTodos();

            Objetivo? existente = objetivos.FirstOrDefault(o =>
                o.Id == idObjetivo &&
                o.IdCliente == idCliente
            );

            if (existente == null)
            {
                throw new ArgumentException(
                    "No se encontró el objetivo."
                );
            }

            objetivos.Remove(existente);
            repositorio.GuardarTodos(objetivos);
        }

        public void CompletarObjetivo(
            int idObjetivo, int idCliente)
        {
            List<Objetivo> objetivos =
                repositorio.ObtenerTodos();

            Objetivo? existente = objetivos.FirstOrDefault(o =>
                o.Id == idObjetivo &&
                o.IdCliente == idCliente
            );

            if (existente == null)
            {
                throw new ArgumentException(
                    "No se encontró el objetivo."
                );
            }

            if (existente.Completado)
            {
                throw new ArgumentException(
                    "Este objetivo ya está completado."
                );
            }

            existente.Completado = true;
            repositorio.GuardarTodos(objetivos);
        }

        public int ContarObjetivosCompletados(int idCliente)
        {
            return ObtenerObjetivos(idCliente)
                .Count(o => o.Completado);
        }

        // Validaciones
        private void ValidarObjetivo(Objetivo objetivo)
        {
            if (objetivo.IdCliente <= 0)
            {
                throw new ArgumentException(
                    "El cliente no es válido."
                );
            }

            if (string.IsNullOrWhiteSpace(objetivo.Nombre))
            {
                throw new ArgumentException(
                    "El nombre del objetivo es obligatorio."
                );
            }

            if (string.IsNullOrWhiteSpace(objetivo.TipoObjetivo))
            {
                throw new ArgumentException(
                    "Debes seleccionar un tipo de objetivo."
                );
            }

            if (objetivo.ValorMeta <= 0 ||
                double.IsNaN(objetivo.ValorMeta) ||
                double.IsInfinity(objetivo.ValorMeta))
            {
                throw new ArgumentException(
                    "El valor de la meta debe ser mayor que cero."
                );
            }

            if (objetivo.FechaLimite == DateTime.MinValue)
            {
                throw new ArgumentException(
                    "Debes seleccionar una fecha límite."
                );
            }
        }
    }
}
