using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FitCR.Controllers;
using FitCR.Models.Medidas;
using FitCR.Models.Objetivos;
using FitCR.Models.Rutinas;
using FitCR.Models.Usuarios;
using FitCR.Models.Visitas;
using FitCR.Repositories;
using FitCR.Models.Clases;

//Código del generador de usuarios
namespace FitCR.Services
{
    public class ServicioDatosDemostracion
    {
        private readonly string rutaData;
        private readonly Random aleatorio = new Random(2026);
        private readonly RepositorioJson<Cliente>repositorioClientes;
        private readonly RepositorioJson<EntrenadorPersonal>repositorioPersonales;
        private readonly RepositorioJson<EntrenadorGimnasio>repositorioGimnasio;

        private readonly string[] nombres =
        {
            "Mariana", "Daniel", "Valeria", "Andrés",
            "Sofía", "Gabriel", "Camila", "Sebastián",
            "Lucía", "Diego", "Isabella", "Esteban",
            "Natalia", "Adrián", "Paula", "José",
            "Fernanda", "David", "Laura", "Alejandro",
            "Carolina", "Marco", "Daniela", "Luis",
            "Gabriela", "Javier", "Victoria", "Felipe",
            "Elena", "Pablo", "Ana", "Cristian",
            "Melissa", "Ricardo", "Andrea", "Jorge",
            "Patricia", "Mauricio", "Tatiana", "Kevin",
            "Rebeca", "Isaac", "Sara", "Antonio",
            "Nicole", "Emmanuel", "Jimena", "Mateo",
            "Alison", "Samuel"
        };

        private readonly string[] apellidos =
        {
            "Mora", "Vargas", "Rodríguez", "Jiménez",
            "Rojas", "Hernández", "Solís", "Castro",
            "Ramírez", "Alvarado", "Chaves", "Aguilar",
            "Sánchez", "González", "Araya", "Salazar",
            "Montero", "Quesada", "Vega", "Méndez"
        };

        private readonly string[] tiposClases =
        {
            "Yoga",
            "Zumba",
            "Spinning",
            "CrossFit",
            "Gimnasio abierto"
        };

        public ServicioDatosDemostracion()
        {
            rutaData = Path.Combine(
                AppContext.BaseDirectory, "Data");

            repositorioClientes =
                new RepositorioJson<Cliente>(
                    Path.Combine(rutaData, "clientes.json"));

            repositorioPersonales =
                new RepositorioJson<EntrenadorPersonal>(
                    Path.Combine(
                        rutaData,
                        "entrenadores_personales.json"));

            repositorioGimnasio =
                new RepositorioJson<EntrenadorGimnasio>(
                    Path.Combine(
                        rutaData,
                        "entrenadores_gimnasio.json"));
        }

        public void GenerarUsuarios()
        {
            GenerarClientes();
            GenerarEntrenadores();
            AsignarClientes();
        }

        private string GenerarNombre(int posicion)
        {
            string nombre =
                nombres[posicion % nombres.Length];

            string primerApellido =
                apellidos[aleatorio.Next(apellidos.Length)];

            string segundoApellido =
                apellidos[aleatorio.Next(apellidos.Length)];

            return nombre + " " +
                primerApellido + " " + segundoApellido;
        }

