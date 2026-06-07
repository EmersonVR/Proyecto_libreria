using LibraryManager.ExternalServices.DTOs;

namespace LibraryManager.ExternalServices;

public interface IExternalBookInfoService
{
    Task<IEnumerable<ExternalBookInfoDto>> GetBooksAsync();
    Task<ExternalQuoteDto?> GetQuoteAsync();
}
