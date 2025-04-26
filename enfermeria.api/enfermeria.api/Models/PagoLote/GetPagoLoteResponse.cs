namespace enfermeria.api.Models.PagoLote
{
    public class GetPagoLoteResponse
    {
        public Guid Id { get; set; }
        public int No { get; set; }
        public DateTime FechaCreacion { get; set; }
        public decimal TotalLote { get; set; }
        public int NumeroPagos { get; set; }
        public int Colaboradores {  get; set; }
        public string EstatusPagoLote { get; set; }
    }
}
