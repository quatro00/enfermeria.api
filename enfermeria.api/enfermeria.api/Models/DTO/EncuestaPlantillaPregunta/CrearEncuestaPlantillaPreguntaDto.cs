namespace enfermeria.api.Models.DTO.EncuestaPlantillaPregunta
{
    public class CrearEncuestaPlantillaPreguntaDto
    {
        public Guid PlantillaId { get; set; }
        public int Orden {  get; set; }
        public string Texto { get; set; }
        public string Tipo { get; set; }
        public bool Requerida { get; set; }
        public bool Activo { get; set; }
            
    }
}
