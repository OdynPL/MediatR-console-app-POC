using MediatRApp.Data;
using MediatRApp.Domain;
using MediatRApp.DTO;
using Microsoft.EntityFrameworkCore;

namespace MediatRApp.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _db;
        public PersonRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<RepositoryResult<Person>> AddAsync(Person person, CancellationToken cancellationToken = default)
        {
            try
            {
                await _db.People.AddAsync(person, cancellationToken);
                return RepositoryResult<Person>.Ok(person);
            }
            catch (Exception ex)
            {
                return RepositoryResult<Person>.Fail(ex.Message);
            }
        }

        public async Task<RepositoryResult<Person>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var person = await _db.People.FindAsync(new object[] { id }, cancellationToken);
                if (person == null)
                    return RepositoryResult<Person>.Fail("Person not found");
                return RepositoryResult<Person>.Ok(person);
            }
            catch (Exception ex)
            {
                return RepositoryResult<Person>.Fail(ex.Message);
            }
        }

        public async Task<RepositoryResult<List<Person>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var people = await Task.Run(() => _db.People.ToList(), cancellationToken);
                return RepositoryResult<List<Person>>.Ok(people);
            }
            catch (Exception ex)
            {
                return RepositoryResult<List<Person>>.Fail(ex.Message);
            }
        }

        public async Task<RepositoryResult<Person>> UpdateAsync(Person person, CancellationToken cancellationToken = default)
        {
            try
            {
                person.RowVersion++;
                _db.People.Update(person);
                await _db.SaveChangesAsync(cancellationToken);
                return RepositoryResult<Person>.Ok(person);
            }
            catch (DbUpdateConcurrencyException)
            {
                return RepositoryResult<Person>.Fail("Concurrency conflict: record was modified by another user.");
            }
            catch (Exception ex)
            {
                return RepositoryResult<Person>.Fail(ex.Message);
            }
        }

        public async Task<RepositoryResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await GetByIdAsync(id, cancellationToken);
                if (!result.Success || result.Data == null)
                    return RepositoryResult<bool>.Fail(result.ErrorMessage ?? "Person not found");
                _db.People.Remove(result.Data);
                await _db.SaveChangesAsync(cancellationToken);
                return RepositoryResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return RepositoryResult<bool>.Fail(ex.Message);
            }
        }

        public IQueryable<Person> GetQueryable()
        {
                return _db.People.AsQueryable();
        }
    }
}
