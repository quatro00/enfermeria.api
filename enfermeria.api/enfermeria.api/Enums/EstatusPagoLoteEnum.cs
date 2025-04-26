using System.ComponentModel;

namespace enfermeria.api.Enums
{
    public enum EstatusPagoLoteEnum
    {
        [Description("Por pagar")]
        PorPagar = 1,

        [Description("Pagado")]
        Pagado = 2,

        [Description("Baja")]
        Cancelada = 99
    }
}
