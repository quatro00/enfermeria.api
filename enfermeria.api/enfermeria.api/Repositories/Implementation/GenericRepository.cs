using enfermeria.api.Data;
using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Interfaces;
using enfermeria.api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace enfermeria.api.Repositories.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<T>> ListAsync(ISpecification<T> spec)
        {
            return await _dbSet.Where(spec.Criteria).ToListAsync();
        }

        public async Task<PaginatedResult<T>> ListAsync(ISpecification<T> spec, PaginationFilter pagination)
        {
            var query = _dbSet.Where(spec.Criteria);

            query = ApplyOrdering(query, pagination.OrderBy, pagination.Descending);

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginatedResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<PaginatedResult<TResult>> ListAsync<TResult>(
            ISpecification<T> spec,
            PaginationFilter pagination,
            Expression<Func<T, TResult>> selector)
        {
            var query = _dbSet.Where(spec.Criteria);

            query = ApplyOrdering(query, pagination.OrderBy, pagination.Descending);

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(selector)
                .ToListAsync();

            return new PaginatedResult<TResult>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        private IQueryable<T> ApplyOrdering(IQueryable<T> query, string? orderBy, bool descending)
        {
            if (string.IsNullOrWhiteSpace(orderBy)) return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, orderBy);
            var lambda = Expression.Lambda(property, parameter);

            string method = descending ? "OrderByDescending" : "OrderBy";
            var expression = Expression.Call(typeof(Queryable), method,
                new Type[] { query.ElementType, property.Type },
                query.Expression, Expression.Quote(lambda));

            return query.Provider.CreateQuery<T>(expression);
        }
    }

}
