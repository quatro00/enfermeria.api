using enfermeria.api.Data;
using enfermeria.api.Models.Domain;
using enfermeria.api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace enfermeria.api.Repositories.Implementation
{
    public class BancoRepository : GenericRepository<CatBanco>, IBancoRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<Colaborador> _dbSet;

        public BancoRepository(DbAb1c8aEnfermeriaContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Colaborador>();
        }

    }
}
