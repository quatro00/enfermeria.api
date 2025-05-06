using enfermeria.api.Data;
using enfermeria.api.Models.Domain;
using enfermeria.api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace enfermeria.api.Repositories.Implementation
{
    public class MensajeRepository : GenericRepository<Mensaje>, IMensajeRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<Mensaje> _dbSet;

        public MensajeRepository(DbAb1c8aEnfermeriaContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Mensaje>();
        }

    }
}
