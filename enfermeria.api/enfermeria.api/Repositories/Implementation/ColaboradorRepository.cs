﻿using enfermeria.api.Data;
using enfermeria.api.Models.Domain;
using enfermeria.api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace enfermeria.api.Repositories.Implementation
{
    public class ColaboradorRepository : GenericRepository<Colaborador>, IColaboradorRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<Colaborador> _dbSet;

        public ColaboradorRepository(DbAb1c8aEnfermeriaContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Colaborador>();
        }

        public async Task<Colaborador> GetByUserIdAsync(string userId)
        {
            return await _dbSet.Where(x=>x.UserId.ToUpper() == userId.ToUpper()).FirstOrDefaultAsync();
        }
    }
}
