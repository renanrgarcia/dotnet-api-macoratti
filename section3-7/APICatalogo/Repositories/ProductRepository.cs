using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public PagedList<Product> GetProducts(ProductsParameters productsParameters)
        {
            var products = GetAll()
                .OrderBy(on => on.Name)
                .AsQueryable();

            return PagedList<Product>.ToPagedList(products, productsParameters.PageNumber, productsParameters.PageSize);

        }

        public PagedList<Product> GetProductsFilterPrice(ProductsFilterPrice productsFilterPrice)
        {
            var products = GetAll().AsQueryable();

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

            return PagedList<Product>.ToPagedList(products, productsFilterPrice.PageNumber, productsFilterPrice.PageSize);
        }

        public IEnumerable<Product> GetProductsByCategory(int id)
        {
            return GetAll().Where(p => p.CategoryId == id);
        }
    }
}
