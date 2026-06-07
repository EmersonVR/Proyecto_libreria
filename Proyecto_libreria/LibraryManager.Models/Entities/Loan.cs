using LibraryManager.Models.Enums;

namespace LibraryManager.Models.Entities;

public class Loan
{
    public int LoanId { get; set; }

    public int BookId { get; set; }
    public Book? Book { get; set; }

    public int ReaderId { get; set; }
    public Reader? Reader { get; set; }

    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public LoanStatus Status { get; set; }
}
