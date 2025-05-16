using ApiCatalogo.Context;

namespace ApiCatalogo.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private ICategoryRepository? _categoryRepository;

        private IProductRepository? _productRepository;

        public AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository ??= new CategoryRepository(_context);
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository ??= new ProductRepository(_context);
            }
        }


        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
