namespace enfermeria.api.Models.DTO.EncuestaPlantillaPregunta
{
    public class GetEncuestaPlantillaPreguntaDto
    {
        public Guid Id { get; set; }
        public string Orden {  get; set; }
        public string Texto { get; set; }
        public string Tipo { get; set; }
        public bool Requerida { get; set; }
        public bool Activo { get; set; }
    }
}
