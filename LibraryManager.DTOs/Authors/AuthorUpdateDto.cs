namespace LibraryManager.DTOs.Authors;

public class AuthorUpdateDto
{
    public int AuthorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
}
