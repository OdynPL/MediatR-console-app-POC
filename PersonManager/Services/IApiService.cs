
namespace PersonManager.Services
{
    public interface IApiService<T>
    {
        Task<DTO.ApiResult<T>> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<DTO.ApiResult<IEnumerable<T>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<DTO.ApiResult<T>> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<DTO.ApiResult<T>> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<DTO.ApiResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
