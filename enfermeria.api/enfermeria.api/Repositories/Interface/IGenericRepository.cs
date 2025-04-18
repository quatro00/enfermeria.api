using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Interfaces;
using System.Linq.Expressions;

namespace enfermeria.api.Repositories.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetByIdAsync(Guid id, string idFieldName, params string[] includePaths);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(Guid id);

        Task<List<T>> ListAsync(ISpecification<T> spec);
        Task<List<T>> ListAsync();

        Task<PaginatedResult<T>> ListAsync(ISpecification<T> spec, PaginationFilter pagination);

        Task<PaginatedResult<TResult>> ListAsync<TResult>(
            ISpecification<T> spec,
            PaginationFilter pagination,
            Expression<Func<T, TResult>> selector);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}
