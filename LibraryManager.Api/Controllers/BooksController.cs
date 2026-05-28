using FluentValidation;
using LibraryManager.DTOs.Books;
using LibraryManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.Api.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _service;
    private readonly IValidator<BookInsertDto> _insertValidator;
    private readonly IValidator<BookUpdateDto> _updateValidator;

    public BooksController(
        IBookService service,
        IValidator<BookInsertDto> insertValidator,
        IValidator<BookUpdateDto> updateValidator)
    {
        _service = service;
        _insertValidator = insertValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> Create(BookInsertDto dto)
    {
        var validationResult = await _insertValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        // TODO: Return Conflict for duplicate ISBN or missing author/category business rules.
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.BookId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, BookUpdateDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var updated = await _service.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        // TODO: Return Conflict when the book has active loans.
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<BookDto>>> Search([FromQuery] string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return BadRequest("The title query parameter is required.");
        }

        return Ok(await _service.SearchByTitleAsync(title));
    }

    [HttpGet("by-author/{authorId:int}")]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetByAuthor(int authorId)
    {
        return Ok(await _service.GetByAuthorAsync(authorId));
    }

    [HttpGet("by-category/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetByCategory(int categoryId)
    {
        return Ok(await _service.GetByCategoryAsync(categoryId));
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAvailable()
    {
        return Ok(await _service.GetAvailableAsync());
    }
}
