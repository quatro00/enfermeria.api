namespace enfermeria.api.Models.DTO
{
    public class FiltroGlobal
    {
        public bool IncluirInactivos { get; set; } = false;
        public string? Nombre { get; set; }
        //public string? Estado { get; set; }
        //public string? Especialidad { get; set; }
    }
}
