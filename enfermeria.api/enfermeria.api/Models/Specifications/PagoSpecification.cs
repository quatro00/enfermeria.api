using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Interfaces;
using System.Linq.Expressions;

namespace enfermeria.api.Models.Specifications
{
    
    public class PagoSpecification : ISpecification<enfermeria.api.Models.Domain.Pago>
    {
        public Expression<Func<enfermeria.api.Models.Domain.Pago, bool>> Criteria { get; }
        public List<string> IncludeStrings { get; set; }

        public PagoSpecification(FiltroGlobal filtro)
        {
            Criteria = p =>
                (filtro.IncluirInactivos || p.Activo) &&
                (filtro.PagoLoteId == null || p.PagoLoteId == filtro.PagoLoteId) &&
                (filtro.ColaboradorAsignadoId == null || p.ServicioFecha.ColaboradorAsignadoId == filtro.ColaboradorAsignadoId) &&
                (filtro.FechaInicio == null || p.FechaCreacion >= filtro.FechaInicio) &&
                (filtro.FechaFin == null || p.FechaCreacion <= filtro.FechaFin.Value.AddDays(1))

                ;
        }
    }
}
