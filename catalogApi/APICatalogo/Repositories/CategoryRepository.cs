using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using X.PagedList;

namespace ApiCatalogo.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParameters)
        {
            var categories = await GetAllAsync();

            var orderedCategories = categories
                .OrderBy(c => c.CategoryId)
                .AsQueryable();

            //return IPagedList<Category>.ToPagedList(
            //    orderedCategories, categoriesParameters.PageNumber, categoriesParameters.PageSize);

            return await orderedCategories.ToPagedListAsync(
                categoriesParameters.PageNumber, categoriesParameters.PageSize);
        }

        public async Task<IPagedList<Category>> GetCategoriesFilterNameAsync(CategoriesFilterName categoriesFilterName)
        {
            var categories = await GetAllAsync();

            if (!string.IsNullOrEmpty(categoriesFilterName.Name))
            {
                var normalizedFilterName = categoriesFilterName.Name.ToLower();
                categories = categories.Where(c =>
                    c.Name != null && c.Name.Contains(normalizedFilterName, StringComparison.CurrentCultureIgnoreCase)
                ).OrderBy(c => c.Name);
            }

            //return IPagedList<Category>.ToPagedList(
            //    categories.AsQueryable(), categoriesFilterName.PageNumber, categoriesFilterName.PageSize);

            return await categories.ToPagedListAsync(
                categoriesFilterName.PageNumber, categoriesFilterName.PageSize);
        }
    }
}