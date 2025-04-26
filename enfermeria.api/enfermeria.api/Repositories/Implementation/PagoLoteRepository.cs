using enfermeria.api.Data;
using enfermeria.api.Models.Domain;
using enfermeria.api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace enfermeria.api.Repositories.Implementation
{
    public class PagoLoteRepository : GenericRepository<PagoLote>, IPagoLoteRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<PagoLote> _dbSet;

        public PagoLoteRepository(DbAb1c8aEnfermeriaContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<PagoLote>();
        }

    }
}
