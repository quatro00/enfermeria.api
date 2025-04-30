namespace enfermeria.api.Models.DTO.Servicio
{
    public class CrearServicioDto
    {
        public Guid pacienteId { get; set; }
        public string motivo {  get; set; }
        public Guid estadoId { get; set; }
        public Guid municipioId { get; set; }
        public string direccion {  get; set; }
        //--------------------
        public bool requiereAyudaBasica { get; set; }
        public string requiereAyudaBasicaDesc {  get; set; }
        public bool enfermedadDiagnosticada { get; set; }
        public string enfermedadDiagnosticadaDesc { get; set; }
        public bool tomaAlgunMedicamento { get; set; }
        public string tomaAlgunMedicamentoDesc { get; set; }
        //--------------------
        public bool requiereCuraciones { get; set; }
        public string requiereCuracionesDesc { get; set; }
        public bool cuentaConDispositivosMedicos { get; set; }
        public string cuentaConDispositivosMedicosDesc { get; set; }
        public bool requiereMonitoreo { get; set; }
        public string requiereMonitoreoDesc { get; set; }
        //--------------------
        public bool cuidadosNocturnos { get; set; }
        public string cuidadosNocturnosDesc { get; set; }
        public bool requiereAtencionNeurologica { get; set; }
        public string requiereAtencionNeurologicaDesc { get; set; }
        public bool cuidadosCriticos { get; set; }
        public string cuidadosCriticosDesc { get; set; }
        //-----------------------
        public int tipoLugarId { get; set; }
        public Guid tipoEnfermeraId { get; set; }
        public string observaciones { get; set; }
        public List<CrearServicioFechasDto> servicioFechasDtos { get; set; }
    }

    public class CrearServicioFechasDto
    {
        public string fecha { get; set; }
        public string inicio { get; set; }
        public string termino { get; set; }
        public decimal horas { get; set; }
    }

    public class CrearServicioFechasFormatoDto
    {
        public DateTime fechaInicio { get; set; }
        public DateTime fechaTermino { get; set; }
        public decimal cantidadHoras { get; set; }
        public decimal precioHora { get; set; }
        public decimal subTotal {  get; set; }
        public decimal descuentos { get; set; }
        public decimal total { get; set; }
    }
}
