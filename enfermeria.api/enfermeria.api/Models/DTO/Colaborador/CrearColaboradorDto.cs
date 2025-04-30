using System.ComponentModel.DataAnnotations;

namespace enfermeria.api.Models.DTO.Colaborador
{
    public class CrearColaboradorDto
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellidos { get; set; }
        [Required]
        public string Telefono { get; set; }
        [Required]
        public string CorreoElectronico { get; set; }
        [Required]
        public string Rfc { get; set; }
        [Required]
        public string Curp { get; set; }
        [Required]
        public string CedulaProfesional { get; set; }
        [Required]
        public string DomicilioCalle { get; set; }
        [Required]
        public string DomicilioNumero { get; set; }
        [Required]
        public string Cp { get; set; }
        [Required]
        public string Colonia { get; set; }
        [Required]
        public int BancoId { get; set; }
        [Required]
        public string Clabe { get; set; }
        [Required]
        public string Cuenta { get; set; }
        [Required]
        public decimal Comision { get; set; }
        [Required]
        public Guid TipoEnfermeraId { get; set; }
        [Required]
        public List<Guid> Estados { get; set; }
    }
}
