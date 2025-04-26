namespace enfermeria.api.Models.DTO.ServicioFecha
{
    public class GetServicioFechaFiltrosDto
    {
        public int? ColaboradorAsignadoId { get; set; }
        public int? EstatusServicioFechaId { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Fin { get; set; }
    }
}
