namespace LibraryManager.DTOs.Books;

public class BookInsertDto
{
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int AvailableCopies { get; set; }
    public int AuthorId { get; set; }
    public int CategoryId { get; set; }
}
