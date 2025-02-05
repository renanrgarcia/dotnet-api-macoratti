using ApiCatalogo.Models;

namespace ApiCatalogo.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories();
        Category GetCategory(int categoryId);
        Category CreateCategory(Category category);
        Category UpdateCategory(Category category);
        Category DeleteCategory(int categoryId);
    }
}