using FluentValidation;
using LibraryManager.DTOs.Loans;
using LibraryManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.Api.Controllers;

[ApiController]
[Route("api/loans")]
public class LoansController : ControllerBase
{
    private readonly ILoanService _service;
    private readonly IValidator<LoanInsertDto> _insertValidator;

    public LoansController(ILoanService service, IValidator<LoanInsertDto> insertValidator)
    {
        _service = service;
        _insertValidator = insertValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LoanDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LoanDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<LoanDto>>> GetActive()
    {
        return Ok(await _service.GetActiveAsync());
    }

    [HttpGet("by-reader/{readerId:int}")]
    public async Task<ActionResult<IEnumerable<LoanDto>>> GetByReader(int readerId)
    {
        return Ok(await _service.GetByReaderAsync(readerId));
    }

    [HttpPost]
    public async Task<ActionResult<LoanDto>> Create(LoanInsertDto dto)
    {
        var validationResult = await _insertValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        // TODO: Return Conflict when the book has 0 available copies.
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.LoanId }, created);
    }

    [HttpPut("{id:int}/return")]
    public async Task<IActionResult> Return(int id)
    {
        // TODO: Return Conflict when the loan was already returned.
        var returned = await _service.ReturnAsync(id);
        return returned ? NoContent() : NotFound();
    }
}
