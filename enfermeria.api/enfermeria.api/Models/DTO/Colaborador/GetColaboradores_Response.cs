namespace enfermeria.api.Models.DTO.Colaborador
{
    public class GetColaboradores_Response
    {
        public Guid id { get; set; }
        public string no {  get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string correoElectronico { get; set; }
        public string rfc { get; set; }
        public string curp { get; set; }
        public string cedula { get; set; }
        public string domicilio { get; set; }
        public List<string> estados { get; set; }
        public int activo { get; set; }
        public string estatus { get; set; }
        public string tipo { get; set; }

    }
}
