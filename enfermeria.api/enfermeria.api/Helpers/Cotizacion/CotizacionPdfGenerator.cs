
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using QuestPDF.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using enfermeria.api.Models.Domain;
using DocumentFormat.OpenXml.Drawing;

public class CotizacionPdfGenerator
{
   

    public static void GenerarPdf(Servicio servicio, string outputPath)
    {

        CotizacionPdf cotizacion = new CotizacionPdf(servicio);
        var logo = Placeholders.Image(100, 100); // logo dummy

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .Row(row =>
                    {
                        row.ConstantItem(100).Image(logo); // Logo dummy
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text($"  No cotización: {cotizacion.Numero}");
                            col.Item().Text($"  Fecha: {cotizacion.Fecha:dd/MM/yyyy}");
                            col.Item().Text($"  Vigencia: {cotizacion.Vigencia:dd/MM/yyyy}");
                            col.Item().Text($" ");
                            col.Item().Text("  Nombre de la Empresa").Bold();
                            col.Item().Text("  Dirección de la Empresa");
                            col.Item().Text("  Teléfono de la Empresa");
                        });
                    });

                page.Content().Column(col =>
                {
                    col.Spacing(15);

                    // Datos del paciente
                    col.Item().Text("Datos del Paciente").Bold();
                    col.Item().Text(text =>
                    {
                        text.Span("Nombre: ").Bold();
                        text.Span(cotizacion.Paciente.Nombre + "    ");

                        text.Span("Teléfono: ").Bold();
                        text.Span(cotizacion.Paciente.Telefono + "    ");

                        text.Span("Correo: ").Bold();
                        text.Span(cotizacion.Paciente.Correo);
                    });

                    // Datos de la cotización
                    col.Item().Text("Detalles de la Cotización").Bold();
                    col.Item().Text(text =>
                    {
                        text.Span("Estado: ").Bold();
                        text.Span(cotizacion.Estado + "    ");

                        text.Span("Tipo de lugar: ").Bold();
                        text.Span(cotizacion.TipoLugar + "    ");

                        text.Span("Tipo enfermera: ").Bold();
                        text.Span(cotizacion.TipoEnfermera);
                    });

                    col.Item().Text(text =>
                    {
                        text.Span("Dirección: ").Bold();
                        text.Span(cotizacion.Direccion);
                    });

                    col.Item().Text(text =>
                    {
                        text.Span("Motivo: ").Bold();
                        text.Span(cotizacion.Motivo);
                    });

                    col.Item().Text(text =>
                    {
                        text.Span("Observaciones: ").Bold();
                        text.Span(cotizacion.Observaciones);
                    });

                    // Requerimientos en dos columnas
                    col.Item().Text("Requerimientos del Paciente").Bold();
                    col.Item().Row(row =>
                    {
                        void AddField(ColumnDescriptor col, string label, string value)
                        {
                            col.Item().Text($"{label}:").Bold();
                            col.Item().Text(value);
                        }

                        row.RelativeItem().Column(c =>
                        {
                            AddField(c, "Requiere ayuda básica", cotizacion.RequiereAyudaBasica ? "Sí" : "No");
                            AddField(c, "Enfermedad diagnosticada", cotizacion.TieneEnfermedad ? "Sí" : "No");
                            AddField(c, "Toma medicamento", cotizacion.TomaMedicamento ? "Sí" : "No");
                            AddField(c, "Requiere curaciones", cotizacion.RequiereCuraciones ? "Sí" : "No");
                            AddField(c, "Dispositivos médicos", cotizacion.TieneDispositivos ? "Sí" : "No");
                        });

                        row.RelativeItem().Column(c =>
                        {
                            AddField(c, "Requiere monitoreo", cotizacion.RequiereMonitoreo ? "Sí" : "No");
                            AddField(c, "Cuidados nocturnos", cotizacion.RequiereCuidadosNocturnos ? "Sí" : "No");
                            AddField(c, "Atención neurológica", cotizacion.RequiereAtencionNeurologica ? "Sí" : "No");
                            AddField(c, "Cuidados críticos", cotizacion.RequiereCuidadosCriticos ? "Sí" : "No");
                        });
                    });

                    // Tabla de fechas y precios por día
                    col.Item().Text("Detalle de Servicios").Bold();
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(); // Fecha
                            columns.ConstantColumn(50); // Inicio
                            columns.ConstantColumn(50); // Fin
                            columns.RelativeColumn(); // Turno
                            columns.ConstantColumn(40); // Horas
                            columns.ConstantColumn(60); // $/hora
                            columns.ConstantColumn(60); // Subtotal
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Fecha").Bold();
                            header.Cell().Text("Inicio").Bold();
                            header.Cell().Text("Fin").Bold();
                            header.Cell().Text("Turno").Bold();
                            header.Cell().Text("Horas").Bold();
                            header.Cell().Text("$/Hora").Bold();
                            header.Cell().Text("Subtotal").Bold();
                        });

                        foreach (var detalle in cotizacion.Detalle)
                        {
                            table.Cell().Text(detalle.Fecha.ToString("dd/MM/yyyy"));
                            table.Cell().Text(detalle.HoraInicio.ToString("hh\\:mm"));
                            table.Cell().Text(detalle.HoraFin.ToString("hh\\:mm"));
                            table.Cell().Text(detalle.Turno);
                            table.Cell().Text(detalle.Horas.ToString("0.##"));
                            table.Cell().Text($"${detalle.PrecioPorHora:N2}");
                            table.Cell().Text($"${detalle.TotalDia:N2}");
                        }
                    });

                    // Totales por turno
                    col.Item().Text("Resumen por Turno").Bold();
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn();
                            c.ConstantColumn(60);
                            c.ConstantColumn(60);
                            c.ConstantColumn(70);
                        });

                        table.Header(h =>
                        {
                            h.Cell().Text("Turno").Bold();
                            h.Cell().Text("Horas").Bold();
                            h.Cell().Text("$/Hora").Bold();
                            h.Cell().Text("Subtotal").Bold();
                        });

                        foreach (var total in cotizacion.TotalesPorTurno)
                        {
                            table.Cell().Text(total.Turno);
                            table.Cell().Text($"{total.Horas:N2}");
                            table.Cell().Text($"${total.PrecioHora:N2}");
                            table.Cell().Text($"${total.Subtotal:N2}");
                        }
                        table.Cell().ColumnSpan(3).AlignRight().Text("Total General:").Bold();
                        table.Cell().Text($"${cotizacion.Descuentos:N2}").Bold();

                        table.Cell().ColumnSpan(3).AlignRight().Text("Total General:").Bold();
                        table.Cell().Text($"${(cotizacion.TotalGeneral - cotizacion.Descuentos):N2}").Bold();
                    });
                });
            });
        })
        .GeneratePdf(outputPath);
    }
}

