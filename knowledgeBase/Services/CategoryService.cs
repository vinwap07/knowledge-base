using knowledgeBase.Entities;
using knowledgeBase.Repositories;

namespace knowledgeBase.Services;

public class CategoryService
{
    private readonly CategoryRepository _categoryRepository;

    public CategoryService(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<Category>> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAll();
        return categories;
    }

    public async Task<string> GetCategoryNameById(string slug)
    {
        return (await GetById(slug)).Name;
    }

    public async Task<Category> GetById(string slug)
    {
        var category = await _categoryRepository.GetById(slug);
        return category;
    }
}