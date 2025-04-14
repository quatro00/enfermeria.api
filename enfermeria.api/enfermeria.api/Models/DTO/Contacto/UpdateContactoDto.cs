namespace enfermeria.api.Models.DTO.Contacto
{
    public class UpdateContactoDto
    {
        public Guid Id { get; set; }

        public Guid PacienteId { get; set; }

        public string Nombre { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public string CorreoElectronico { get; set; } = null!;

        public string Parentezco { get; set; } = null!;

        public bool Activo { get; set; }
    }
}
