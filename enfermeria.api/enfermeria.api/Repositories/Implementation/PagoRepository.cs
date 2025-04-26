using enfermeria.api.Data;
using enfermeria.api.Models.Domain;
using enfermeria.api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace enfermeria.api.Repositories.Implementation
{
    public class PagoRepository : GenericRepository<Pago>, IPagoRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<Pago> _dbSet;

        public PagoRepository(DbAb1c8aEnfermeriaContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Pago>();
        }

    }
}
