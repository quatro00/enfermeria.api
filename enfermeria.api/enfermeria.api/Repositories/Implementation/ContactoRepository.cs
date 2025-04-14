using enfermeria.api.Data;
using enfermeria.api.Models.Domain;
using enfermeria.api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace enfermeria.api.Repositories.Implementation
{

    public class ContactoRepository : GenericRepository<Contacto>, IContactoRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<Contacto> _dbSet;

        public ContactoRepository(DbAb1c8aEnfermeriaContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Contacto>();
        }

        public async Task<List<Contacto>> GetByPaciente(Guid pacienteId)
        {
            return await _dbSet.Where(x=>x.PacienteId == pacienteId).ToListAsync();
        }
    }
}
