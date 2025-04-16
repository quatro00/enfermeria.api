using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Interfaces;
using System.Linq.Expressions;

namespace enfermeria.api.Models.Specifications
{
    public class ColaboradorSpecification : ISpecification<Colaborador>
    {
        public Expression<Func<Colaborador, bool>> Criteria { get; }
        public List<string> IncludeStrings { get; set; }

        public ColaboradorSpecification(FiltroGlobal filtro)
        {
            Criteria = p =>
                (filtro.IncluirInactivos || p.Activo) &&
                (string.IsNullOrEmpty(filtro.Nombre) || p.Nombre.Contains(filtro.Nombre)) &&
                (string.IsNullOrEmpty(filtro.Nombre) || p.Apellidos.Contains(filtro.Nombre)) &&
                (string.IsNullOrEmpty(filtro.Telefono) || p.Telefono.Contains(filtro.Telefono)) &&
                (string.IsNullOrEmpty(filtro.TipoEnfermeraId) || p.TipoEnfermeraId == Guid.Parse(filtro.TipoEnfermeraId)) &&
                (string.IsNullOrEmpty(filtro.CorreoElectronico) || p.CorreoElectronico.Contains(filtro.CorreoElectronico));
        }
    }
}
