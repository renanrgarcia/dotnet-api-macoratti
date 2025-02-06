using ApiCatalogo.Models;

namespace ApiCatalogo.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProductsByCategory(int id);
    }
}
