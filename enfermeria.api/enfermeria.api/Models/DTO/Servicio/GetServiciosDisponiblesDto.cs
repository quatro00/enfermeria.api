using Microsoft.Identity.Client;

namespace enfermeria.api.Models.DTO.Servicio
{
    public class GetServiciosDisponiblesDto
    {
        public Guid Id { get; set; }
        public int No {  get; set; }
        public string Motivo { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTermino { get; set; }
        public decimal importe { get; set; }
        public bool RequiereAyudaBasica { get; set; }
        public string RequiereAyudaBasicaDesc { get; set; }
        public bool EnfermedadDiagnosticada { get; set; }
        public string EnfermedadDiagnosticadaDesc { get; set; }
        public bool TomaMedicamento { get; set; }
        public string TomaMedicamentoDesc { get; set; }
        public bool RequiereCuraciones { get; set; }
        public string RequiereCuracionesDesc { get; set; }
        public bool DispositivosMedicos { get; set; }
        public string DispositivosMedicosDesc { get; set; }
        public bool RequiereMonitoreo { get; set; }
        public string RequiereMonitoreoDesc { get; set; }
        public bool CuidadosNocturnos { get; set; }
        public string CuidadosNocturnosDesc { get; set; }
        public bool RequiereAtencionNeurologica { get; set; }
        public string RequiereAtencionNeurologicaDesc { get; set; }
        public bool RequiereCuidadosCriticos { get; set; }
        public string RequiereCuidadosCriticosDesc { get; set; }
        public string TipoEnfermera { get; set; }
        public string TipoLugar { get; set; }
        public string Observaciones { get; set; }
        public decimal Horas { get; set; }
        public decimal Total { get; set; }
    }
}
