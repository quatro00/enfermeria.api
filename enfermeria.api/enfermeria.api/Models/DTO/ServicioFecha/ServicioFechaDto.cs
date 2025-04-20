namespace enfermeria.api.Models.DTO.ServicioFecha
{
    public class ServicioFechaDto
    {
        public Guid Id { get; set; }
        public Guid ServicioId { get; set; }
        public Guid? ColaboradorAsignadoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTermino { get; set; }
        public string EstatusServicioFecha { get; set; }
        public int Ofertas { get; set; }
        public int No { get; set; }
    }
}
