using enfermeria.api.Models.Domain;

namespace enfermeria.api.Models.DTO.Paciente
{
    public class CrearPacienteDto
    {
        public string Nombre { get; set; } = null!;

        public string Apellidos { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public string CorreoElectronico { get; set; } = null!;

        public DateTime FechaNacimiento { get; set; }

        public string Genero { get; set; } = null!;

        public decimal Peso { get; set; }

        public decimal Estatura { get; set; }

        public bool Discapacidad { get; set; }

        public string DescripcionDiscapacidad { get; set; } = null!;
        public List<CrearPacienteContactoDto> Contactos { get; set; }

    }

    public class CrearPacienteContactoDto
    {
        public Guid PacienteId { get; set; }

        public string Nombre { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public string CorreoElectronico { get; set; } = null!;

        public string Parentezco { get; set; } = null!;

    }
}
