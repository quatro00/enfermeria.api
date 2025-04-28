namespace enfermeria.api.Models.DTO.Dashboard
{
    public class GraficoTipoLugar
    {
        public List<string> labels { get; set; }
        public List<decimal> series { get; set; }
    }

    public class GraficoDonut
    {
        public List<string> labels { get; set; }
        public List<decimal> series { get; set; }
    }
}