        private void GenerarClientes()
        {
            ControladorClientes controlador =
                new ControladorClientes(repositorioClientes);

            ServicioUsuarios servicioUsuarios =
                new ServicioUsuarios();

            Contrasenas servicioContrasenas =
                new Contrasenas();

            for (int i = 1; i <= 50; i++)
            {
                string nombreUsuario =
                    "cliente_demo_" + i.ToString("D3");

                if (servicioUsuarios.ExisteNombreUsuario(
                    nombreUsuario))
                {
                    continue;
                }

                Cliente cliente = new Cliente
                {
                    Nombre = GenerarNombre(i - 1),
                    NombreUsuario = nombreUsuario,
                    Edad = aleatorio.Next(18, 66),
                    Peso = Math.Round(
                        50 + aleatorio.NextDouble() * 50, 2),
                    Estatura = Math.Round(
                        1.50 + aleatorio.NextDouble() * 0.45, 2)
                };

                cliente.ContrasenaHash =
                    servicioContrasenas.GenerarHash("FitCR1234");

                controlador.RegistrarCliente(cliente);

                var clientes =
                    repositorioClientes.ObtenerTodos();

                var clienteGuardado = clientes.First(
                    c => c.NombreUsuario == nombreUsuario);

                DateTime fechaRegistro =
                    new DateTime(2026, 1, 1)
                    .AddDays(aleatorio.Next(0, 60));

                clienteGuardado.FechaRegistro =
                    fechaRegistro;

                repositorioClientes.GuardarTodos(clientes);
            }
        }

        private void GenerarEntrenadores()
        {
            ControladorEntrenadores controlador =
                new ControladorEntrenadores(
                    repositorioPersonales,
                    repositorioGimnasio,
                    repositorioClientes);

            ServicioUsuarios servicioUsuarios =
                new ServicioUsuarios();

            for (int i = 1; i <= 40; i++)
            {
                string usuarioPersonal =
                    "personal_demo_" + i.ToString("D3");

                if (!servicioUsuarios.ExisteNombreUsuario(
                    usuarioPersonal))
                {
                    EntrenadorPersonal entrenador =
                        new EntrenadorPersonal
                        {
                            Nombre = GenerarNombre(i + 10),
                            NombreUsuario = usuarioPersonal,
                            Edad = aleatorio.Next(23, 56),
                            ClaseImpartida = i % 3 == 0
                                ? tiposClases[
                                    i % tiposClases.Length]
                                : "Ninguna"
                        };

                    controlador.RegistrarEntrenadorPersonal(
                        entrenador, "FitCR1234");
                }

                string usuarioGimnasio =
                    "gimnasio_demo_" + i.ToString("D3");

                if (!servicioUsuarios.ExisteNombreUsuario(
                    usuarioGimnasio))
                {
                    EntrenadorGimnasio entrenador =
                        new EntrenadorGimnasio
                        {
                            Nombre = GenerarNombre(i + 20),
                            NombreUsuario = usuarioGimnasio,
                            Edad = aleatorio.Next(23, 56),
                            ClaseImpartida = tiposClases[
                                (i - 1) % tiposClases.Length]
                        };

                    controlador.RegistrarEntrenadorGimnasio(
                        entrenador, "FitCR1234");
                }
            }
        }

        private void AsignarClientes()
        {
            var clientes = repositorioClientes.ObtenerTodos()
                .Where(c => c.NombreUsuario.StartsWith(
                    "cliente_demo_"))
                .OrderBy(c => c.NombreUsuario)
                .ToList();

            var entrenadores =
                repositorioPersonales.ObtenerTodos()
                .Where(e => e.NombreUsuario.StartsWith(
                    "personal_demo_"))
                .OrderBy(e => e.NombreUsuario)
                .ToList();

            if (entrenadores.Count == 0)
            {
                return;
            }

            ControladorEntrenadores controlador =
                new ControladorEntrenadores(
                    repositorioPersonales,
                    repositorioGimnasio,
                    repositorioClientes);

            for (int i = 0; i < clientes.Count; i++)
            {
                var entrenador =
                    entrenadores[i % entrenadores.Count];

                var cliente = clientes[i];

                if (!entrenador.ClientesAsignados.Contains(
                    cliente.Id))
                {
                    controlador.AsignarCliente(
                        entrenador.Id, cliente.Id);

                    entrenador.ClientesAsignados.Add(
                        cliente.Id);
                }
            }
        }

