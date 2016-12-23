using System.Data;
using Microsoft.Extensions.Logging;

namespace Dapper.Extensions.Repository
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> CreateRepository<TEntity>(IDbConnection connection, ILogger logger = null) where TEntity : class;
    }
}
