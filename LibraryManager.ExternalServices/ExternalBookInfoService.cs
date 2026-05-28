using System.Net.Http.Json;
using LibraryManager.ExternalServices.DTOs;

namespace LibraryManager.ExternalServices;

public class ExternalBookInfoService : IExternalBookInfoService
{
    private readonly HttpClient _httpClient;

    public ExternalBookInfoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ExternalBookInfoDto>> GetBooksAsync()
    {
        // TODO: Replace demo path with a real public API endpoint if desired.
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<ExternalBookInfoDto>>("books");
        return result ?? Enumerable.Empty<ExternalBookInfoDto>();
    }

    public async Task<ExternalQuoteDto?> GetQuoteAsync()
    {
        // TODO: Replace demo path with a real public API endpoint if desired.
        return await _httpClient.GetFromJsonAsync<ExternalQuoteDto>("quote");
    }
}
