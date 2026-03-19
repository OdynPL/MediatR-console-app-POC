using PersonManager.Domain;

namespace PersonManager.Services
{
    public interface IPersonService
    {
        Task<DTO.RepositoryResult<Person>> AddPersonAsync(Person person, CancellationToken cancellationToken = default);
        Task<DTO.RepositoryResult<Person>> GetPersonByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<DTO.RepositoryResult<List<Person>>> GetAllPersonsAsync(CancellationToken cancellationToken = default);
        Task<DTO.RepositoryResult<Person>> UpdatePersonAsync(Person person, CancellationToken cancellationToken = default);
        Task<DTO.RepositoryResult<bool>> DeletePersonAsync(int id, CancellationToken cancellationToken = default);
        IQueryable<PersonManager.Domain.Person> GetQueryablePersons();
    }
}
