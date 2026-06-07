namespace LibraryManager.DTOs.Categories;

public class CategoryUpdateDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
