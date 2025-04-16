using System.ComponentModel;

namespace enfermeria.api.Enums
{
    public enum TipoDocumentoEnum
    {
        [Description("TITULO")]
        Titulo = 1,
        [Description("IDENTIFICACION")]
        Identificacion = 2,
        [Description("COMPROBANTE DE DOMICILIO")]
        ComprobanteDeDomicilio = 3,
        [Description("CEDULA PROFESIONAL")]
        CedulaProfesional = 4,
        [Description("CONTRATO FIRMADO")]
        ContratoFirmado = 5,
        [Description("FOTOGRAFIA")]
        Fotografia = 6,
        [Description("OTRO")]
        Otro = 99,
    }
}
