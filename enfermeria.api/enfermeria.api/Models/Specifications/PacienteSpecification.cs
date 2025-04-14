using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Interfaces;
using System.Linq.Expressions;

namespace enfermeria.api.Models.Specifications
{
    public class PacienteSpecification : ISpecification<Paciente>
    {
        public Expression<Func<Paciente, bool>> Criteria { get; }

        public PacienteSpecification(FiltroGlobal filtro)
        {
            Criteria = p =>
                (filtro.IncluirInactivos || p.Activo) &&
                (string.IsNullOrEmpty(filtro.Nombre) || p.Nombre.Contains(filtro.Nombre)) &&
                (string.IsNullOrEmpty(filtro.Nombre) || p.Apellidos.Contains(filtro.Nombre)) &&
                (string.IsNullOrEmpty(filtro.CorreoElectronico) || p.CorreoElectronico.Contains(filtro.CorreoElectronico));
        }
    }
}
