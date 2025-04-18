using enfermeria.api.Models.Domain;

namespace enfermeria.api.Helpers.Cotizacion
{
    public class CalculoCotizacion
    {
        public static List<ResultadoPorFecha> CalcularHorasPorTurnoPorFecha(List<CatHorario> turnosParams, List<ServicioFecha> rangosParams)
        {

            List<Turno> turnos = new List<Turno>();
            List< RangoFecha > rangos = new List<RangoFecha>();

            foreach (var horario in turnosParams)
            {
                turnos.Add(new Turno()
                {
                    Descripcion = horario.Descripcion,
                    HoraInicio = horario.HoraInicio,
                    HoraTermino = horario.HoraTermino,
                    PorcentajeTarifa = horario.PorcentajeTarifa,
                });
            }

            foreach (var item in rangosParams)
            {
                rangos.Add(new RangoFecha()
                {
                    Id = item.Id,
                    FechaInicio = item.FechaInicio,
                    FechaTermino = item.FechaTermino,
                });
            }

            var resultado = new List<ResultadoPorFecha>();

            foreach (var rango in rangos)
            {
                Dictionary<string, int> horasPorTurno = new();
                DateTime actual = rango.FechaInicio;

                DateTime fin = rango.FechaTermino;
                if (fin.Minute > 0 || fin.Second > 0 || fin.Millisecond > 0)
                {
                    fin = fin.AddMinutes(60 - fin.Minute).AddSeconds(-fin.Second).AddMilliseconds(-fin.Millisecond);
                }

                while (actual < fin)
                {
                    var horaActual = TimeOnly.FromDateTime(actual);

                    var turnoEncontrado = turnos.FirstOrDefault(t =>
                        t.HoraInicio <= t.HoraTermino
                            ? horaActual >= t.HoraInicio && horaActual < t.HoraTermino
                            : (horaActual >= t.HoraInicio || horaActual < t.HoraTermino)
                    );

                    if (turnoEncontrado != null)
                    {
                        if (!horasPorTurno.ContainsKey(turnoEncontrado.Descripcion))
                            horasPorTurno[turnoEncontrado.Descripcion] = 0;

                        horasPorTurno[turnoEncontrado.Descripcion]++;
                    }

                    actual = actual.AddHours(1);
                }

                var detalle = horasPorTurno.Select(kvp => new ResultadoDetalleTurno
                {
                    Horario = kvp.Key,
                    Horas = kvp.Value
                }).ToList();

                resultado.Add(new ResultadoPorFecha
                {
                    Id = rango.Id,
                    Fecha = rango.FechaInicio.Date,
                    DetallePorTurno = detalle
                });
            }

            return resultado;
        }

    }

    public class Turno
    {
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraTermino { get; set; }
        public string Descripcion { get; set; } = null!;
        public decimal PorcentajeTarifa { get; set; }
    }

    public class RangoFecha
    {
        public Guid Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTermino { get; set; }
    }

    public class ResultadoHorario
    {
        public string Horario { get; set; } = null!;
        public int Horas { get; set; }
    }

    public class ResultadoDetalleTurno
    {
        public string Horario { get; set; } = null!;
        public int Horas { get; set; }
    }

    public class ResultadoPorFecha
    {
        public DateTime Fecha { get; set; } // O puedes usar DateOnly si prefieres
        public Guid Id { get; set; }
        public List<ResultadoDetalleTurno> DetallePorTurno { get; set; } = new();
    }
}
