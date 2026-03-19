using PersonManager.Domain;
using PersonManager.UnitOfWork;

namespace PersonManager.Services
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DTO.RepositoryResult<Person>> AddPersonAsync(Person person, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.PersonRepository.AddAsync(person, cancellationToken);
            if (!result.Success)
                return result;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<DTO.RepositoryResult<Person>> GetPersonByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.PersonRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<DTO.RepositoryResult<List<Person>>> GetAllPersonsAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.PersonRepository.GetAllAsync(cancellationToken);
        }

        public async Task<DTO.RepositoryResult<Person>> UpdatePersonAsync(Person person, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.PersonRepository.UpdateAsync(person, cancellationToken);
            if (!result.Success)
                return result;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<DTO.RepositoryResult<bool>> DeletePersonAsync(int id, CancellationToken cancellationToken = default)
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
