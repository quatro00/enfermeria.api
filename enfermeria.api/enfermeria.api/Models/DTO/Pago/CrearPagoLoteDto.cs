namespace enfermeria.api.Models.DTO.Pago
{
    public class CrearPagoLoteDto
    {
        public string Concepto { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin {  get; set; }
        public List<Guid> Pagos { get; set; }
    }
}
