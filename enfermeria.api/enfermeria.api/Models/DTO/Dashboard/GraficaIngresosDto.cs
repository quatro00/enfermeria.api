namespace enfermeria.api.Models.DTO.Dashboard
{
    public class GraficaIngresosDto
    {
        public List<GraficaIngresosDetalleDto> Series { get; set; }
        public List<string> Meses { get; set; }
    }
    public class GraficaIngresosDetalleDto
    {
        public string name { get; set; }
        public List<decimal> data { get; set; }
        public string color { get; set; }
    }
}
