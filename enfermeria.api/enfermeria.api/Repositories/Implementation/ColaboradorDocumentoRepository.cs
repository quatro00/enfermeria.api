using enfermeria.api.Data;
using enfermeria.api.Models.Domain;
using enfermeria.api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace enfermeria.api.Repositories.Implementation
{
    public class ColaboradorDocumentoRepository : GenericRepository<ColaboradorDocumento>, IColaboradorDocumentoRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<ColaboradorDocumento> _dbSet;

        public ColaboradorDocumentoRepository(DbAb1c8aEnfermeriaContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<ColaboradorDocumento>();
        }

    }
}