        public void GenerarRutinas()
        {
            RepositorioJson<Rutina> repositorioRutinas =
                new RepositorioJson<Rutina>(
                    Path.Combine(rutaData, "rutinas.json"));

            var rutinas = repositorioRutinas.ObtenerTodos();

            var clientes = repositorioClientes.ObtenerTodos()
                .Where(c => c.NombreUsuario.StartsWith(
                    "cliente_demo_"))
                .OrderBy(c => c.NombreUsuario)
                .ToList();

            string[] ejerciciosFuerza =
            {
        "Sentadillas",
        "Press de banca",
        "Peso muerto",
        "Curl de bíceps",
        "Press de hombros"
    };

            string[] ejerciciosCardio =
            {
        "Caminadora",
        "Bicicleta estática",
        "Elíptica",
        "Saltar cuerda"
    };

            int siguienteId = rutinas.Count == 0
                ? 1
                : rutinas.Max(r => r.Id) + 1;

            Random randomRutinas = new Random(2027);

            for (int i = 0; i < clientes.Count; i++)
            {
                Cliente cliente = clientes[i];

                if (!cliente.FechaRegistro.HasValue)
                {
                    continue;
                }

                if (rutinas.Any(r => r.IdCliente == cliente.Id))
                {
                    continue;
                }

                int cantidadCompletadas;

                if (i < 10)
                {
                    cantidadCompletadas = 7;
                }
                else if (i < 25)
                {
                    cantidadCompletadas = 6;
                }
                else if (i < 35)
                {
                    cantidadCompletadas = 4;
                }
                else if (i < 45)
                {
                    cantidadCompletadas = 2;
                }
                else
                {
                    cantidadCompletadas = 0;
                }

                DateTime fechaRegistro =
                    cliente.FechaRegistro.Value.Date;

                string ejercicioPrincipal =
                    ejerciciosFuerza[i % ejerciciosFuerza.Length];

                for (int j = 0; j < 7; j++)
                {
                    bool completada = j < cantidadCompletadas;

                    string nombre;
                    string tipo;
                    int series;
                    int repeticiones;
                    int duracion;

                    if (j == 0 || j == 1)
                    {
                        nombre = ejercicioPrincipal;
                        tipo = "Fuerza";
                        series = 3 + j;
                        repeticiones = 10 + j * 2;
                        duracion = 20 + j * 5;
                    }
                    else if (j % 2 == 0)
                    {
                        nombre = ejerciciosCardio[
                            randomRutinas.Next(
                                ejerciciosCardio.Length)];

                        tipo = "Cardio";
                        series = 1;
                        repeticiones = 1;
                        duracion = randomRutinas.Next(15, 61);
                    }
                    else
                    {
                        nombre = ejerciciosFuerza[
                            randomRutinas.Next(
                                ejerciciosFuerza.Length)];

                        tipo = "Fuerza";
                        series = randomRutinas.Next(3, 6);
                        repeticiones = randomRutinas.Next(8, 16);
                        duracion = randomRutinas.Next(15, 46);
                    }

                    DateTime fechaCreacion =
                        fechaRegistro.AddDays(5 + j * 30);

                    DateTime fechaVencimiento =
                        fechaCreacion.AddDays(20);

                    DateTime? fechaCompletada = null;

                    if (completada)
                    {
                        fechaCompletada =
                            fechaCreacion.AddDays(
                                randomRutinas.Next(1, 15));
                    }
                    else if (j == 6 && i % 2 == 0)
                    {
                        fechaCreacion = DateTime.Today.AddDays(-2);
                        fechaVencimiento = DateTime.Today.AddDays(15);
                    }

                    Rutina rutina = new Rutina
                    {
                        Id = siguienteId++,
                        IdCliente = cliente.Id,
                        Nombre = nombre,
                        Descripcion = tipo == "Fuerza"
                            ? "Entrenamiento para mejorar la fuerza muscular."
                            : "Entrenamiento de resistencia cardiovascular.",
                        TipoEjercicio = tipo,
                        Duracion = duracion,
                        Series = series,
                        Repeticiones = repeticiones,
                        FechaCreacion = fechaCreacion,
                        FechaVencimiento = fechaVencimiento,
                        Completada = completada,
                        FechaCompletada = fechaCompletada
                    };

                    rutinas.Add(rutina);
                }
            }

            repositorioRutinas.GuardarTodos(rutinas);
        }

