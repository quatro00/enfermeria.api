namespace enfermeria.api.Models.DTO.Servicio
{
    public class EnviarCotizacionDto
    {
        public Guid ServicioFechaId {  get; set; }
        public string Comentario { get; set; }
        public decimal Monto { get; set; }
    }
}
