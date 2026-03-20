using System.Net.Http.Json;
using PersonManager.DTO;
using Polly;

namespace PersonManager.Services
{
    public class RestApiService<T> : IApiService<T>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _clientName;
        private readonly string _endpoint;

        public RestApiService(IHttpClientFactory httpClientFactory, string clientName, string endpoint)
        {
            _httpClientFactory = httpClientFactory;
            _clientName = clientName;
            _endpoint = endpoint;
        }

        public async Task<ApiResult<T>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(_clientName);
                var response = await client.GetAsync($"{_endpoint}/{id}", cancellationToken);
                if (!response.IsSuccessStatusCode)
                    return ApiResult<T>.Fail($"REST: Błąd pobierania: {response.StatusCode}");
                var item = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
                if (item == null)
                    return ApiResult<T>.Fail($"REST: Nie znaleziono obiektu o id {id}");
                return ApiResult<T>.Ok(item);
            }
            catch (Exception ex)
            {
                // Tu można dodać logowanie
                return ApiResult<T>.Fail($"REST: Wyjątek: {ex.Message}");
            }
        }
        public async Task<ApiResult<IEnumerable<T>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(_clientName);
                client.Timeout = TimeSpan.FromSeconds(10);
                var retryPolicy = Policy
                    .Handle<HttpRequestException>()
                    .Or<TaskCanceledException>()
                    .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(2));

                HttpResponseMessage response = null;
                await retryPolicy.ExecuteAsync(async () =>
                {
                    response = await client.GetAsync(_endpoint, cancellationToken);
                });
                if (response == null || !response.IsSuccessStatusCode)
                    return ApiResult<IEnumerable<T>>.Fail($"REST: Błąd pobierania listy: {(response != null ? response.StatusCode.ToString() : "Brak odpowiedzi")}");
                var items = await response.Content.ReadFromJsonAsync<IEnumerable<T>>(cancellationToken: cancellationToken) ?? new List<T>();
                return ApiResult<IEnumerable<T>>.Ok(items);
            }
            catch (Exception ex)
            {
                return ApiResult<IEnumerable<T>>.Fail($"REST: Wyjątek: {ex.Message}");
            }
        }

        public async Task<ApiResult<T>> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(_clientName);
                var response = await client.PostAsJsonAsync(_endpoint, entity, cancellationToken);
                if (!response.IsSuccessStatusCode)
                    return ApiResult<T>.Fail($"REST: Błąd dodawania: {response.StatusCode}");
                var item = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
                if (item == null)
                    return ApiResult<T>.Fail("REST: Nie udało się dodać obiektu");
                return ApiResult<T>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResult<T>.Fail($"REST: Wyjątek: {ex.Message}");
            }
        }

        public async Task<ApiResult<T>> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(_clientName);
                var idProp = typeof(T).GetProperty("Id");
                if (idProp == null)
                    return ApiResult<T>.Fail("REST: Typ T nie posiada właściwości Id");
                var id = idProp.GetValue(entity);
                var response = await client.PutAsJsonAsync($"{_endpoint}/{id}", entity, cancellationToken);
                if (!response.IsSuccessStatusCode)
                    return ApiResult<T>.Fail($"REST: Błąd aktualizacji: {response.StatusCode}");
                var item = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
                if (item == null)
                    return ApiResult<T>.Fail($"REST: Nie udało się zaktualizować obiektu o id {id}");
                return ApiResult<T>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResult<T>.Fail($"REST: Wyjątek: {ex.Message}");
            }
        }

        public async Task<ApiResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(_clientName);
                var response = await client.DeleteAsync($"{_endpoint}/{id}", cancellationToken);
                if (!response.IsSuccessStatusCode)
                    return ApiResult<bool>.Fail($"REST: Błąd usuwania obiektu {id}: {response.StatusCode}");
                return ApiResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return ApiResult<bool>.Fail($"REST: Wyjątek: {ex.Message}");
            }
        }
    }
}
