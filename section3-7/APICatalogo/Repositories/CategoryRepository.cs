using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public PagedList<Category> GetCategories(CategoriesParameters categoriesParameters)
        {
            var categories = GetAll()
                .OrderBy(on => on.CategoryId)
                .AsQueryable();

            return PagedList<Category>.ToPagedList(categories, categoriesParameters.PageNumber, categoriesParameters.PageSize);
        }

        public PagedList<Category> GetCategoriesFilterName(CategoriesFilterName categoriesFilterName)
        {
            var categories = GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(categoriesFilterName.Name))
            {
                var normalizedFilterName = categoriesFilterName.Name.ToLower();
                categories = categories.Where(c =>
                    c.Name != null && c.Name.Contains(normalizedFilterName, StringComparison.CurrentCultureIgnoreCase)
                ).OrderBy(c => c.Name);
            }

            return PagedList<Category>.ToPagedList(categories, categoriesFilterName.PageNumber, categoriesFilterName.PageSize);
        }
    }
}