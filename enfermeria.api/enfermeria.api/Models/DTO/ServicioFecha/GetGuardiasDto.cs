using DocumentFormat.OpenXml.Bibliography;

namespace enfermeria.api.Models.DTO.ServicioFecha
{
    public class GetGuardiasDto
    {
        public Guid Id { get; set; }
        public int NoServicio { get; set; }
        public string Colaborador { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin {  get; set; }
        public string EstatusServicioFecha { get; set; }
        public decimal CantidadHoras { get; set; }
    }
}
