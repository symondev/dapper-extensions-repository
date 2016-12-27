namespace Dapper.Extensions.Repository
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class;
    }
}
