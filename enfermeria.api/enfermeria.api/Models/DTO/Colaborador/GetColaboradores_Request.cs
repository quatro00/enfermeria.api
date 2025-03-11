namespace enfermeria.api.Models.DTO.Colaborador
{
    public class GetColaboradores_Request
    {
        public string? nombre {  get; set; }
        public string? correoElectronico { get; set; }
        public string? telefono { get; set; }
        public string? tipo { get; set; }
    }
}
