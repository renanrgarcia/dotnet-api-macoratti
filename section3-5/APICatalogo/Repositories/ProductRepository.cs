using ApiCatalogo.Context;
using ApiCatalogo.Models;

namespace ApiCatalogo.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Product> GetProducts()
        {
            return _context.Products;
        }

        public Product GetProduct(int id)
        {
            return _context.Products.FirstOrDefault(c => c.ProductId == id);
        }

        public Product Create(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public bool Update(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (_context.Products.Any(p => p.ProductId == product.ProductId))
            {
                _context.Products.Update(product);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public bool Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product is not null)
            {
                _context.Products.Remove(product);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