        public void GenerarVisitas()
        {
            RepositorioJson<RegistroVisita> repositorioVisitas =
                new RepositorioJson<RegistroVisita>(
                    Path.Combine(rutaData, "visitas.json"));

            var visitas = repositorioVisitas.ObtenerTodos();

            var clientes = repositorioClientes.ObtenerTodos()
                .Where(c => c.NombreUsuario.StartsWith(
                    "cliente_demo_"))
                .OrderBy(c => c.NombreUsuario)
                .ToList();

            int siguienteId = visitas.Count == 0
                ? 1
                : visitas.Max(v => v.Id) + 1;

            Random randomVisitas = new Random(2028);

            for (int i = 0; i < clientes.Count; i++)
            {
                Cliente cliente = clientes[i];

                if (!cliente.FechaRegistro.HasValue)
                {
                    continue;
                }

                if (visitas.Any(v => v.IdCliente == cliente.Id))
                {
                    continue;
                }

                int cantidadVisitas;

                if (i < 10)
                {
                    cantidadVisitas = 105;
                }
                else if (i < 25)
                {
                    cantidadVisitas = 20;
                }
                else if (i < 35)
                {
                    cantidadVisitas = 12;
                }
                else if (i < 45)
                {
                    cantidadVisitas = 3;
                }
                else
                {
                    cantidadVisitas = 0;
                }

                if (cantidadVisitas == 0)
                {
                    continue;
                }

                DateTime fechaInicial =
                    cliente.FechaRegistro.Value.Date;

                DateTime fechaFinal =
                    DateTime.Today.AddDays(-1);

                int diasDisponibles =
                    (fechaFinal - fechaInicial).Days + 1;

                if (diasDisponibles < cantidadVisitas)
                {
                    throw new InvalidOperationException(
                        "No hay suficientes fechas para generar las visitas.");
                }

                HashSet<DateTime> fechas =
                    new HashSet<DateTime>();

                if (i < 10)
                {
                    DateTime inicioRacha =
                        new DateTime(2026, 4, 1)
                        .AddDays(i);

                    for (int dia = 0; dia < 7; dia++)
                    {
                        fechas.Add(inicioRacha.AddDays(dia));
                    }

                    DateTime inicioMesActivo =
                        new DateTime(2026, 6, 1);

                    for (int dia = 0; dia < 20; dia++)
                    {
                        fechas.Add(inicioMesActivo.AddDays(dia));
                    }
                }

                while (fechas.Count < cantidadVisitas)
                {
                    DateTime fecha = fechaInicial.AddDays(
                        randomVisitas.Next(diasDisponibles));

                    if (i < 10 &&
                        fecha.Month == 8 &&
                        fecha.Year == 2026)
                    {
                        continue;
                    }

                    fechas.Add(fecha);
                }

                foreach (DateTime fecha in fechas.OrderBy(f => f))
                {
                    RegistroVisita visita = new RegistroVisita
                    {
                        Id = siguienteId++,
                        IdCliente = cliente.Id,
                        FechaVisita = fecha
                            .AddHours(randomVisitas.Next(6, 21))
                            .AddMinutes(randomVisitas.Next(0, 60))
                    };

                    visitas.Add(visita);
                }
            }

            repositorioVisitas.GuardarTodos(visitas);
        }

