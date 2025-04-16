namespace enfermeria.api.Models.DTO.Colaborador
{
    public class ColaboradorDto
    {
        public Guid Id { get; set; }

        public int No { get; set; }

        public string Avatar { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string Apellidos { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public string CorreoElectronico { get; set; } = null!;

        public string Rfc { get; set; } = null!;

        public string Curp { get; set; } = null!;

        public string CedulaProfesional { get; set; } = null!;

        public string DomicilioCalle { get; set; } = null!;

        public string DomicilioNumero { get; set; } = null!;

        public string Cp { get; set; } = null!;

        public string Colonia { get; set; } = null!;

        public int BancoId { get; set; }

        public string Clabe { get; set; } = null!;

        public string Cuenta { get; set; } = null!;

        public int EstatusColaboradorId { get; set; }

        public Guid TipoEnfermeraId { get; set; }

        public bool? CuentaCreada { get; set; }
        public List<string> Estados { get; set; }
        public bool Activo { get; set; }
        public string Estatus { get; set; } = null;
    }
}
