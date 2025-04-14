using enfermeria.api.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace enfermeria.api.Models.DTO.Paciente
{
    public class CrearPacienteDto
    {
        [Required]
        public string Nombre { get; set; } = null!;
        [Required]
        public string Apellidos { get; set; } = null!;
        [Required]
        public string Telefono { get; set; } = null!;
        [Required]
        public string CorreoElectronico { get; set; } = null!;
        [Required]
        public DateTime FechaNacimiento { get; set; }
        [Required]
        public string Genero { get; set; } = null!;
        [Required]
        public decimal Peso { get; set; }
        [Required]
        public decimal Estatura { get; set; }
        [Required]
        public bool Discapacidad { get; set; }
        [Required]
        public string DescripcionDiscapacidad { get; set; } = null!;
        [Required]
        public List<CrearPacienteContactoDto> Contactos { get; set; }

    }

    public class CrearPacienteContactoDto
    {
        [Required]
        public Guid PacienteId { get; set; }
        [Required]
        public string Nombre { get; set; } = null!;
        [Required]
        public string Telefono { get; set; } = null!;
        [Required]
        public string CorreoElectronico { get; set; } = null!;
        [Required]
        public string Parentezco { get; set; } = null!;

    }
}
