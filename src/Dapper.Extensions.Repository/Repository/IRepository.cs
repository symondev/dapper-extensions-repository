using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper.Extensions.Repository.SqlGenerator;

namespace Dapper.Extensions.Repository
{
    /// <summary>
    /// Repository interface
    /// </summary>
    public interface IRepository
    {
    }

    /// <summary>
    /// Repository interface
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IRepository
        where TEntity : class
    {
        /// <summary>
        ///
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        ///
        /// </summary>
        ISqlGenerator<TEntity> SqlGenerator { get; }

        #region Find

        TEntity Find(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null);

        TEntity Find(string sql, object param = null, IDbTransaction transaction = null);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null);

        Task<TEntity> FindAsync(string sql, object param = null, IDbTransaction transaction = null);

        #endregion

        #region Find All
        
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate = null, IDbTransaction transaction = null);

        IEnumerable<TEntity> FindAll(string sql, object param = null, IDbTransaction transaction = null);
        
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate = null, IDbTransaction transaction = null);

        Task<IEnumerable<TEntity>> FindAllAsync(string sql, object param = null, IDbTransaction transaction = null);

        #endregion

        #region Find Multiple Mapping

        IEnumerable<TEntity> FindAll<TChild1>(string sql, object param = null, IDbTransaction transaction = null);

        IEnumerable<TEntity> FindAll<TChild1, TChild2>(string sql, object param = null, IDbTransaction transaction = null);

        IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3>(string sql, object param = null, IDbTransaction transaction = null);

        IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3, TChild4>(string sql, object param = null, IDbTransaction transaction = null);

        IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3, TChild4, TChild5>(string sql, object param = null, IDbTransaction transaction = null);

        IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(string sql, object param = null, IDbTransaction transaction = null);

        Task<IEnumerable<TEntity>> FindAllAsync<TChild1>(string sql, object param = null, IDbTransaction transaction = null);

        Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2>(string sql, object param = null, IDbTransaction transaction = null);

        Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3>(string sql, object param = null, IDbTransaction transaction = null);

        Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3, TChild4>(string sql, object param = null, IDbTransaction transaction = null);

        Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(string sql, object param = null, IDbTransaction transaction = null);

        Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(string sql, object param = null, IDbTransaction transaction = null);

        #endregion

        #region Count

        int Count(Expression<Func<TEntity, bool>> predicate = null, IDbTransaction transaction = null);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null, IDbTransaction transaction = null);

        #endregion

        #region Any

        bool Any(Expression<Func<TEntity, bool>> predicate = null, IDbTransaction transaction = null);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null, IDbTransaction transaction = null);

        #endregion

        #region Insert

        /// <summary>
        ///
        /// </summary>
        bool Insert(TEntity instance, IDbTransaction transaction = null);

        /// <summary>
        ///
        /// </summary>>
        Task<bool> InsertAsync(TEntity instance, IDbTransaction transaction = null);

        #endregion

        #region Delete

        /// <summary>
        ///
        /// </summary>
        bool Delete(TEntity instance, IDbTransaction transaction = null);

        /// <summary>
        ///
        /// </summary>
        Task<bool> DeleteAsync(TEntity instance, IDbTransaction transaction = null);

        #endregion

        #region Update

        /// <summary>
        ///
        /// </summary>
        bool Update(TEntity instance, IDbTransaction transaction = null);

        /// <summary>
        ///
        /// </summary>
        Task<bool> UpdateAsync(TEntity instance, IDbTransaction transaction = null);

        #endregion

        #region Execute

        int Execute(string sql, object param = null, IDbTransaction transaction = null);

        Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null);

        IDataReader ExecuteReader(string sql, object param = null, IDbTransaction transaction = null);

        Task<IDataReader> ExecuteReaderAsync(string sql, object param = null, IDbTransaction transaction = null);

        object ExecuteScalar(string sql, object param = null, IDbTransaction transaction = null);

        Task<object> ExecuteScalarAsync(string sql, object param = null, IDbTransaction transaction = null);

        T ExecuteScalar<T>(string sql, object param = null, IDbTransaction transaction = null);

        Task<T> ExecuteScalarAsync<T>(string sql, object param = null, IDbTransaction transaction = null);

        #endregion
    }
}
