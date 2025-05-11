namespace enfermeria.api.Models.DTO.Aviso
{
    public class CrearAvisoDto
    {
        public DateTime Vigencia { get; set; }
        public string titulo { get; set; }
        public string texto { get; set; }
        public string icono { get; set; }
        public string color { get; set; }
        public Guid? colaboradorId { get; set; }
    }
}
