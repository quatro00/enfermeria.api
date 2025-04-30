namespace enfermeria.api.Models.DTO.ServicioFecha
{
    public class AdjuntarReferenciaDto
    {
        public Guid ServicioId { get; set; }
        public string Referencia { get; set; }
        public IFormFile Transferencia { get; set; }
    }
}
