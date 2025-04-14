namespace enfermeria.api.Models.DTO
{
    public class FiltroGlobal
    {
        public bool IncluirInactivos { get; set; } = false;
        public string? Nombre { get; set; }
        public string? CorreoElectronico { get; set; }
        //public string? Especialidad { get; set; }
    }
}
