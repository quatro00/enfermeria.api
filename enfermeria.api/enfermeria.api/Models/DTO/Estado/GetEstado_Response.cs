namespace enfermeria.api.Models.DTO.Estado
{
    public class GetEstado_Response
    {
        public Guid id { get; set; }
        public string nombre { get; set; }
        public string nombreCorto { get; set; }
        public int activo { get; set; }
    }
}
