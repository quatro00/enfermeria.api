using System.ComponentModel;

namespace enfermeria.api.Enums
{
    public enum EstatusPagoEnum
    {
        [Description("Por pagar")]
        PorPagar = 1,

        [Description("Pagado")]
        Pagado = 2,

        [Description("Baja")]
        Cancelada = 99
    }
}
