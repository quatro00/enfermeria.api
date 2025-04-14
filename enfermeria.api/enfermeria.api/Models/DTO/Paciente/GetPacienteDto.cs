namespace enfermeria.api.Models.DTO.Paciente
{
    public class GetPacienteDto
    {
        public Guid Id { get; set; }

        public int No { get; set; }

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

        // Puedes incluir solo lo que quieres exponer, por ejemplo:
        //public List<GetPacienteContactoDto> Contactos { get; set; }
    }

    //public class GetPacienteContactoDto
    //{
    //    public string Nombre { get; set; }
    //    public string Telefono { get; set; }
    //    public string Relacion { get; set; }
    //}
}
