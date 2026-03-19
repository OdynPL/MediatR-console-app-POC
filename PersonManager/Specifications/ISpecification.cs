using System;
using System.Linq.Expressions;

namespace PersonManager.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
    }
}
