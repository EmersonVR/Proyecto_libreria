namespace LibraryManager.Models.Entities;

public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
