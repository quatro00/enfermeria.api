namespace enfermeria.api.Models.DTO.Colaborador
{
    public class AdjuntarDocumentacionDto
    {
        public Guid Id { get; set; }
        public IFormFile Fotografia { get; set; }
        public IFormFile Identificacion { get; set; }
        public IFormFile ComprobanteDeDomicilio { get; set; }
        public IFormFile Titulo { get; set; }
        public IFormFile Cedula { get; set; }
        public IFormFile ContratoFirmado { get; set; }
    }
}
