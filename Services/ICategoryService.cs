using ESIID42025.DTOs;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(int id);
    Task AddCategoryAsync(CategoryDto category);
    Task UpdateCategoryAsync(CategoryDto category);
    Task DeleteCategoryAsync(int id);
}