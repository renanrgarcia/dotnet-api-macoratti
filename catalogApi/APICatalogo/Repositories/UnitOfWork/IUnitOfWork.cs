namespace ApiCatalogo.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        Task CommitAsync();
    }
}
