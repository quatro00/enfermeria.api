using System.ComponentModel;

namespace enfermeria.api.Enums
{
    public enum EstatusServicioFechaEnum
    {
        [Description("Por asignar")]
        PorAsignar = 1,

        [Description("Asignada")]
        Asignada = 2,

        [Description("Completada")]
        Completada = 3,

        [Description("Proceso de pago")]
        ProcesoDePago = 4,

        [Description("Pagado")]
        Pagado = 4,

        [Description("Cancelada")]
        Cancelada = 99
    }
}
