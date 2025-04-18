using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Interfaces;
using System.Linq.Expressions;

namespace enfermeria.api.Models.Specifications
{
    public class ServicioSpecification : ISpecification<Servicio>
    {
        public Expression<Func<Servicio, bool>> Criteria { get; }
        public List<string> IncludeStrings { get; set; }

        public ServicioSpecification(FiltroGlobal filtro)
        {
            Criteria = p =>
                (string.IsNullOrEmpty(filtro.Nombre) || p.Paciente.Nombre.Contains(filtro.Nombre)) &&
                (string.IsNullOrEmpty(filtro.Nombre) || p.Paciente.Apellidos.Contains(filtro.Nombre)) &&
                (string.IsNullOrEmpty(filtro.noServicio) || p.No.ToString().ToUpper().Contains(filtro.noServicio)) &&
                (filtro.EstadoId == null || p.EstadoId == filtro.EstadoId) &&
                (filtro.EstatusServicioId == null || p.EstatusServicioId == filtro.EstatusServicioId);
        }
    }
}
