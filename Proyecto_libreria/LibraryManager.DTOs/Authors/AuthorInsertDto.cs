namespace LibraryManager.DTOs.Authors;

public class AuthorInsertDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
}
