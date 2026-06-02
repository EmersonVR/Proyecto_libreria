using LibraryManager.DataAccess.Repositories.Interfaces;
using LibraryManager.DTOs.Categories;
using LibraryManager.Models.Entities;
using LibraryManager.Services.Interfaces;

namespace LibraryManager.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        // TODO: Map Category entities manually to CategoryDto.
        var categories = await _repository.GetAllAsync();
        return categories.Select(c => new CategoryDto
        {
            CategoryId = c.CategoryId,
            Name = c.Name,
            Description = c.Description
        });

    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null) return null;

        return new CategoryDto
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task<CategoryDto> CreateAsync(CategoryInsertDto dto)
    {
        // TODO: Check duplicate name and map manually.

        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };

        if (await _repository.NameExistsAsync(dto.Name))
        {
            throw new InvalidOperationException($"Category with name '{dto.Name}' already exists.");
        }

        var createdCategory = await _repository.AddAsync(category);

        return new CategoryDto
        {
            CategoryId = createdCategory.CategoryId,
            Name = createdCategory.Name,
            Description = createdCategory.Description
        };


    }

    public async Task<bool> UpdateAsync(int id, CategoryUpdateDto dto)
    {
        if (id != dto.CategoryId)
        {
            throw new ArgumentException("ID in URL does not match ID in body.");
        }        

        var createcategory = await _repository.GetByIdAsync(id);
        
        if (createcategory == null) return false;

        var exists = await _repository.NameExistsAsync(dto.Name, id);

        if (exists)
        {
            throw new InvalidOperationException($"Category with name '{dto.Name}' already exists.");
        }


        createcategory.Name = dto.Name;
        createcategory.Description = dto.Description;
        await _repository.UpdateAsync(createcategory);
        return true;


    }

    public async Task<bool> DeleteAsync(int id)
    {
        // TODO: Check that the category has no books before deleting.
        
        var category = await _repository.GetByIdAsync(id); if (category == null) return false;

        await _repository.DeleteAsync(category); return true;
    }
}
