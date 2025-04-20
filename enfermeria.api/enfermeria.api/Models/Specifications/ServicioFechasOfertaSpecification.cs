using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Interfaces;
using System.Linq.Expressions;

namespace enfermeria.api.Models.Specifications
{
    public class ServicioFechasOfertaSpecification : ISpecification<ServicioFechasOfertum>
    {
        public Expression<Func<ServicioFechasOfertum, bool>> Criteria { get; }
        public List<string> IncludeStrings { get; set; }

        public ServicioFechasOfertaSpecification(FiltroGlobal filtro)
        {
            Criteria = p =>
                (filtro.IncluirInactivos || p.Activo) &&
                (filtro.ServicioFechaId == null || p.ServicioFechaId == filtro.ServicioFechaId) &&
                (filtro.EstatusOfertaId == null || p.EstatusOfertaId == filtro.EstatusOfertaId);
        }
    }
}
