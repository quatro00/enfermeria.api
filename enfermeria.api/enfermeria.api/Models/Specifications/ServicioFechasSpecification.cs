using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Interfaces;
using System.Linq.Expressions;

namespace enfermeria.api.Models.Specifications
{
    public class ServicioFechasSpecification : ISpecification<ServicioFecha>
    {
        public Expression<Func<ServicioFecha, bool>> Criteria { get; }
        public List<string> IncludeStrings { get; set; }

        public ServicioFechasSpecification(FiltroGlobal filtro)
        {
            Criteria = p =>
                (filtro.IncluirInactivos || p.Activo) &&
                (filtro.ServicioId == null || p.ServicioId == filtro.ServicioId) &&
                (filtro.EstatusServicioFechaId == null || p.EstatusServicioFechaId == filtro.EstatusServicioFechaId);
        }
    }
}
