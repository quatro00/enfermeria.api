using enfermeria.api.Models.Domain;

namespace enfermeria.api.Repositories.Interface
{
    public interface IColaboradorRepository : IGenericRepository<Colaborador>
    {
        Task<Colaborador> GetByUserIdAsync(string userId);
    }
}
