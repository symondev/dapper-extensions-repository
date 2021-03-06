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

            Logger?.LogInformation("Started new IDbContext.");
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
                InnerConnection.Open();

                Logger?.LogInformation("Opened sql connection.");
            }
        }

        /// <summary>
        /// Open DB connection and Begin transaction
        /// </summary>
        public virtual IDbTransaction BeginTransaction()
        {
            var tran = Connection.BeginTransaction();

            Logger?.LogInformation("Began transaction.");

            return tran;
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
                var repository = RepositoryFactory == null
                    ? new Repository<TEntity>(Connection, Logger)
                    : RepositoryFactory.CreateRepository<TEntity>(Connection, Logger);

                Repositories.Add(type, repository);

                Logger?.LogInformation($"Added new repository {typeof(TEntity).FullName} and into DbContext.");

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
                InnerConnection.Close();

                Logger?.LogInformation("Closed sql connection.");
            }

            Logger?.LogInformation("DbContext finished.");
        }
    }
}
