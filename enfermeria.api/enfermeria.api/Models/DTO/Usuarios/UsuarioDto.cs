namespace enfermeria.api.Models.DTO.Usuarios
{
    public class UsuarioDto
    {
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public string? correo { get; set; }
        public string? cargo { get; set; }
        public string? telefono { get; set; }
        public string? ciudad { get; set; }
        public string? descripcion { get; set; }
        public string? tipo { get; set; }
        public bool activo { get; set; }
    }
}
