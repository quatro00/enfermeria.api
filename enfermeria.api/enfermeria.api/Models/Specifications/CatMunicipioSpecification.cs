using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Interfaces;
using System.Linq.Expressions;

namespace enfermeria.api.Models.Specifications
{
    public class CatMunicipioSpecification : ISpecification<CatMunicipio>
    {
        public Expression<Func<CatMunicipio, bool>> Criteria { get; }
        public List<string> IncludeStrings { get; set; }

        public CatMunicipioSpecification(FiltroGlobal filtro)
        {
            Criteria = p =>
                (filtro.IncluirInactivos || p.Activo) &&
                (filtro.EstadoId == null || p.EstadoId == filtro.EstadoId);
        }
    }
}
