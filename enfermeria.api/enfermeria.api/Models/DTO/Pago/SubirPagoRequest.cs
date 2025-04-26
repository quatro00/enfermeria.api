namespace enfermeria.api.Models.DTO.Pago
{
    public class SubirPagoRequest
    {
        public Guid PagoLoteId { get; set; }
        public Guid ColaboradorId { get; set; }
        public decimal Monto { get; set; }
        public string Referencia { get; set; }
        public IFormFile Documento { get; set; }
    }
}
