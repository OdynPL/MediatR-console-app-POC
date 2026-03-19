namespace PersonManager.Repositories
{
    using PersonManager.Data;
    using PersonManager.Domain;
    using PersonManager.DTO;
    using Microsoft.EntityFrameworkCore;
    using PersonManager.Specifications;

    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _db;
        public CompanyRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<RepositoryResult<Company>> AddAsync(Company company, CancellationToken cancellationToken = default)
        {
            try
            {
                await _db.Companies.AddAsync(company, cancellationToken);
                return RepositoryResult<Company>.Ok(company);
            }
            catch (Exception ex)
            {
                return RepositoryResult<Company>.Fail(ex.Message);
            }
        }
        public async Task<RepositoryResult<Company>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var company = await _db.Companies.FindAsync(new object[] { id }, cancellationToken);
                if (company == null)
                    return RepositoryResult<Company>.Fail("Company not found");
                return RepositoryResult<Company>.Ok(company);
            }
            catch (Exception ex)
            {
                return RepositoryResult<Company>.Fail(ex.Message);
            }
        }
        public async Task<RepositoryResult<List<Company>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var companies = await _db.Companies.ToListAsync(cancellationToken);
                return RepositoryResult<List<Company>>.Ok(companies);
            }
            catch (Exception ex)
            {
                return RepositoryResult<List<Company>>.Fail(ex.Message);
            }
        }
        public async Task<RepositoryResult<Company>> UpdateAsync(Company company, CancellationToken cancellationToken = default)
        {
            try
            {
                _db.Companies.Update(company);
                return RepositoryResult<Company>.Ok(company);
            }
            catch (Exception ex)
            {
                return RepositoryResult<Company>.Fail(ex.Message);
            }
        }
        public async Task<RepositoryResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var company = await _db.Companies.FindAsync(new object[] { id }, cancellationToken);
                if (company == null)
                    return RepositoryResult<bool>.Fail("Company not found");
                _db.Companies.Remove(company);
                return RepositoryResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return RepositoryResult<bool>.Fail(ex.Message);
            }
        }
        public IQueryable<Company> GetQueryable()
        {
            return _db.Companies.AsQueryable();
        }

        public async Task<RepositoryResult<List<Company>>> GetBySpecificationAsync(ISpecification<Company> specification, CancellationToken cancellationToken = default)
        {
            try
            {
                var companies = await _db.Companies
                    .Where(specification.Criteria)
                    .ToListAsync(cancellationToken);
                return RepositoryResult<List<Company>>.Ok(companies);
            }
            catch (Exception ex)
            {
                return RepositoryResult<List<Company>>.Fail(ex.Message);
            }
        }
    }
}

