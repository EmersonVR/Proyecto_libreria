namespace LibraryManager.DTOs.Loans;

public class LoanDto
{
    public int LoanId { get; set; }
    public int BookId { get; set; }
    public string? BookTitle { get; set; }
    public int ReaderId { get; set; }
    public string? ReaderName { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
