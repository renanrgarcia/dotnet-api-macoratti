using ApiCatalogo.Models;

namespace ApiCatalogo.Repositories
{
    public interface IProductRepository
    {
        IQueryable<Product> GetProducts();
        Product GetProduct(int id);
        Product Create(Product produto);
        bool Update(Product produto);
        bool Delete(int id);

    }
}
