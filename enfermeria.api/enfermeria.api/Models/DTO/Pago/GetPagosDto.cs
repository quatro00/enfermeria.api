namespace enfermeria.api.Models.DTO.Pago
{
    public class GetPagosDto
    {
        public Guid Id { get; set; }
        public int No { get; set; }
        public string Referencia { get; set; }
        public string Beneficiario { get; set; }
        public decimal Monto { get; set; }
        public DateTime? FechaPago { get; set; }
        public string EstatusPago { get; set; }
    }
}
