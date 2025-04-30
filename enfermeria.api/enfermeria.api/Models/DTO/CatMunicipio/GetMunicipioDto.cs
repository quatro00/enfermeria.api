namespace enfermeria.api.Models.DTO.CatMunicipio
{
    public class GetMunicipioDto
    {
        public Guid Id { get; set; }
        public Guid EstadoId { get; set; }
        public string Nombre { get; set; }
        public string NombreCorto { get; set; }
    }
}
