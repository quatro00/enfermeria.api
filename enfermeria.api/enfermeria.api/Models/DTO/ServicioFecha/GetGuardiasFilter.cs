namespace enfermeria.api.Models.DTO.ServicioFecha
{
    public class GetGuardiasFilter
    {
        public string? NoServicio { get; set; }
        public int? EstatusServicioFechaId { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
