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

        public IEnumerable<Product> GetProducts(ProductsParameters productsParameters)
        {
            return GetAll()
                .OrderBy(on => on.Name)
                .Skip((productsParameters.PageNumber - 1) * productsParameters.PageSize)
                .Take(productsParameters.PageSize)
                .ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(int id)
        {
            return GetAll().Where(p => p.CategoryId == id);
        }
    }
}
