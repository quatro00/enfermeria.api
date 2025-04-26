namespace enfermeria.api.Models.DTO.EncuestaPlantilla
{
    public class GetEncuestasPlantillaDto
    {
        public Guid Id { get; set; }
        public int No {  get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo {  get; set; }
    }
}
