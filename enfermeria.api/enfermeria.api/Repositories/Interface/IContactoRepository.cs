using enfermeria.api.Models.Domain;
using enfermeria.api.Models.Interfaces;

namespace enfermeria.api.Repositories.Interface
{
    public interface IContactoRepository : IGenericRepository<Contacto>
    {
        Task<List<Contacto>> GetByPaciente(Guid pacienteId);
    }
}
