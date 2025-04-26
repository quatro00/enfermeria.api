namespace enfermeria.api.Models.DTO.Pago
{
    public class GetDepositosDto
    {
        public Guid PagoLoteId { get; set; }
        public Guid ColaboradorId { get; set; }
        public string Banco {  get; set; }
        public string Clabe { get; set; }
        public string Beneficiario { get; set; }
        public decimal Monto { get; set; }
        public string Referencia { get; set; }
        public int Pagado { get; set; }
    }
}
