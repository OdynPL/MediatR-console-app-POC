using PersonManager.Domain;
using PersonManager.DTO;
using PersonManager.UnitOfWork;

namespace PersonManager.Services
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        public PersonService(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<RepositoryResult<Person>> AddPersonAsync(Person person, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"Dodawanie osoby: {person.Name}");
            var result = await _unitOfWork.PersonRepository.AddAsync(person, cancellationToken);
            if (!result.Success)
            {
                _logger.LogWarning($"Nieudane dodanie osoby: {person.Name}");
                return result;
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInfo($"Osoba dodana: {person.Name}");
            return result;
        }

        public async Task<RepositoryResult<Person>> GetPersonByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.PersonRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<RepositoryResult<List<Person>>> GetAllPersonsAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.PersonRepository.GetAllAsync(cancellationToken);
        }

        public async Task<RepositoryResult<Person>> UpdatePersonAsync(Person person, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.PersonRepository.UpdateAsync(person, cancellationToken);
            if (!result.Success)
                return result;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<RepositoryResult<bool>> DeletePersonAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.PersonRepository.DeleteAsync(id, cancellationToken);
            if (!result.Success)
                return result;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }

        public IQueryable<Person> GetQueryablePersons()
        {
            return _unitOfWork.PersonRepository.GetQueryable();
        }
    }
}
