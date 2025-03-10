namespace enfermeria.api.Models.DTO.TipoEnfermera
{
    public class GetTipoEnfermera_Response
    {
        public Guid id { get; set; }
        public int no {  get; set; }
        public string descripcion { get; set; }
        public int valor { get; set; }
        public decimal costoHora { get; set; }
        public int activo { get; set; }
    }
}
