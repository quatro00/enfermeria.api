namespace enfermeria.api.Models.DTO.ServicioFecha
{
    public class GetServicioFechaFiltrosResponse
    {
        public Guid Id { get; set; }
        public string Colaborador { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public decimal Horas { get; set; }
        public decimal ImporteBruto { get; set; }
        public decimal ImporteSolicitado { get; set; }
        public decimal Comision { get; set; }
        public decimal Descuento { get; set; }
        public decimal CostosOperativos { get; set; }
        public decimal Retenciones { get; set; }
        public decimal Total { get; set; }
        public string EstatusServicioFecha { get; set; }
    }
}
