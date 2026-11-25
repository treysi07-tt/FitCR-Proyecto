using System;
using System.Collections.Generic;
using System.Linq;
using FitCR.Interfaces;
using FitCR.Models.Rutinas;

namespace FitCR.Controllers
{
    public class ControladorRutinas
    {
        private readonly IRepositorio<Rutina> repositorio;

        public ControladorRutinas(
            IRepositorio<Rutina> repositorioRecibido)
        {
            repositorio = repositorioRecibido;
        }

        public List<Rutina> ObtenerRutinas(int idCliente)
        {
            return repositorio.ObtenerTodos()
                .Where(r => r.IdCliente == idCliente)
                .OrderByDescending(r => r.FechaCreacion)
                .ToList();
        }

        public void RegistrarRutina(Rutina nuevaRutina)
        {
            ValidarRutina(nuevaRutina);

            List<Rutina> rutinas = repositorio.ObtenerTodos();

            int nuevoId = 1;

            foreach (Rutina rutina in rutinas)
            {
                if (rutina.Id >= nuevoId)
                {
                    nuevoId = rutina.Id + 1;
                }
            }

            nuevaRutina.Id = nuevoId;
            nuevaRutina.FechaCreacion = DateTime.Now;
            nuevaRutina.Completada = false;

            rutinas.Add(nuevaRutina);

            repositorio.GuardarTodos(rutinas);
        }

        public void EditarRutina(Rutina rutinaEditada)
        {
            ValidarRutina(rutinaEditada);

            List<Rutina> rutinas = repositorio.ObtenerTodos();

            Rutina? rutinaExistente = rutinas.FirstOrDefault(
                r => r.Id == rutinaEditada.Id &&
                     r.IdCliente == rutinaEditada.IdCliente
            );

            if (rutinaExistente == null)
            {
                throw new ArgumentException(
                    "No se encontró la rutina. Intente de nuevo"
                );
            }

            rutinaExistente.Nombre = rutinaEditada.Nombre;
            rutinaExistente.Descripcion = rutinaEditada.Descripcion;
            rutinaExistente.TipoEjercicio = rutinaEditada.TipoEjercicio;
            rutinaExistente.Duracion = rutinaEditada.Duracion;
            rutinaExistente.Series = rutinaEditada.Series;
            rutinaExistente.Repeticiones = rutinaEditada.Repeticiones;

            rutinaExistente.FechaVencimiento =
                rutinaEditada.FechaVencimiento;

            repositorio.GuardarTodos(rutinas);
        }

        public void EliminarRutina(int idRutina, int idCliente)
        {
            List<Rutina> rutinas = repositorio.ObtenerTodos();

            Rutina? rutina = rutinas.FirstOrDefault(
                r => r.Id == idRutina &&
                     r.IdCliente == idCliente
            );

            if (rutina == null)
            {
                throw new ArgumentException(
                    "No se encontró la rutina. Intente de nuevo"
                );
            }

            rutinas.Remove(rutina);

            repositorio.GuardarTodos(rutinas);
        }

        public void CompletarRutina(int idRutina, int idCliente)
        {
            List<Rutina> rutinas = repositorio.ObtenerTodos();

            Rutina? rutina = rutinas.FirstOrDefault(
                r => r.Id == idRutina &&
                     r.IdCliente == idCliente
            );

            if (rutina == null)
            {
                throw new ArgumentException(
                    "No se encontró la rutina. Intente de nuevo"
                );
            }

            if (rutina.Completada)
            {
                throw new ArgumentException(
                    "Esta rutina ya fue completada."
                );
            }

            rutina.Completada = true;
            rutina.FechaCompletada = DateTime.Now;

            repositorio.GuardarTodos(rutinas);
        }

        public List<Rutina> ObtenerUltimasCinco(int idCliente)
        {
            return ObtenerRutinas(idCliente)
                .Take(5)
                .ToList();
        }

        private void ValidarRutina(Rutina rutina)
        {
            if (rutina.IdCliente <= 0)
            {
                throw new ArgumentException(
                    "El cliente no es válido. Intente de nuevo"
                );
            }

            if (string.IsNullOrWhiteSpace(rutina.Nombre))
            {
                throw new ArgumentException(
                    "El nombre de la rutina es obligatorio."
                );
            }

            if (string.IsNullOrWhiteSpace(rutina.TipoEjercicio))
            {
                throw new ArgumentException(
                    "Debe indicar el tipo de ejercicio."
                );
            }

            if (rutina.Duracion <= 0)
            {
                throw new ArgumentException(
                    "La duración debe ser mayor que cero."
                );
            }

            if (rutina.Series <= 0)
            {
                throw new ArgumentException(
                    "Las series deben ser mayores que cero."
                );
            }

            if (rutina.Repeticiones <= 0)
            {
                throw new ArgumentException(
                    "Las repeticiones deben ser mayores que cero."
                );
            }

            if (rutina.FechaVencimiento == DateTime.MinValue)
            {
                throw new ArgumentException(
                    "Debe indicar una fecha de vencimiento."
                );
            }
        }
    }
}