        public void GenerarMedidas()
        {
            RepositorioJson<RegistroMedida> repositorioMedidas =
                new RepositorioJson<RegistroMedida>(
                    Path.Combine(rutaData, "medidas.json"));

            var medidas = repositorioMedidas.ObtenerTodos();

            var todosClientes = repositorioClientes.ObtenerTodos();

            var clientes = todosClientes
                .Where(c => c.NombreUsuario.StartsWith(
                    "cliente_demo_"))
                .OrderBy(c => c.NombreUsuario)
                .ToList();

            int siguienteId = medidas.Count == 0
                ? 1
                : medidas.Max(m => m.Id) + 1;

            Random randomMedidas = new Random(2029);

            for (int i = 0; i < clientes.Count; i++)
            {
                Cliente cliente = clientes[i];

                if (!cliente.FechaRegistro.HasValue)
                {
                    continue;
                }

                if (medidas.Any(m => m.IdCliente == cliente.Id))
                {
                    continue;
                }

                double pesoInicial = cliente.Peso;
                double estatura = cliente.Estatura;
                double pesoActual = pesoInicial;

                DateTime fechaInicial =
                    cliente.FechaRegistro.Value.Date;

                double cambioMensual = Math.Round(
                    0.30 + randomMedidas.NextDouble() * 0.70, 2);

                for (int j = 0; j < 6; j++)
                {
                    if (i < 20)
                    {
                        pesoActual = pesoInicial - cambioMensual * j;
                    }
                    else if (i < 35)
                    {
                        pesoActual = pesoInicial + cambioMensual * j;
                    }
                    else
                    {
                        if (j == 0 || j == 5)
                        {
                            pesoActual = pesoInicial;
                        }
                        else
                        {
                            pesoActual = pesoInicial +
                                (randomMedidas.NextDouble() - 0.5);
                        }
                    }

                    pesoActual = Math.Round(pesoActual, 2);

                    DateTime fechaMedida =
                        fechaInicial.AddMonths(j);

                    RegistroMedida medida = new RegistroMedida
                    {
                        Id = siguienteId++,
                        IdCliente = cliente.Id,
                        Fecha = fechaMedida,
                        Peso = pesoActual,
                        Estatura = estatura,
                        IMC = Math.Round(
                            pesoActual / (estatura * estatura), 2)
                    };

                    medidas.Add(medida);
                }

                cliente.Peso = pesoActual;
            }

            repositorioMedidas.GuardarTodos(medidas);
            repositorioClientes.GuardarTodos(todosClientes);
        }

