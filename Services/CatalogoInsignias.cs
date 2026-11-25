using System.Collections.Generic;
using FitCR.Models.Insignias;

namespace FitCR.Services
{
    public static class CatalogoInsignias
    {
        public static List<Insignia> ObtenerInsignias()
        {
            return new List<Insignia>
            {
                Crear(1, "Primer esfuerzo",
                    "Completar 1 rutina.",
                    "Rutinas", 1),

                Crear(2, "Constancia",
                    "Completar 10 rutinas.",
                    "Rutinas", 10),

                Crear(3, "Disciplina",
                    "Completar 25 rutinas.",
                    "Rutinas", 25),

                Crear(4, "Imparable",
                    "Completar 50 rutinas.",
                    "Rutinas", 50),

                Crear(5, "Amante de la fuerza",
                    "Completar 10 rutinas de fuerza.",
                    "Rutinas", 10),

                Crear(6, "Amante del cardio",
                    "Completar 10 rutinas de cardio.",
                    "Rutinas", 10),

                Crear(7, "Resistencia",
                    "Acumular 600 minutos de rutinas completadas.",
                    "Rutinas", 600),

                Crear(8, "Más fuerte que ayer",
                    "Aumentar las repeticiones o series de un ejercicio comparable.",
                    "Rutinas", 1),

                Crear(9, "Primer paso",
                    "Registrar la primera visita.",
                    "Visitas", 1),

                Crear(10, "Cliente activo",
                    "Registrar 10 visitas.",
                    "Visitas", 10),

                Crear(11, "Compromiso",
                    "Registrar 30 visitas.",
                    "Visitas", 30),

                Crear(12, "Veterano del gimnasio",
                    "Registrar 100 visitas.",
                    "Visitas", 100),

                Crear(13, "En racha",
                    "Asistir 3 días consecutivos.",
                    "Visitas", 3),

                Crear(14, "Semana perfecta",
                    "Asistir 7 días consecutivos.",
                    "Visitas", 7,
                    mensaje:
                    "¡Felicidades por tu constancia! " +
                    "Recuerda que el descanso también " +
                    "forma parte del entrenamiento. " +
                    "Incluye días de recuperación entre " +
                    "tus entrenamientos intensos para " +
                    "evitar la sobrecarga y cuidar tu salud."),

                Crear(15, "Mes activo",
                    "Registrar 15 visitas durante un mes.",
                    "Visitas", 15,
                    periodica: true),

                Crear(16, "Top 10 mensual",
                    "Estar entre los 10 clientes con mayor asistencia mensual.",
                    "Visitas", 10,
                    periodica: true),

                Crear(17, "Rey o reina del gimnasio",
                    "Ser el cliente con mayor asistencia del mes.",
                    "Visitas", 1,
                    periodica: true),

                Crear(18, "De vuelta al gimnasio",
                    "Regresar después de 15 días consecutivos sin asistir.",
                    "Visitas", 15,
                    mensaje:
                    "¡Bienvenido de vuelta! Cada día es " +
                    "una nueva oportunidad para avanzar."),

                Crear(19, "Primera meta",
                    "Completar el primer objetivo.",
                    "Objetivos", 1),

                Crear(20, "Superación personal",
                    "Completar 5 objetivos.",
                    "Objetivos", 5),

                Crear(21, "Sin límites",
                    "Completar 10 objetivos.",
                    "Objetivos", 10),

                Crear(22, "Peso objetivo alcanzado",
                    "Cumplir un objetivo relacionado con el peso.",
                    "Objetivos", 1),

                Crear(23, "Misión cumplida",
                    "Cumplir un objetivo de rutinas.",
                    "Objetivos", 1),

                Crear(24, "Asistencia ejemplar",
                    "Cumplir un objetivo de visitas.",
                    "Objetivos", 1),

                Crear(25, "Antes de tiempo",
                    "Completar un objetivo antes de su fecha límite.",
                    "Objetivos", 1),

                Crear(26, "Todo cumplido",
                    "Completar todos los objetivos registrados, con un mínimo de 3.",
                    "Objetivos", 3),

                Crear(27, "Seguimiento constante",
                    "Registrar 5 medidas corporales.",
                    "Medidas", 5),

                Crear(28, "Mi evolución",
                    "Registrar 10 medidas corporales.",
                    "Medidas", 10),

                Crear(29, "Conoce tu progreso",
                    "Registrar medidas en 3 meses diferentes.",
                    "Medidas", 3),

                Crear(30, "Compañero Fit",
                    "Registrar el primer invitado.",
                    "Invitados", 1),

                Crear(31, "Entrenar acompañado",
                    "Registrar 5 visitas de invitados.",
                    "Invitados", 5),

                Crear(32, "Embajador Fit.CR",
                    "Invitar a 10 personas diferentes.",
                    "Invitados", 10),

                Crear(33, "Atleta versátil",
                    "Participar en todos los tipos de clases disponibles.",
                    "Clases", 5),

                Crear(34, "Vergüenza",
                    "No registrar visitas durante un mes calendario completo de membresía.",
                    "Asistencia", 0,
                    negativa: true,
                    periodica: true,
                    mensaje:
                    "Durante el último mes no registraste " +
                    "ninguna visita al gimnasio. " +
                    "¡Es momento de retomar el ritmo! " +
                    "Cada pequeño esfuerzo cuenta para " +
                    "alcanzar tus objetivos."),

                Crear(35, "Modo descanso",
                    "Acumular 15 días consecutivos sin asistir.",
                    "Asistencia", 15,
                    negativa: true,
                    mensaje:
                    "Si tus circunstancias lo permiten, " +
                    "puedes retomar tu actividad física " +
                    "de manera gradual."),

                Crear(36, "Meta pendiente",
                    "Tener un objetivo vencido sin completar.",
                    "Objetivos", 1,
                    negativa: true,
                    mensaje:
                    "No alcanzar una meta a tiempo no " +
                    "significa fracasar. Puedes revisar " +
                    "tus objetivos y continuar avanzando."),

                Crear(37, "Recupera el ritmo",
                    "Registrar menos de 4 visitas durante un mes calendario completo de membresía.",
                    "Asistencia", 4,
                    negativa: true,
                    periodica: true,
                    mensaje:
                    "Intenta establecer una frecuencia " +
                    "de asistencia que se adapte a tu " +
                    "tiempo, salud y necesidades."),

                Crear(38, "Baja asistencia",
                    "Estar entre los 10 clientes con menor asistencia mensual.",
                    "Asistencia", 10,
                    negativa: true,
                    periodica: true,
                    mensaje:
                    "Cada persona tiene su propio ritmo. " +
                    "Concéntrate en mejorar tu constancia " +
                    "y alcanzar tus objetivos personales.")
            };
        }

        private static Insignia Crear(
            int id,
            string nombre,
            string descripcion,
            string categoria,
            int cantidad,
            bool negativa = false,
            bool periodica = false,
            string mensaje = "")
        {
            return new Insignia
            {
                Id = id,
                Nombre = nombre,
                Descripcion = descripcion,
                Categoria = categoria,
                CantidadRequerida = cantidad,
                Icono = string.Empty,
                EsNegativa = negativa,
                EsPeriodica = periodica,
                Mensaje = string.IsNullOrWhiteSpace(mensaje)
                    ? "¡Felicidades! Has obtenido una nueva insignia: "
                        + nombre + "."
                    : mensaje
            };
        }
    }
}
