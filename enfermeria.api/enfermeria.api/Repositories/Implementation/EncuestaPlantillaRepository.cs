using enfermeria.api.Data;
using enfermeria.api.Models.Domain;
using enfermeria.api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace enfermeria.api.Repositories.Implementation
{
    public class EncuestaPlantillaRepository : GenericRepository<EncuestaPlantilla>, IEncuestaPlantillaRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<Colaborador> _dbSet;

        public EncuestaPlantillaRepository(DbAb1c8aEnfermeriaContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Colaborador>();
        }

    }
}
