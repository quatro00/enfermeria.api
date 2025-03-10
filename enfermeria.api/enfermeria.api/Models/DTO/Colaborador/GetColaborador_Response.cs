namespace enfermeria.api.Models.DTO.Colaborador
{
    public class GetColaborador_Response
    {
        public Guid id { get; set; }
        public string no { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string telefono { get; set; }
        public string correoElectronico { get; set; }
        public string rfc { get; set; }
        public string curp { get; set; }
        public string cedulaProfesional { get; set; }
        public string domicilioCalle { get; set; }
        public string domicilioNumero { get; set; }
        public string cp { get; set; }
        public string colonia { get; set; }
        public string banco { get; set; }
        public string clabe { get; set; }
        public string cuenta { get; set; }
        public int estatusColaboradorId { get; set; }
        public string estatusColaborador {  get; set; }
        public Guid tipoEnfermeraId { get; set; }
        public string tipoEnfermera {  get; set; }
        public List<Guid> estados { get; set; }
        public int activo { get; set; }
    }
}
