using enfermeria.api.Models.DTO;
using enfermeria.api.Models.Interfaces;
using System.Linq.Expressions;

namespace enfermeria.api.Repositories.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(Guid id);

        Task<List<T>> ListAsync(ISpecification<T> spec);

        Task<PaginatedResult<T>> ListAsync(ISpecification<T> spec, PaginationFilter pagination);

        Task<PaginatedResult<TResult>> ListAsync<TResult>(
            ISpecification<T> spec,
            PaginationFilter pagination,
            Expression<Func<T, TResult>> selector);
    }
}
