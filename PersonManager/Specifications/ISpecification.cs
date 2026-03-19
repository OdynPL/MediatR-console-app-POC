using System.Linq.Expressions;

namespace PersonManager.Specifications
{
    // Specification pattern allows flexible, reusable, and testable filtering logic.
    // Instead of hardcoding filters in repositories, you define them as separate classes.
    // This makes code cleaner, easier to maintain, and enables combining filters.
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
    }
}
