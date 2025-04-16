using enfermeria.api.Models.Interfaces;
using System.Linq.Expressions;

namespace enfermeria.api.Models.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; }
        public List<string> IncludeStrings { get; set; }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
    }
}
