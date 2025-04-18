namespace enfermeria.api.Models.DTO.Servicio
{
    public class FilterGetServicio
    {
        public string? No { get; set; }
        public string? NombrePaciente { get; set; }
        public Guid? Estado { get; set; }
        public int? Estatus { get; set; }
    }
}
