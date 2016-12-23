using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Dapper.Extensions.Repository.Extensions;
using Dapper.Extensions.Repository.Helper;
using Dapper.Extensions.Repository.SqlGenerator;
using Microsoft.Extensions.Logging;
using Dapper.Extensions;

namespace Dapper.Extensions.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Constructor
        public Repository(IDbConnection connection, ILogger logger = null)
        {
            Connection = connection;
            SqlGenerator = new SqlGenerator<TEntity>(ESqlConnector.MSSQL);
            Logger = logger;
        }

        public Repository(IDbConnection connection, ESqlConnector sqlConnector, ILogger logger = null)
        {
            Connection = connection;
            SqlGenerator = new SqlGenerator<TEntity>(sqlConnector);
            Logger = logger;
        }

        public Repository(IDbConnection connection, ISqlGenerator<TEntity> sqlGenerator, ILogger logger = null)
        {
            Connection = connection;
            SqlGenerator = sqlGenerator;
            Logger = logger;
        }

        #endregion

        protected ILogger Logger { get; }

        public IDbConnection Connection { get; }

        public ISqlGenerator<TEntity> SqlGenerator { get; }

        #region Find

        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate = null, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetSelectFirst(predicate);

            Logger.LogSql(queryResult.Sql, queryResult.Param);

            return Connection.QueryFirstOrDefault<TEntity>(queryResult.Sql, queryResult.Param, transaction);
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate = null, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetSelectFirst(predicate);

            Logger.LogSql(queryResult.Sql, queryResult.Param);

            return await Connection.QueryFirstOrDefaultAsync<TEntity>(queryResult.Sql, queryResult.Param, transaction);
        }

        #endregion Find

        #region FindAll

        /// <summary>
        ///
        /// </summary>
        public virtual IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate = null, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetSelectAll(predicate);
            return Connection.Query<TEntity>(queryResult.Sql, queryResult.Param, transaction);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate = null, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetSelectAll(predicate);

            return await Connection.QueryAsync<TEntity>(queryResult.Sql, queryResult.Param, transaction);
        }

        #endregion FindAll

        #region Find Multiple Mapping

        public IEnumerable<TEntity> FindAll<TChild1>(
           string sql,
           object param,
           IDbTransaction transaction)
        {
            return FindAll<TChild1, DontMap, DontMap, DontMap, DontMap, DontMap>(sql, param, transaction);
        }

        public IEnumerable<TEntity> FindAll<TChild1, TChild2>(
           string sql,
           object param,
           IDbTransaction transaction)
        {
            return FindAll<TChild1, TChild2, DontMap, DontMap, DontMap, DontMap>(sql, param, transaction);
        }

        public IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3>(
           string sql,
           object param,
           IDbTransaction transaction)
        {
            return FindAll<TChild1, TChild2, TChild3, DontMap, DontMap, DontMap>(sql, param, transaction);
        }

        public IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3, TChild4>(
            string sql,
            object param,
            IDbTransaction transaction)
        {
            return FindAll<TChild1, TChild2, TChild3, TChild4, DontMap, DontMap>(sql, param, transaction);
        }

        public IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3, TChild4, TChild5>(
            string sql,
            object param,
            IDbTransaction transaction)
        {
            return FindAll<TChild1, TChild2, TChild3, TChild4, TChild5, DontMap>(sql, param, transaction);
        }

        public IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
            string sql,
            object param,
            IDbTransaction transaction)
        {
            // Set entity types
            var entityTypes = new List<TypeMetadata>
            {
                new TypeMetadata(typeof(TEntity)),
            };
            var dontMapType = typeof(DontMap);
            Action<Type> addChildType = type =>
            {
                if (type != dontMapType) entityTypes.Add(new TypeMetadata(type, false));
            };
            addChildType(typeof(TChild1));
            addChildType(typeof(TChild2));
            addChildType(typeof(TChild3));
            addChildType(typeof(TChild4));
            addChildType(typeof(TChild5));
            addChildType(typeof(TChild6));

            // Set spilt on
            var spiltOn = "Id";
            var keyFieldNames = entityTypes.Select(p => string.IsNullOrEmpty(p.KeyPropertyMetadata.Alias) ? p.KeyProperty.Name : p.KeyPropertyMetadata.Alias).ToList();
            if (keyFieldNames.Any(p => p != "Id"))
            {
                spiltOn = string.Join(",", keyFieldNames);
            }

            Logger.LogSql(sql);

            var lookup = new Dictionary<object, TEntity>();
            bool buffered = true;
            var rootTypeMetadata = entityTypes.Single(p => p.IsRoot);
            var childTypeMetadatas = entityTypes.Where(p => !p.IsRoot).ToList();
            var childEntityCount = childTypeMetadatas.Count;
            if (childEntityCount == 1)
            {
                Connection.Query<TEntity, TChild1, TEntity>(sql, (entity, child1) => EntityMapping<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(lookup, rootTypeMetadata, childTypeMetadatas, entity, child1), param, transaction, buffered, spiltOn);
            }
            else if (childEntityCount == 2)
            {
                Connection.Query<TEntity, TChild1, TChild2, TEntity>(sql, (entity, child1, child2) => EntityMapping<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(lookup, rootTypeMetadata, childTypeMetadatas, entity, child1, child2), param, transaction, buffered, spiltOn);
            }
            else if (childEntityCount == 3)
            {
                Connection.Query<TEntity, TChild1, TChild2, TChild3, TEntity>(sql, (entity, child1, child2, child3) => EntityMapping<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(lookup, rootTypeMetadata, childTypeMetadatas, entity, child1, child2, child3), param, transaction, buffered, spiltOn);
            }
            else if (childEntityCount == 4)
            {
                Connection.Query<TEntity, TChild1, TChild2, TChild3, TChild4, TEntity>(sql, (entity, child1, child2, child3, child4) => EntityMapping<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(lookup, rootTypeMetadata, childTypeMetadatas, entity, child1, child2, child3, child4), param, transaction, buffered, spiltOn);
            }
            else if (childEntityCount == 5)
            {
                Connection.Query<TEntity, TChild1, TChild2, TChild3, TChild4, TChild5, TEntity>(sql, (entity, child1, child2, child3, child4, child5) => EntityMapping<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(lookup, rootTypeMetadata, childTypeMetadatas, entity, child1, child2, child3, child4, child5), param, transaction, buffered, spiltOn);
            }
            else if (childEntityCount == 6)
            {
                Connection.Query<TEntity, TChild1, TChild2, TChild3, TChild4, TChild5, TChild6, TEntity>(sql, (entity, child1, child2, child3, child4, child5, child6) => EntityMapping<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(lookup, rootTypeMetadata, childTypeMetadatas, entity, child1, child2, child3, child4, child5, child6), param, transaction, buffered, spiltOn);
            }
            else
            {
                throw new NotSupportedException();
            }

            return lookup.Values;
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TChild1>(
           string sql,
           object param,
           IDbTransaction transaction)
        {
            return await FindAllAsync<TChild1, DontMap, DontMap, DontMap, DontMap, DontMap>(sql, param, transaction);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2>(
           string sql,
           object param,
           IDbTransaction transaction)
        {
            return await FindAllAsync<TChild1, TChild2, DontMap, DontMap, DontMap, DontMap>(sql, param, transaction);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3>(
           string sql,
           object param,
           IDbTransaction transaction)
        {
            return await FindAllAsync<TChild1, TChild2, TChild3, DontMap, DontMap, DontMap>(sql, param, transaction);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3, TChild4>(
            string sql,
            object param,
            IDbTransaction transaction)
        {
            return await FindAllAsync<TChild1, TChild2, TChild3, TChild4, DontMap, DontMap>(sql, param, transaction);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(
            string sql,
            object param,
            IDbTransaction transaction)
        {
            return await FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5, DontMap>(sql, param, transaction);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
            string sql,
            object param,
            IDbTransaction transaction)
        {
            // Set entity types
            var entityTypes = new List<TypeMetadata>
            {
                new TypeMetadata(typeof(TEntity)),
            };
            var dontMapType = typeof(DontMap);
            Action<Type> addChildType = type =>
            {
                if (type != dontMapType) entityTypes.Add(new TypeMetadata(type, false));
            };
            addChildType(typeof(TChild1));
            addChildType(typeof(TChild2));
            addChildType(typeof(TChild3));
            addChildType(typeof(TChild4));
            addChildType(typeof(TChild5));
            addChildType(typeof(TChild6));

            // Set spilt on
            var spiltOn = "Id";
            var keyFieldNames = entityTypes.Select(p => string.IsNullOrEmpty(p.KeyPropertyMetadata.Alias) ? p.KeyProperty.Name : p.KeyPropertyMetadata.Alias).ToList();
            if (keyFieldNames.Any(p => p != "Id"))
            {
                spiltOn = string.Join(",", keyFieldNames);
            }

            Logger.LogSql(sql);

            var lookup = new Dictionary<object, TEntity>();
            bool buffered = true;
            var rootTypeMetadata = entityTypes.Single(p => p.IsRoot);
            var childTypeMetadatas = entityTypes.Where(p => !p.IsRoot).ToList();
            var childEntityCount = childTypeMetadatas.Count;
            if (childEntityCount == 1)
            {
                await Connection.QueryAsync<TEntity, TChild1, TEntity>(sql, (entity, child1) => EntityMapping<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(lookup, rootTypeMetadata, childTypeMetadatas, entity, child1), param, transaction, buffered, spiltOn);
            }
            else if (childEntityCount == 2)
            {
                await Connection.QueryAsync<TEntity, TChild1, TChild2, TEntity>(sql, (entity, child1, child2) => EntityMapping<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(lookup, rootTypeMetadata, childTypeMetadatas, entity, child1, child2), param, transaction, buffered, spiltOn);
            }
            else if (childEntityCount == 3)
            {
                await Connection.QueryAsync<TEntity, TChild1, TChild2, TChild3, TEntity>(sql, (entity, child1, child2, child3) => EntityMapping<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(lookup, rootTypeMetadata, childTypeMetadatas, entity, child1, child2, child3), param, transaction, buffered, spiltOn);
            }
            else if (childEntityCount == 4)
            {
                await Connection.QueryAsync<TEntity, TChild1, TChild2, TChild3, TChild4, TEntity>(sql, (entity, child1, child2, child3, child4) => EntityMapping<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(lookup, rootTypeMetadata, childTypeMetadatas, entity, child1, child2, child3, child4), param, transaction, buffered, spiltOn);
            }
            else if (childEntityCount == 5)
            {
                await Connection.QueryAsync<TEntity, TChild1, TChild2, TChild3, TChild4, TChild5, TEntity>(sql, (entity, child1, child2, child3, child4, child5) => EntityMapping<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(lookup, rootTypeMetadata, childTypeMetadatas, entity, child1, child2, child3, child4, child5), param, transaction, buffered, spiltOn);
            }
            else if (childEntityCount == 6)
            {
                await Connection.QueryAsync<TEntity, TChild1, TChild2, TChild3, TChild4, TChild5, TChild6, TEntity>(sql, (entity, child1, child2, child3, child4, child5, child6) => EntityMapping<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(lookup, rootTypeMetadata, childTypeMetadatas, entity, child1, child2, child3, child4, child5, child6), param, transaction, buffered, spiltOn);
            }
            else
            {
                throw new NotSupportedException();
            }

            return lookup.Values;
        }

        private TEntity EntityMapping<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(Dictionary<object, TEntity> lookup, TypeMetadata rootTypeMetadata, IList<TypeMetadata> childTypeMetadatas, TEntity entity, params object[] childs)
        {
            var key = rootTypeMetadata.KeyProperty.GetValue(entity);

            TEntity target;
            if (!lookup.TryGetValue(key, out target))
            {
                lookup.Add(key, target = entity);
            }

            var tagretChilds = new List<object>();
            for (var i = 0; i < childs.Length; i++)
            {
                var child = childs[i];
                if (child == null)
                {
                    continue;
                }

                var childTypeMetadata = childTypeMetadatas[i];

                #region Get parent entity type

                Type parentEntityType = null;
                PropertyInfo parentPropertyType = null;
                var parentIndex = 0;

                parentPropertyType =
                    rootTypeMetadata.ChildProperties.SingleOrDefault(p => p.PropertyType == childTypeMetadata.Type) ??
                    rootTypeMetadata.ChildProperties.SingleOrDefault(
                        p =>
                            p.PropertyType.IsGenericType() &&
                            p.PropertyType.GenericTypeArguments.Any(
                                x => x == childTypeMetadata.Type));

                if (parentPropertyType != null)
                {
                    parentEntityType = rootTypeMetadata.Type;
                }
                else
                {
                    for (var j = 0; j < childTypeMetadatas.Count; j++)
                    {
                        parentPropertyType =
                            childTypeMetadatas[j].ChildProperties.SingleOrDefault(
                                p => p.PropertyType == childTypeMetadata.Type) ??
                            childTypeMetadatas[j].ChildProperties.SingleOrDefault(
                                p =>
                                    p.PropertyType.IsGenericType() &&
                                    p.PropertyType.GenericTypeArguments.Any(
                                        x => x == childTypeMetadata.Type));

                        if (parentPropertyType != null)
                        {
                            parentEntityType = childTypeMetadatas[j].Type;
                            parentIndex = j;
                            break;
                        }
                    }
                }

                if (parentEntityType == null || parentPropertyType == null)
                {
                    throw new Exception("Can not find parent entity and parent property");
                }

                #endregion

                #region Get parent entity

                object parentEntity = null;
                if (rootTypeMetadata.Type == parentEntityType)
                {
                    parentEntity = target;
                }
                else
                {
                    parentEntity = tagretChilds[parentIndex];
                }

                #endregion

                if (parentPropertyType.PropertyType.IsGenericType())
                {
                    var list = (IList)parentPropertyType.GetValue(parentEntity);

                    if (list == null)
                    {
                        switch (i)
                        {
                            case 0:
                                list = new List<TChild1>();
                                break;
                            case 1:
                                list = new List<TChild2>();
                                break;
                            case 2:
                                list = new List<TChild3>();
                                break;
                            case 3:
                                list = new List<TChild4>();
                                break;
                            case 4:
                                list = new List<TChild5>();
                                break;
                            case 5:
                                list = new List<TChild6>();
                                break;
                            default:
                                throw new NotSupportedException();
                        }

                        parentPropertyType.SetValue(parentEntity, list);
                    }

                    var childKey = childTypeMetadata.KeyProperty.GetValue(child);
                    var value = (from object item in list where childTypeMetadata.KeyProperty.GetValue(item).Equals(childKey) select item).SingleOrDefault();
                    if (value == null)
                    {
                        list.Add(value = child);
                    }

                    tagretChilds.Add(value);
                }
                else
                {
                    var value = parentPropertyType.GetValue(parentEntity);
                    if (value == null)
                    {
                        parentPropertyType.SetValue(parentEntity, value = child);
                    }

                    tagretChilds.Add(value);
                }
            }

            return target;
        }


        #endregion

        #region Count

        public virtual int Count(Expression<Func<TEntity, bool>> predicate = null, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetSelectCount(predicate);

            Logger.LogSql(queryResult.Sql, queryResult.Param);

            return Connection.Query<int>(queryResult.Sql, queryResult.Param, transaction).First();
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetSelectCount(predicate);

            Logger.LogSql(queryResult.Sql, queryResult.Param);

            return (await Connection.QueryAsync<int>(queryResult.Sql, queryResult.Param, transaction)).First();
        }

        #endregion

        #region Any

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate = null, IDbTransaction transaction = null)
        {
            return Count(predicate, transaction) > 0;
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null, IDbTransaction transaction = null)
        {
            return await CountAsync(predicate, transaction) > 0;
        }

        #endregion

        #region Insert

        /// <summary>
        ///
        /// </summary>
        public virtual bool Insert(TEntity instance, IDbTransaction transaction = null)
        {
            bool added;

            var queryResult = SqlGenerator.GetInsert(instance);

            Logger.LogSql(queryResult.Sql, queryResult.Param);

            if (SqlGenerator.IsIdentity)
            {
                var newId = Connection.Query<long>(queryResult.Sql, queryResult.Param, transaction).FirstOrDefault();
                added = newId > 0;

                if (added)
                {
                    var newParsedId = Convert.ChangeType(newId, SqlGenerator.IdentitySqlProperty.PropertyInfo.PropertyType);
                    SqlGenerator.IdentitySqlProperty.PropertyInfo.SetValue(instance, newParsedId);
                }
            }
            else
            {
                added = Connection.Execute(queryResult.Sql, instance, transaction) > 0;
            }

            return added;
        }

        /// <summary>
        ///
        /// </summary>>
        public virtual async Task<bool> InsertAsync(TEntity instance, IDbTransaction transaction = null)
        {
            bool added;

            var queryResult = SqlGenerator.GetInsert(instance);

            Logger.LogSql(queryResult.Sql, queryResult.Param);

            if (SqlGenerator.IsIdentity)
            {
                var newId = (await Connection.QueryAsync<long>(queryResult.Sql, queryResult.Param, transaction)).FirstOrDefault();
                added = newId > 0;

                if (added)
                {
                    var newParsedId = Convert.ChangeType(newId, SqlGenerator.IdentitySqlProperty.PropertyInfo.PropertyType);
                    SqlGenerator.IdentitySqlProperty.PropertyInfo.SetValue(instance, newParsedId);
                }
            }
            else
            {
                added = Connection.Execute(queryResult.Sql, instance, transaction) > 0;
            }

            return added;
        }

        #endregion Insert

        #region Delete

        /// <summary>
        ///
        /// </summary>
        public virtual bool Delete(TEntity instance, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetDelete(instance);

            Logger.LogSql(queryResult.Sql, queryResult.Param);

            var deleted = Connection.Execute(queryResult.Sql, queryResult.Param, transaction) > 0;

            return deleted;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual async Task<bool> DeleteAsync(TEntity instance, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetDelete(instance);

            Logger.LogSql(queryResult.Sql, queryResult.Param);

            var deleted = (await Connection.ExecuteAsync(queryResult.Sql, queryResult.Param, transaction)) > 0;

            return deleted;
        }

        #endregion Delete

        #region Update

        /// <summary>
        ///
        /// </summary>
        public virtual bool Update(TEntity instance, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetUpdate(instance);

            Logger.LogSql(queryResult.Sql, queryResult.Param);

            var updated = Connection.Execute(queryResult.Sql, instance, transaction) > 0;

            return updated;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual async Task<bool> UpdateAsync(TEntity instance, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetUpdate(instance);

            Logger.LogSql(queryResult.Sql, queryResult.Param);

            var updated = (await Connection.ExecuteAsync(queryResult.Sql, instance, transaction)) > 0;

            return updated;
        }

        #endregion Update

        /// <summary>
        /// Dummy type for excluding from multi-map
        /// </summary>
        private class DontMap { }

        private class TypeMetadata
        {
            public TypeMetadata(Type type, bool isRoot = true)
            {
                Type = type;
                IsRoot = isRoot;

                var allProperties = type.GetProperties().Where(q => q.CanWrite).ToArray();

                // Filter key properties
                var keyProperties =
                    allProperties.Where(p => p.GetCustomAttributes<KeyAttribute>().Any())
                        .ToArray();
                if (keyProperties.Length > 1)
                {
                    throw new Exception("Support only one key in a table");
                }
                KeyProperty = keyProperties.First();
                KeyPropertyMetadata = new SqlPropertyMetadata(KeyProperty);

                // Filter child properties
                ChildProperties = allProperties.Where(ExpressionHelper.GetChildPropertiesPredicate()).ToArray();
            }

            public Type Type { get; }

            public PropertyInfo KeyProperty { get; }

            public SqlPropertyMetadata KeyPropertyMetadata { get; }

            public PropertyInfo[] ChildProperties { get; }

            public bool IsRoot { get; }
        }
    }
}
