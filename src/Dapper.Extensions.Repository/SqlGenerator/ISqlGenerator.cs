using System;
using System.Linq.Expressions;

namespace Dapper.Extensions.Repository.SqlGenerator
{

    /// <summary>
    /// Universal SqlGenerator for Tables
    /// </summary>
    public interface ISqlGenerator<TEntity> where TEntity : class
    {
        /// <summary>
        /// 
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsIdentity { get; }

        /// <summary>
        /// 
        /// </summary>
        ESqlConnector SqlConnector { get; set; }

        /// <summary>
        /// 
        /// </summary>
        SqlPropertyMetadata[] KeySqlProperties { get; }

        /// <summary>
        /// 
        /// </summary>
        SqlPropertyMetadata[] SqlProperties { get; }

        /// <summary>
        /// 
        /// </summary>
        SqlPropertyMetadata IdentitySqlProperty { get; }

        /// <summary>
        /// 
        /// </summary>
        string StatusPropertyName { get; }

        /// <summary>
        /// 
        /// </summary>
        object LogicalDeleteValue { get; }

        /// <summary>
        /// 
        /// </summary>
        bool LogicalDelete { get; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        SqlQuery GetSelectFirst(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        SqlQuery GetSelectAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        SqlQuery GetSelectCount(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        SqlQuery GetInsert(TEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        SqlQuery GetUpdate(TEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        SqlQuery GetDelete(TEntity entity);
    }
}