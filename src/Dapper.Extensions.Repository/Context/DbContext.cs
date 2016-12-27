﻿using System;
using System.Collections.Generic;
using System.Data;
using Dapper.Extensions.Repository.Extensions;
using Microsoft.Extensions.Logging;

namespace Dapper.Extensions.Repository.Context
{
    /// <summary>
    /// DbContext
    /// </summary>
    public class DbContext : IDbContext
    {
        public DbContext(IDbConnection connection, ILogger logger = null)
        {
            InnerConnection = connection;
            Repositories = new Dictionary<Type, IRepository>();
            Logger = logger;
        }
        
        public DbContext(IDbConnection connection, IRepositoryFactory repositoryFactory, ILogger logger = null) : this(connection, logger)
        {
            RepositoryFactory = repositoryFactory;
        }

        /// <summary>
        /// DB Connection for internal use
        /// </summary>
        protected readonly IDbConnection InnerConnection;

        protected readonly ILogger Logger;

        public Dictionary<Type, IRepository> Repositories;

        public IRepositoryFactory RepositoryFactory;

        /// <summary>
        /// Get opened DB Connection
        /// </summary>
        public virtual IDbConnection Connection
        {
            get
            {
                OpenConnection();
                return InnerConnection;
            }
        }

        /// <summary>
        /// Open DB connection
        /// </summary>
        public void OpenConnection()
        {
            if (InnerConnection.State != ConnectionState.Open && InnerConnection.State != ConnectionState.Connecting)
            {
                Logger.LogDbContext("Open connection.");
                InnerConnection.Open();
            }
        }

        /// <summary>
        /// Open DB connection and Begin transaction
        /// </summary>
        public virtual IDbTransaction BeginTransaction()
        {
            Logger.LogDbContext("Begin transaction.");

            return Connection.BeginTransaction();
        }

        /// <summary>
        /// Get repository by type of entity
        /// If can't find target repository within private dictionary, auto build new repository instance with type of entity then add it to private dictionary.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IRepository<TEntity> SetEntity<TEntity>() where TEntity : class
        {
            var type = typeof (TEntity);
            if (Repositories.ContainsKey(type))
            {
                return (IRepository<TEntity>) Repositories[type];
            }
            else
            {
                Logger.LogDbContext($"Add new repository {typeof(TEntity).FullName} and into DbContext.");

                var repository = RepositoryFactory == null
                    ? new Repository<TEntity>(Connection, Logger)
                    : RepositoryFactory.CreateRepository<TEntity>();

                Repositories.Add(type, repository);

                return repository;
            }
        }

        /// <summary>
        /// Close DB connection
        /// </summary>
        public void Dispose()
        {
            if (InnerConnection != null && InnerConnection.State != ConnectionState.Closed)
            {
                Logger.LogDbContext($"Close connection.");

                InnerConnection.Close();
            }
        }
    }
}
