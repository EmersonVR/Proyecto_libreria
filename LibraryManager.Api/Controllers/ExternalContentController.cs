using LibraryManager.ExternalServices;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.Api.Controllers;

[ApiController]
[Route("api/external-content")]
public class ExternalContentController : ControllerBase
{
    private readonly IExternalBookInfoService _externalBookInfoService;

    public ExternalContentController(IExternalBookInfoService externalBookInfoService)
    {
        _externalBookInfoService = externalBookInfoService;
    }

    [HttpGet("books")]
    public async Task<IActionResult> GetBooks([FromHeader(Name = "X-Practice-Source")] string? practiceSource)
    {
        // TODO: Header is optional and exists only to practice reading headers.
        var result = await _externalBookInfoService.GetBooksAsync();
        return Ok(new { practiceSource, result });
    }

    [HttpGet("quote")]
    public async Task<IActionResult> GetQuote()
    {
        var result = await _externalBookInfoService.GetQuoteAsync();
        return result is null ? NotFound() : Ok(result);
    }
}