// Clases modelo simplificadas
public class CotizacionPdf
{
    public CotizacionPdf(Servicio servicio) 
    {
        this.Numero = servicio.No;
        this.Descuentos = servicio.Descuento;
        this.Fecha = servicio.FechaCreacion;
        this.Vigencia = servicio.Vigencia;
        this.Estado = servicio.Municipio.Nombre +", " + servicio.Municipio.Estado.NombreCorto;
        this.Direccion = servicio.Direccion;
        this.Motivo = servicio.PrincipalRazon;
        this.RequiereAyudaBasica = servicio.RequiereAyudaBasica;
        this.DescripcionAyudaBasica = servicio.RequiereAyudaBasicaDesc;
        this.TieneEnfermedad = servicio.EnfermedadDiagnosticada;
        this.DescripcionEnfermedad = servicio.EnfermedadDiagnosticadaDesc;
        this.TomaMedicamento = servicio.TomaMedicamento;
        this.DescripcionMedicamento = servicio.TomaMedicamentoDesc;
        this.RequiereCuraciones = servicio.RequiereCuraciones;
        this.DescripcionCuraciones = servicio.RequiereCuracionesDesc;
        this.TieneDispositivos = servicio.DispositivosMedicos;
        this.DescripcionDispositivos = servicio.DispositivosMedicosDesc;
        this.RequiereMonitoreo = servicio.RequiereMonitoreo;
        this.DescripcionMonitoreo = servicio.RequiereMonitoreoDesc;
        this.RequiereCuidadosNocturnos = servicio.CuidadosNocturnos;
        this.DescripcionCuidadosNocturnos = servicio.CuidadosNocturnosDesc;
        this.RequiereAtencionNeurologica = servicio.RequiereAtencionNeurologica;
        this.DescripcionAtencionNeurologica = servicio.RequiereAtencionNeurologicaDesc;
        this.RequiereCuidadosCriticos = servicio.RequiereCuidadosCriticos;
        this.DescripcionCuidadosCriticos = servicio.RequiereCuidadosCriticosDesc;
        this.TipoEnfermera = servicio.TipoEnfermera.Descripcion;
        this.Observaciones = servicio.Observaciones;
        this.TipoLugar = servicio.TipoLugar.Descripcion;

        this.Paciente = new PacientePDF()
        {
            Correo = servicio.Paciente.CorreoElectronico,
            Nombre = servicio.Paciente.Nombre + " " + servicio.Paciente.Apellidos,
            Telefono = servicio.Paciente.Telefono,
        };

        this.Detalle = new List<DetalleServicioPDF>();
        this.TotalesPorTurno = new List<TotalPorTurno>();

        foreach(var item in servicio.ServicioFechas)
        {
            foreach(var itemDet in item.ServicioCotizacions)
            {
                this.Detalle.Add(new DetalleServicioPDF() {
                    Fecha = item.FechaInicio.Date,
                    HoraInicio = TimeOnly.FromDateTime(item.FechaInicio),
                    HoraFin = TimeOnly.FromDateTime(item.FechaTermino),
                    Horas = (int)itemDet.Horas,
                    PrecioPorHora = itemDet.PrecioHoraFinal,
                    Turno = itemDet.Horario
                });
            }
        }

        //buscamos todos los turnos en detalle
        var turnos = this.Detalle.Select(x=>x.Turno.ToString()).Distinct().ToList();
        foreach (var turno in turnos)
        {
            TotalPorTurno totalPorTurno = new TotalPorTurno();
            totalPorTurno.Turno = turno.ToString();
            totalPorTurno.Horas = this.Detalle.Where(x => x.Turno.ToUpper() == turno.ToUpper()).Sum(x=>x.Horas);
            totalPorTurno.PrecioHora = this.Detalle.Where(x => x.Turno.ToUpper() == turno.ToUpper()).FirstOrDefault().PrecioPorHora;
            this.TotalesPorTurno.Add(totalPorTurno);
        }

        this.TotalGeneral = this.TotalesPorTurno.Sum(x => x.Subtotal);
    }
    public int Numero { get; set; }
    public decimal Descuentos { get; set; }
    public DateTime Fecha { get; set; }
    public DateTime Vigencia { get; set; }
    public string Estado { get; set; }
    public string Direccion { get; set; }
    public string Motivo { get; set; }
    public bool RequiereAyudaBasica { get; set; }
    public string? DescripcionAyudaBasica { get; set; }
    public bool TieneEnfermedad { get; set; }
    public string? DescripcionEnfermedad { get; set; }
    public bool TomaMedicamento { get; set; }
    public string? DescripcionMedicamento { get; set; }
    public bool RequiereCuraciones { get; set; }
    public string? DescripcionCuraciones { get; set; }
    public bool TieneDispositivos { get; set; }
    public string? DescripcionDispositivos { get; set; }
    public bool RequiereMonitoreo { get; set; }
    public string? DescripcionMonitoreo { get; set; }
    public bool RequiereCuidadosNocturnos { get; set; }
    public string? DescripcionCuidadosNocturnos { get; set; }
    public bool RequiereAtencionNeurologica { get; set; }
    public string? DescripcionAtencionNeurologica { get; set; }
    public bool RequiereCuidadosCriticos { get; set; }
    public string? DescripcionCuidadosCriticos { get; set; }
    public string TipoEnfermera { get; set; }
    public string Observaciones { get; set; }
    public string TipoLugar { get; set; }
    public decimal TotalGeneral { get; set; }
    public PacientePDF Paciente { get; set; }
    public List<DetalleServicioPDF> Detalle { get; set; }
    public List<TotalPorTurno> TotalesPorTurno { get; set; }
}

public class PacientePDF
{
    public string Nombre { get; set; }
    public string Telefono { get; set; }
    public string Correo { get; set; }
}

public class DetalleServicioPDF
{
    public DateTime Fecha { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }
    public string Turno { get; set; }
    public int Horas { get; set; }
    public decimal PrecioPorHora { get; set; }
    public decimal TotalDia => Horas * PrecioPorHora;
}

public class TotalPorTurno
{
    public string Turno { get; set; } = null!; // Ej: "Matutino", "Vespertino", "Nocturno"
    public decimal Horas { get; set; }         // Total de horas en ese turno
    public decimal PrecioHora { get; set; }    // Costo por hora para ese turno
    public decimal Subtotal => Horas * PrecioHora; // Calculado automáticamente
}
