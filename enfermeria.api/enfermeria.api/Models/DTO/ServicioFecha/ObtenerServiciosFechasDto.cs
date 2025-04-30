namespace enfermeria.api.Models.DTO.ServicioFecha
{
    public class ObtenerServiciosFechasDto
    {
        public Guid Id { get; set; }

        public Guid ServicioId { get; set; }

        public Guid? ColaboradorAsignadoId { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaTermino { get; set; }

        public int EstatusServicioFechaId { get; set; }

        public decimal CantidadHoras { get; set; }

        public decimal ImporteBruto { get; set; }

        public decimal ImporteSolicitado { get; set; }

        public decimal Comision { get; set; }

        public decimal CostosOperativos { get; set; }

        public decimal Retenciones { get; set; }

        public decimal Descuento { get; set; }

        public decimal Total { get; set; }
    }
}
