using System.Linq.Expressions;

namespace enfermeria.api.Models.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<string> IncludeStrings { get; set; }
    }
}
