using PersonManager.Data;
using PersonManager.Domain;
using PersonManager.DTO;
using Microsoft.EntityFrameworkCore;
public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _db;
    public ProjectRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<RepositoryResult<Project>> AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        try
        {
            await _db.Projects.AddAsync(project, cancellationToken);
            return RepositoryResult<Project>.Ok(project);
        }
        catch (Exception ex)
        {
            return RepositoryResult<Project>.Fail(ex.Message);
        }
    }
    public async Task<RepositoryResult<Project>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var project = await _db.Projects.FindAsync(new object[] { id }, cancellationToken);
            if (project == null)
                return RepositoryResult<Project>.Fail("Project not found");
            return RepositoryResult<Project>.Ok(project);
        }
        catch (Exception ex)
        {
            return RepositoryResult<Project>.Fail(ex.Message);
        }
    }
    public async Task<RepositoryResult<List<Project>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var projects = await _db.Projects.ToListAsync(cancellationToken);
            return RepositoryResult<List<Project>>.Ok(projects);
        }
        catch (Exception ex)
        {
            return RepositoryResult<List<Project>>.Fail(ex.Message);
        }
    }
    public async Task<RepositoryResult<Project>> UpdateAsync(Project project, CancellationToken cancellationToken = default)
    {
        try
        {
            _db.Projects.Update(project);
            return RepositoryResult<Project>.Ok(project);
        }
        catch (Exception ex)
        {
            return RepositoryResult<Project>.Fail(ex.Message);
        }
    }
    public async Task<RepositoryResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var project = await _db.Projects.FindAsync(new object[] { id }, cancellationToken);
            if (project == null)
                return RepositoryResult<bool>.Fail("Project not found");
            _db.Projects.Remove(project);
            return RepositoryResult<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return RepositoryResult<bool>.Fail(ex.Message);
        }
    }
    public IQueryable<Project> GetQueryable()
    {
        return _db.Projects.AsQueryable();
    }
}

