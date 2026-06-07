namespace LibraryManager.DTOs.Readers;

public class ReaderUpdateDto
{
    public int ReaderId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
}
