namespace enfermeria.api.Models.DTO.Estado
{
    public class EstadoDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string NombreCorto { get; set; }
        public List<EstadoMunicipioDto> Municipios { get; set; }
    }

    public class EstadoMunicipioDto
    {
        public Guid Id { get; set; }
        public Guid EstadoId { get; set; }
        public string Nombre { get; set; }
        public string NombreCorto { get; set; }
    }
}
