namespace LibraryManager.ExternalServices.DTOs;

public class ExternalBookInfoDto
{
    public string? Title { get; set; }
    public IEnumerable<string> Authors { get; set; } = Enumerable.Empty<string>();
    public int? FirstPublishYear { get; set; }
    public string? Isbn { get; set; }
}
