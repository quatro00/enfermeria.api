
namespace enfermeria.api.Models.DTO.Pago
{
    public class GetPagosColaboradorDto
    {
        public Guid Id { get; set; }
        public int Folio { get; set; }
        public string Motivo { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estatus { get; set; }
        public decimal Total { get; set; }
        public List<GetPagosColaboradorDetalleDto> Detalle { get; set; }
    }

    public class GetPagosColaboradorDetalleDto
    {
        public Guid Id { get; set; }
        public int No { get; set; }
        public Guid PagoLoteId { get; set; }
        public Guid ServicioFechaId { get; set; }
        public decimal ImporteBruto { get; set; }
        public decimal Comision { get; set; }
        public decimal Retencion { get; set; }
        public decimal CostoOperativo { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public decimal EstatusPagoId { get; set; }
        public decimal CantidadHoras { get; set; }
        public int NoGuardia { get; set; }
        public DateTime Fecha {  get; set; }

    }
}
