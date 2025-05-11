namespace enfermeria.api.Models.DTO.Auth
{
    public class GetPerfilDto
    {
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string Rfc {  get; set; }
        public string Curp { get; set; }
        public string CedulaProfesional { get; set; }
        public string Domicilio { get; set; }
        public string Banco {  get; set; }
        public string Clabe { get; set; }
        public string CuentaBancaria { get; set; }
        public List<string> Estados { get; set; }
    }
}
