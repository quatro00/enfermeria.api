namespace enfermeria.api.Models.DTO.Servicio
{
    public class FilterGetServiciosDisponiblesDto
    {
        public Guid? EstadoId { get; set; }
        public Guid? MunicipioId { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? FechaFin {  get; set; }
    }
}