        public void GenerarObjetivos()
        {
            RepositorioJson<Objetivo> repositorioObjetivos =
                new RepositorioJson<Objetivo>(
                    Path.Combine(rutaData, "objetivos.json"));

            RepositorioJson<Rutina> repositorioRutinas =
                new RepositorioJson<Rutina>(
                    Path.Combine(rutaData, "rutinas.json"));

            RepositorioJson<RegistroMedida> repositorioMedidas =
                new RepositorioJson<RegistroMedida>(
                    Path.Combine(rutaData, "medidas.json"));

            RepositorioJson<RegistroVisita> repositorioVisitas =
                new RepositorioJson<RegistroVisita>(
                    Path.Combine(rutaData, "visitas.json"));

            var objetivos = repositorioObjetivos.ObtenerTodos();
            var medidas = repositorioMedidas.ObtenerTodos();

            var clientes = repositorioClientes.ObtenerTodos()
                .Where(c => c.NombreUsuario.StartsWith(
                    "cliente_demo_"))
                .OrderBy(c => c.NombreUsuario)
                .ToList();

            int siguienteId = objetivos.Count == 0
                ? 1
                : objetivos.Max(o => o.Id) + 1;

            for (int i = 0; i < clientes.Count; i++)
            {
                Cliente cliente = clientes[i];

                if (!cliente.FechaRegistro.HasValue)
                {
                    continue;
                }

                if (objetivos.Any(o => o.IdCliente == cliente.Id))
                {
                    continue;
                }

                DateTime fechaCreacion =
                    cliente.FechaRegistro.Value.Date;

                DateTime fechaLimite = i % 3 == 0
                    ? DateTime.Today.AddDays(30)
                    : new DateTime(2026, 10, 31);

                var historial = medidas
                    .Where(m => m.IdCliente == cliente.Id)
                    .OrderBy(m => m.Fecha)
                    .ToList();

                double pesoInicial = historial.Count > 0
                    ? historial.First().Peso
                    : cliente.Peso;

                double pesoFinal = historial.Count > 0
                    ? historial.Last().Peso
                    : cliente.Peso;

                double metaPeso;

                if (i < 20)
                {
                    metaPeso = Math.Round(
                        (pesoInicial + pesoFinal) / 2, 2);
                }
                else if (i < 35)
                {
                    metaPeso = Math.Round(
                        (pesoInicial + pesoFinal) / 2, 2);
                }
                else
                {
                    metaPeso = Math.Round(pesoInicial - 3, 2);
                }

                Objetivo objetivoPeso = new Objetivo
                {
                    Id = siguienteId++,
                    IdCliente = cliente.Id,
                    Nombre = "Alcanzar mi peso objetivo",
                    Descripcion = "Alcanzar el peso establecido y registrar mi progreso.",
                    TipoObjetivo = "Peso",
                    ValorMeta = metaPeso,
                    FechaCreacion = fechaCreacion,
                    FechaLimite = fechaLimite,
                    Completado = false,
                    FechaCompletado = null
                };

                Objetivo objetivoRutinas = new Objetivo
                {
                    Id = siguienteId++,
                    IdCliente = cliente.Id,
                    Nombre = "Mejorar mi constancia",
                    Descripcion = "Completar las rutinas de entrenamiento establecidas.",
                    TipoObjetivo = "Rutinas completadas",
                    ValorMeta = i % 2 == 0 ? 3 : 5,
                    FechaCreacion = fechaCreacion,
                    FechaLimite = fechaLimite,
                    Completado = false,
                    FechaCompletado = null
                };

                Objetivo objetivoVisitas = new Objetivo
                {
                    Id = siguienteId++,
                    IdCliente = cliente.Id,
                    Nombre = "Aumentar mi asistencia",
                    Descripcion = "Mantener una asistencia constante al gimnasio.",
                    TipoObjetivo = "Visitas al gimnasio",
                    ValorMeta = i % 2 == 0 ? 10 : 30,
                    FechaCreacion = fechaCreacion,
                    FechaLimite = fechaLimite,
                    Completado = false,
                    FechaCompletado = null
                };

                objetivos.Add(objetivoPeso);
                objetivos.Add(objetivoRutinas);
                objetivos.Add(objetivoVisitas);
            }

            repositorioObjetivos.GuardarTodos(objetivos);

            ControladorProgresoObjetivos controlador =
                new ControladorProgresoObjetivos(
                    repositorioObjetivos,
                    repositorioRutinas,
                    repositorioMedidas,
                    repositorioVisitas);

            foreach (Cliente cliente in clientes)
            {
                controlador.SincronizarObjetivos(cliente.Id);
            }
        }

        public void GenerarInvitados()
        {
            RepositorioJson<RegistroInvitado> repositorioInvitados =
                new RepositorioJson<RegistroInvitado>(
                    Path.Combine(rutaData, "invitados.json"));

            var invitados = repositorioInvitados.ObtenerTodos();

            var clientes = repositorioClientes.ObtenerTodos()
                .Where(c => c.NombreUsuario.StartsWith(
                    "cliente_demo_"))
                .OrderBy(c => c.NombreUsuario)
                .ToList();

            int siguienteId = invitados.Count == 0
                ? 1
                : invitados.Max(v => v.Id) + 1;

            for (int i = 0; i < clientes.Count; i++)
            {
                Cliente cliente = clientes[i];

                if (!cliente.FechaRegistro.HasValue)
                {
                    continue;
                }

                if (invitados.Any(v => v.IdCliente == cliente.Id))
                {
                    continue;
                }

                int cantidadInvitados;

                if (i < 5)
                {
                    cantidadInvitados = 10;
                }
                else if (i < 10)
                {
                    cantidadInvitados = 5;
                }
                else if (i < 20)
                {
                    cantidadInvitados = 2;
                }
                else if (i < 25)
                {
                    cantidadInvitados = 1;
                }
                else
                {
                    cantidadInvitados = 0;
                }

                DateTime fechaInicial =
                    cliente.FechaRegistro.Value.Date;

                for (int j = 0; j < cantidadInvitados; j++)
                {
                    DateTime fechaVisita =
                        fechaInicial.AddMonths(j / 2)
                        .AddDays((j % 2) * 7)
                        .AddHours(10);

                    RegistroInvitado invitado = new RegistroInvitado
                    {
                        Id = siguienteId++,
                        IdCliente = cliente.Id,
                        NombreInvitado = GenerarNombre(i + j + 15),
                        FechaVisita = fechaVisita
                    };

                    invitados.Add(invitado);
                }
            }

            repositorioInvitados.GuardarTodos(invitados);
        }

