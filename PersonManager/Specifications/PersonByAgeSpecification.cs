using System.Linq.Expressions;
using PersonManager.Domain;

namespace PersonManager.Specifications
{
    public class PersonByAgeSpecification : ISpecification<Person>
    {
        public Expression<Func<Person, bool>> Criteria { get; }

        public PersonByAgeSpecification(int minAge)
        {
            Criteria = p => p.Age >= minAge;
        }
    }
}
