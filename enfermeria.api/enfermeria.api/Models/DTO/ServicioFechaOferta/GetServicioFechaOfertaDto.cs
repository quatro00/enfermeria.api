namespace enfermeria.api.Models.DTO.ServicioFechaOferta
{
    public class GetServicioFechaOfertaDto
    {
        public Guid Id { get; set; }
        public string Colaborador { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTermino { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Tipo { get; set; }
        public string Comentario { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha {  get; set; }
    }
}
