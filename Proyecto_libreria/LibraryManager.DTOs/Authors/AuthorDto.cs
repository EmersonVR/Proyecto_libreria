namespace LibraryManager.DTOs.Authors;

public class AuthorDto
{
    public int AuthorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
}