        public void GenerarClases()
        {
            RepositorioJson<ClaseGimnasio> repositorioClases =
                new RepositorioJson<ClaseGimnasio>(
                    Path.Combine(rutaData, "clases.json"));

            var clases = repositorioClases.ObtenerTodos();

            var entrenadores = repositorioGimnasio.ObtenerTodos()
                .Where(e => e.NombreUsuario.StartsWith(
                    "gimnasio_demo_"))
                .OrderBy(e => e.NombreUsuario)
                .ToList();

            int siguienteId = clases.Count == 0
                ? 1
                : clases.Max(c => c.Id) + 1;

            for (int i = 0; i < tiposClases.Length; i++)
            {
                string tipoClase = tiposClases[i];

                var entrenadoresDisponibles = entrenadores
                    .Where(e => e.ClaseImpartida == tipoClase)
                    .ToList();

                if (entrenadoresDisponibles.Count == 0)
                {
                    continue;
                }

                for (int j = 0; j < 20; j++)
                {
                    string descripcion =
                        "Sesión de " + tipoClase +
                        " - Demostración " + (j + 1).ToString("D2");

                    if (clases.Any(c =>
                        c.Descripcion == descripcion))
                    {
                        continue;
                    }

                    var entrenador = entrenadoresDisponibles[
                        j % entrenadoresDisponibles.Count];

                    DateTime fechaHora;

                    if (j < 17)
                    {
                        fechaHora = new DateTime(2026, 3, 15)
                            .AddDays(j * 12 + i)
                            .AddHours(8 + i * 2);
                    }
                    else
                    {
                        fechaHora = DateTime.Today
                            .AddDays(2 + (j - 17) * 5 + i)
                            .AddHours(8 + i * 2);
                    }

                    ClaseGimnasio clase = new ClaseGimnasio
                    {
                        Id = siguienteId++,
                        Nombre = tipoClase,
                        Descripcion = descripcion,
                        IdEntrenador = entrenador.Id,
                        FechaHora = fechaHora,
                        Duracion = tipoClase == "Gimnasio abierto"
                            ? 90
                            : 60,
                        CupoMaximo = 15 + (j % 4) * 5,
                        Cancelada = j == 16
                    };

                    clases.Add(clase);
                }
            }

            repositorioClases.GuardarTodos(clases);
        }

