using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using LibraryManager.ExternalServices.DTOs;

namespace LibraryManager.ExternalServices;

public class ExternalBookInfoService : IExternalBookInfoService
{
    private const string OpenLibraryClientName = "OpenLibrary";
    private const string DummyJsonQuotesClientName = "DummyJsonQuotes";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    public ExternalBookInfoService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<ExternalBookInfoDto>> GetBooksAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient(OpenLibraryClientName);
            using var response = await client.GetAsync("search.json?q=clean%20code&limit=5");

            if (!response.IsSuccessStatusCode)
            {
                return Enumerable.Empty<ExternalBookInfoDto>();
            }

            var searchResult = await response.Content.ReadFromJsonAsync<OpenLibrarySearchResponse>(_jsonOptions);
            if (searchResult?.Docs is null)
            {
                return Enumerable.Empty<ExternalBookInfoDto>();
            }

            return searchResult.Docs.Select(book => new ExternalBookInfoDto
            {
                Title = book.Title,
                Authors = book.AuthorNames ?? Enumerable.Empty<string>(),
                FirstPublishYear = book.FirstPublishYear,
                Isbn = book.Isbns?.FirstOrDefault()
            });
        }
        catch (HttpRequestException)
        {
            return Enumerable.Empty<ExternalBookInfoDto>();
        }
        catch (JsonException)
        {
            return Enumerable.Empty<ExternalBookInfoDto>();
        }
    }

    public async Task<ExternalQuoteDto?> GetQuoteAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient(DummyJsonQuotesClientName);
            using var response = await client.GetAsync("quotes/random");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<ExternalQuoteDto>(_jsonOptions);
        }
        catch (HttpRequestException)
        {
            return null;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private class OpenLibrarySearchResponse
    {
        [JsonPropertyName("docs")]
        public IEnumerable<OpenLibraryBookDocument>? Docs { get; set; }
    }

    private class OpenLibraryBookDocument
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("author_name")]
        public IEnumerable<string>? AuthorNames { get; set; }

        [JsonPropertyName("first_publish_year")]
        public int? FirstPublishYear { get; set; }

        [JsonPropertyName("isbn")]
        public IEnumerable<string>? Isbns { get; set; }
    }
}
