using System.Data;

namespace Dapper.Extensions.Repository
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> CreateRepository<TEntity>(IDbConnection connection) where TEntity : class;
    }
}
