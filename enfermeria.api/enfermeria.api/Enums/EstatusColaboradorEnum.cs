using System.ComponentModel;

namespace enfermeria.api.Enums
{
    public enum EstatusColaboradorEnum
    {
        [Description("Pre Registro")]
        PreRegistro = 1,

        [Description("Registro completo")]
        RegistroCompleto = 2,

        [Description("Activo")]
        Activo = 3,

        [Description("Suspendido")]
        Suspendido = 4,

        [Description("Baja")]
        Baja = 5
    }
}
