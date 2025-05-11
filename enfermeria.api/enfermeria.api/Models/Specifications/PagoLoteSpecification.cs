using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Interfaces;
using System.Linq.Expressions;

namespace enfermeria.api.Models.Specifications
{
    public class PagoLoteSpecification : ISpecification<enfermeria.api.Models.Domain.PagoLote>
    {
        public Expression<Func<enfermeria.api.Models.Domain.PagoLote, bool>> Criteria { get; }
        public List<string> IncludeStrings { get; set; }

        public PagoLoteSpecification(FiltroGlobal filtro)
        {
            Criteria = p =>
                (filtro.IncluirInactivos || p.Activo) &&
                (filtro.FechaInicio == null || p.FechaCreacion >= filtro.FechaInicio) &&
                (filtro.FechaFin == null || p.FechaCreacion <= filtro.FechaFin.Value.AddDays(1)) &&
                (filtro.ColaboradorAsignadoId == null || p.Pagos.Any(x => x.ServicioFecha != null && x.ServicioFecha.ColaboradorAsignadoId == filtro.ColaboradorAsignadoId)) &&


                (filtro.EstatusPagoLoteId == null || p.EstatosPagoLoteId == filtro.EstatusPagoLoteId);
        }
    }
}
