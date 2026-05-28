using LibraryManager.DataAccess.Repositories.Interfaces;
using LibraryManager.DTOs.Categories;
using LibraryManager.Services.Interfaces;

namespace LibraryManager.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        // TODO: Map Category entities manually to CategoryDto.
        throw new NotImplementedException("TODO: Implement Category GetAllAsync.");
    }

    public Task<CategoryDto?> GetByIdAsync(int id)
    {
        throw new NotImplementedException("TODO: Implement Category GetByIdAsync.");
    }

    public Task<CategoryDto> CreateAsync(CategoryInsertDto dto)
    {
        // TODO: Check duplicate name and map manually.
        throw new NotImplementedException("TODO: Implement Category CreateAsync.");
    }

    public Task<bool> UpdateAsync(int id, CategoryUpdateDto dto)
    {
        throw new NotImplementedException("TODO: Implement Category UpdateAsync.");
    }

    public Task<bool> DeleteAsync(int id)
    {
        // TODO: Check that the category has no books before deleting.
        throw new NotImplementedException("TODO: Implement Category DeleteAsync.");
    }
}
