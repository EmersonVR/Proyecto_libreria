namespace LibraryManager.DTOs.Books;

public class BookDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int AvailableCopies { get; set; }
    public int AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
}
