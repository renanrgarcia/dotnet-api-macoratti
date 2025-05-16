using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using X.PagedList;

namespace ApiCatalogo.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productsParameters)
        {
            var products = await GetAllAsync();

            var orderedProducts = products
                .OrderBy(on => on.Name)
                .AsQueryable();

            return await orderedProducts.ToPagedListAsync(productsParameters.PageNumber, productsParameters.PageSize);

        }

        public async Task<IPagedList<Product>> GetProductsFilterPriceAsync(ProductsFilterPrice productsFilterPrice)
        {
            var products = await GetAllAsync();

            if (productsFilterPrice.Price.HasValue && !string.IsNullOrEmpty(productsFilterPrice.PriceCriteria))
            {
                if (productsFilterPrice.PriceCriteria.Equals("greaterThan", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price > productsFilterPrice.Price.Value).OrderBy(p => p.Price);
                }
                else if (productsFilterPrice.PriceCriteria.Equals("lessThan", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price < productsFilterPrice.Price.Value).OrderBy(p => p.Price);
                }
                else if (productsFilterPrice.PriceCriteria.Equals("equalTo", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price == productsFilterPrice.Price.Value).OrderBy(p => p.Price);
                }
            }

            return await products.ToPagedListAsync(productsFilterPrice.PageNumber, productsFilterPrice.PageSize);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id)
        {
            var products = await GetAllAsync();

            return products.Where(p => p.CategoryId == id);
        }
    }
}