        public void GenerarInscripciones()
        {
            RepositorioJson<ClaseGimnasio> repositorioClases =
                new RepositorioJson<ClaseGimnasio>(
                    Path.Combine(rutaData, "clases.json"));

            RepositorioJson<InscripcionClase> repositorioInscripciones =
                new RepositorioJson<InscripcionClase>(
                    Path.Combine(rutaData, "inscripciones_clases.json"));

            var clases = repositorioClases.ObtenerTodos()
                .Where(c => c.Descripcion.Contains(
                    " - Demostración "))
                .OrderBy(c => c.Id)
                .ToList();

            var inscripciones =
                repositorioInscripciones.ObtenerTodos();

            var clientes = repositorioClientes.ObtenerTodos()
                .Where(c => c.NombreUsuario.StartsWith(
                    "cliente_demo_"))
                .OrderBy(c => c.NombreUsuario)
                .ToList();

            if (clientes.Count < 3)
            {
                return;
            }

            int siguienteId = inscripciones.Count == 0
                ? 1
                : inscripciones.Max(i => i.Id) + 1;

            for (int i = 0; i < clases.Count; i++)
            {
                ClaseGimnasio clase = clases[i];

                if (inscripciones.Any(
                    x => x.IdClase == clase.Id))
                {
                    continue;
                }

                for (int j = 0; j < 3; j++)
                {
                    int posicion = j == 0
                        ? 0
                        : 1 + ((i * 2 + j - 1) %
                            (clientes.Count - 1));

                    Cliente cliente = clientes[posicion];

                    DateTime fechaInscripcion =
                        clase.FechaHora.AddDays(-3);

                    if (fechaInscripcion > DateTime.Now)
                    {
                        fechaInscripcion =
                            DateTime.Today.AddDays(-1);
                    }

                    InscripcionClase inscripcion =
                        new InscripcionClase
                        {
                            Id = siguienteId++,
                            IdClase = clase.Id,
                            IdCliente = cliente.Id,
                            FechaInscripcion = fechaInscripcion
                        };

                    inscripciones.Add(inscripcion);
                }
            }

            repositorioInscripciones.GuardarTodos(inscripciones);
        }

        public void GenerarAsistencias()
        {
            RepositorioJson<ClaseGimnasio> repositorioClases =
                new RepositorioJson<ClaseGimnasio>(
                    Path.Combine(rutaData, "clases.json"));

            RepositorioJson<InscripcionClase> repositorioInscripciones =
                new RepositorioJson<InscripcionClase>(
                    Path.Combine(rutaData, "inscripciones_clases.json"));

            RepositorioJson<AsistenciaClase> repositorioAsistencias =
                new RepositorioJson<AsistenciaClase>(
                    Path.Combine(rutaData, "asistencias_clases.json"));

            var clases = repositorioClases.ObtenerTodos()
                .Where(c => c.Descripcion.Contains(
                    " - Demostración ") &&
                    !c.Cancelada &&
                    c.FechaHora.AddMinutes(c.Duracion) < DateTime.Now)
                .OrderBy(c => c.Id)
                .ToList();

            var inscripciones =
                repositorioInscripciones.ObtenerTodos();

            var asistencias =
                repositorioAsistencias.ObtenerTodos();

            int siguienteId = asistencias.Count == 0
                ? 1
                : asistencias.Max(a => a.Id) + 1;

            for (int i = 0; i < clases.Count; i++)
            {
                ClaseGimnasio clase = clases[i];

                var participantes = inscripciones
                    .Where(x => x.IdClase == clase.Id)
                    .OrderBy(x => x.Id)
                    .ToList();

                int cantidadAsistencias =
                    i % 2 == 0 ? 3 : 2;

                foreach (var participante in
                    participantes.Take(cantidadAsistencias))
                {
                    if (asistencias.Any(a =>
                        a.IdClase == clase.Id &&
                        a.IdCliente == participante.IdCliente))
                    {
                        continue;
                    }

                    AsistenciaClase asistencia =
                        new AsistenciaClase
                        {
                            Id = siguienteId++,
                            IdClase = clase.Id,
                            IdCliente = participante.IdCliente,
                            FechaAsistencia =
                                clase.FechaHora.AddMinutes(5)
                        };

                    asistencias.Add(asistencia);
                }
            }

            repositorioAsistencias.GuardarTodos(asistencias);
        }

        public void SincronizarLogrosYNotificaciones()
        {
            var clientes = repositorioClientes.ObtenerTodos()
                .Where(c => c.NombreUsuario.StartsWith(
                    "cliente_demo_"))
                .OrderBy(c => c.NombreUsuario)
                .ToList();

            ServicioNotificaciones servicioNotificaciones =
                new ServicioNotificaciones();

            foreach (Cliente cliente in clientes)
            {
                servicioNotificaciones.SincronizarNotificaciones(
                    cliente);
            }
        }
    }
}