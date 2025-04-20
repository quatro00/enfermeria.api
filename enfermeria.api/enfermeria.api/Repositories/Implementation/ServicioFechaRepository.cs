using enfermeria.api.Data;
using enfermeria.api.Models.Domain;
using enfermeria.api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace enfermeria.api.Repositories.Implementation
{
    public class ServicioFechaRepository : GenericRepository<ServicioFecha>, IServicioFechaRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<ServicioFecha> _dbSet;

        public ServicioFechaRepository(DbAb1c8aEnfermeriaContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<ServicioFecha>();
        }

    }
}
