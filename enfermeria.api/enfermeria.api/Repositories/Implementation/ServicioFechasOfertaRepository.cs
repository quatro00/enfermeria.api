using enfermeria.api.Data;
using enfermeria.api.Models.Domain;
using enfermeria.api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace enfermeria.api.Repositories.Implementation
{
    public class ServicioFechasOfertaRepository : GenericRepository<ServicioFechasOfertum>, IServicioFechasOfertaRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<ServicioFechasOfertum> _dbSet;

        public ServicioFechasOfertaRepository(DbAb1c8aEnfermeriaContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<ServicioFechasOfertum>();
        }

    }
}
