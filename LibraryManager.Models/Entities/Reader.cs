namespace LibraryManager.Models.Entities;

public class Reader
{
    public int ReaderId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
