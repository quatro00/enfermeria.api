namespace enfermeria.api.Models.DTO.Paciente
{
    public class UpdatePacienteDto
    {
        public Guid Id { get; set; }

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

        public bool Activo { get; set; }
    }
}
