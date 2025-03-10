namespace enfermeria.api.Models.DTO.TipoEnfermera
{
    public class UpdateTipoEnfermera_Request
    {
        public int no { get; set; }
        public string descripcion { get; set; }
        public int valor { get; set; }
        public decimal costoHora { get; set; }
    }
}
