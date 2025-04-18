namespace enfermeria.api.Helpers.Cotizacion
{
    public class GetCotizacionResult
    {
        public Guid Id { get; set; }
        public int No {  get; set; }
        public string Paciente { get; set; }
        public string Estado { get; set; }
        public string Direccion { get; set; }
        public string Motivo { get; set; }
        public string TipoEnfermera { get; set; }
        public decimal Horas { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public string Estatus { get; set; }
    }
}
