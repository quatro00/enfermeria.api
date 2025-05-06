namespace enfermeria.api.Models.DTO
{
    public class FiltroGlobal
    {
        public bool IncluirInactivos { get; set; } = false;
        public string? Nombre { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Telefono { get; set; }
        public string? TipoEnfermeraId { get; set; }
        public Guid? EstadoId { get; set; }
        public Guid? MunicipioId { get; set; }
        public int? EstatusServicioId { get; set; }
        public string? noServicio { get; set; }
        public Guid? ServicioId { get; set; }
        public Guid? ServicioFechaId { get; set; }
        public int? EstatusOfertaId { get; set; }
        public int? EstatusServicioFechaId { get; set; }

        public int? NoServicio { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public Guid? ColaboradorAsignadoId { get; set; }
        public DateTime? Periodo { get; set; }
        public int? EstatusPagoLoteId { get; set; }
        public Guid? PagoLoteId { get; set; }

        public DateTime? FechaPagoInicio { get; set; }
        public DateTime? FechaPagoFin { get; set; }
    }
}
