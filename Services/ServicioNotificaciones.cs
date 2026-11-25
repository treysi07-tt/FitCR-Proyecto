using System;
using System.IO;
using System.Linq;
using FitCR.Controllers;
using FitCR.Repositories;
using FitCR.Models.Usuarios;
using FitCR.Models.Notificaciones;
using FitCR.Models.Rutinas;
using FitCR.Models.Medidas;
using FitCR.Models.Visitas;
using FitCR.Models.Objetivos;
using FitCR.Models.Insignias;

namespace FitCR.Services
{
    public class ServicioNotificaciones
    {
        private readonly string rutaData;
        private readonly ControladorNotificaciones controlador;

        public ServicioNotificaciones()
        {
            rutaData = Path.Combine(
                AppContext.BaseDirectory, "Data");

            RepositorioJson<Notificacion> repositorio =
                new RepositorioJson<Notificacion>(
                    Path.Combine(
                        rutaData, "notificaciones.json"));

            controlador =
                new ControladorNotificaciones(repositorio);
        }

        public int SincronizarNotificaciones(Cliente cliente)
        {
            int cantidad = 0;
            cantidad += NotificarBienvenida(cliente);
            cantidad += NotificarRutinas(cliente.Id);
            cantidad += NotificarVisitas(cliente.Id);
            cantidad += NotificarInvitados(cliente.Id);
            cantidad += NotificarMedidas(cliente.Id);
            cantidad += NotificarObjetivos(cliente.Id);
            cantidad += NotificarInsignias(cliente.Id);

            return cantidad;
        }

        private int NotificarBienvenida(Cliente cliente)
        {
            bool creada = controlador.RegistrarNotificacion(
                cliente.Id,
                "Bienvenido a Fit.CR",
                "Hola, " + cliente.Nombre +
                ". Te damos la bienvenida a Fit.CR.",
                "Acceso",
                "bienvenida_" + cliente.Id);

            return creada ? 1 : 0;
        }

        private int NotificarRutinas(int idCliente)
        {
            var repositorio = new RepositorioJson<Rutina>(
                Path.Combine(rutaData, "rutinas.json"));

            var rutinas = repositorio.ObtenerTodos()
                .Where(r => r.IdCliente == idCliente);

            int cantidad = 0;

            foreach (var rutina in rutinas)
            {
                if (controlador.RegistrarNotificacion(
                    idCliente,
                    "Nueva rutina registrada",
                    "Has registrado la rutina: " +
                    rutina.Nombre + ".",
                    "Rutinas",
                    "rutina_registrada_" + rutina.Id,
                    rutina.FechaCreacion))
                {
                    cantidad++;
                }

                if (rutina.Completada)
                {
                    if (controlador.RegistrarNotificacion(
                        idCliente,
                        "Rutina completada",
                        "Has completado la rutina: " +
                        rutina.Nombre + ".",
                        "Rutinas",
                        "rutina_completada_" + rutina.Id,
                        rutina.FechaCompletada))
                    {
                        cantidad++;
                    }
                }
            }

            return cantidad;
        }

        private int NotificarVisitas(int idCliente)
        {
            var repositorio =
                new RepositorioJson<RegistroVisita>(
                    Path.Combine(rutaData, "visitas.json"));

            var visitas = repositorio.ObtenerTodos()
                .Where(v => v.IdCliente == idCliente);

            int cantidad = 0;

            foreach (var visita in visitas)
            {
                if (controlador.RegistrarNotificacion(
                    idCliente,
                    "Visita registrada",
                    "Has registrado tu asistencia al gimnasio.",
                    "Visitas",
                    "visita_" + visita.Id,
                    visita.FechaVisita))
                {
                    cantidad++;
                }
            }

            return cantidad;
        }

