using System;
using System.Data;

namespace Dapper.Extensions.Repository.Context
{
    /// <summary>
    /// DbContext interface
    /// </summary>
    public interface IDbContext : IDisposable
    {
        IDbConnection Connection { get; }

        /// <summary>
        /// Get repository by type of entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IRepository<TEntity> SetEntity<TEntity>() where TEntity : class;
    }
}