        private int NotificarInvitados(int idCliente)
        {
            var repositorio =
                new RepositorioJson<RegistroInvitado>(
                    Path.Combine(rutaData, "invitados.json"));

            var invitados = repositorio.ObtenerTodos()
                .Where(i => i.IdCliente == idCliente);

            int cantidad = 0;

            foreach (var invitado in invitados)
            {
                if (controlador.RegistrarNotificacion(
                    idCliente,
                    "Invitado registrado",
                    "Has registrado una visita para " +
                    invitado.NombreInvitado + ".",
                    "Invitados",
                    "invitado_" + invitado.Id,
                    invitado.FechaVisita))
                {
                    cantidad++;
                }
            }

            return cantidad;
        }

        private int NotificarMedidas(int idCliente)
        {
            var repositorio =
                new RepositorioJson<RegistroMedida>(
                    Path.Combine(rutaData, "medidas.json"));

            var medidas = repositorio.ObtenerTodos()
                .Where(m => m.IdCliente == idCliente);

            int cantidad = 0;

            foreach (var medida in medidas)
            {
                if (controlador.RegistrarNotificacion(
                    idCliente,
                    "Medidas actualizadas",
                    "Has registrado tus medidas corporales. " +
                    "Peso registrado: " + medida.Peso + " kg.",
                    "Medidas",
                    "medida_" + medida.Id,
                    medida.Fecha))
                {
                    cantidad++;
                }
            }

            return cantidad;
        }

        private int NotificarObjetivos(int idCliente)
        {
            var repositorio =
                new RepositorioJson<Objetivo>(
                    Path.Combine(rutaData, "objetivos.json"));

            var objetivos = repositorio.ObtenerTodos()
                .Where(o => o.IdCliente == idCliente);

            int cantidad = 0;

            foreach (var objetivo in objetivos)
            {
                if (controlador.RegistrarNotificacion(
                    idCliente,
                    "Nuevo objetivo registrado",
                    "Has establecido el objetivo: " +
                    objetivo.Nombre + ".",
                    "Objetivos",
                    "objetivo_registrado_" + objetivo.Id,
                    objetivo.FechaCreacion))
                {
                    cantidad++;
                }

                if (objetivo.Completado)
                {
                    if (controlador.RegistrarNotificacion(
                        idCliente,
                        "Objetivo alcanzado",
                        "Felicidades, has alcanzado tu objetivo: " +
                        objetivo.Nombre + ".",
                        "Objetivos",
                        "objetivo_completado_" + objetivo.Id))
                    {
                        cantidad++;
                    }
                }
                else if (DateTime.Today >
                    objetivo.FechaLimite.Date)
                {
                    if (controlador.RegistrarNotificacion(
                        idCliente,
                        "Objetivo vencido",
                        "El objetivo " + objetivo.Nombre +
                        " ha superado su fecha límite.",
                        "Objetivos",
                        "objetivo_vencido_" + objetivo.Id +
                        "_" + objetivo.FechaLimite.ToString("yyyyMMdd"),
                        objetivo.FechaLimite.Date.AddDays(1)))
                    {
                        cantidad++;
                    }
                }
            }

            return cantidad;
        }

        private int NotificarInsignias(int idCliente)
        {
            var repositorio =
                new RepositorioJson<InsigniaObtenida>(
                    Path.Combine(
                        rutaData, "insignias_obtenidas.json"));

            var obtenidas = repositorio.ObtenerTodos()
                .Where(i => i.IdCliente == idCliente);

            var catalogo = CatalogoInsignias.ObtenerInsignias();

            int cantidad = 0;

            foreach (var obtenida in obtenidas)
            {
                var insignia = catalogo.FirstOrDefault(i =>
                    i.Id == obtenida.IdInsignia);

                if (insignia == null)
                {
                    continue;
                }

                string titulo = insignia.EsNegativa
                    ? "Observación personal: " + insignia.Nombre
                    : "Nueva insignia: " + insignia.Nombre;

                string mensaje =
                    string.IsNullOrWhiteSpace(insignia.Mensaje)
                    ? insignia.Descripcion
                    : insignia.Mensaje;

                if (controlador.RegistrarNotificacion(
                    idCliente,
                    titulo,
                    mensaje,
                    "Insignias",
                    "insignia_obtenida_" + obtenida.Id,
                    obtenida.FechaObtencion))
                {
                    cantidad++;
                }
            }

            return cantidad;
        }
    }
}