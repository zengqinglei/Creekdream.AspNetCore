using Creekdream.Orm.Dapper;
using Creekdream.Uow;
using DapperExtensions;
using DapperExtensions.Sql;
using System;
using System.Data.Common;

namespace Creekdream.Orm.Uow
{
    /// <inheritdoc />
    public class DatabaseProvider : IDatabaseProvider
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly DapperOptionsBuilder _dapperOptionsBuilder;

        /// <inheritdoc />
        public DatabaseProvider(
            IUnitOfWorkManager unitOfWorkManager,
            DapperOptionsBuilder dapperOptionsBuilder)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _dapperOptionsBuilder = dapperOptionsBuilder;
        }

        /// <inheritdoc />
        public IDatabase GetDatabase()
        {
            var unitOfWork = _unitOfWorkManager.Current;
            if (unitOfWork == null)
            {
                throw new Exception("A DbContext can only be created inside a unit of work!");
            }

            var databaseApi = unitOfWork.GetOrAddDatabaseApi(
                () => new DatabaseApi(
                    CreateDatabase(unitOfWork)
                ));

            return ((DatabaseApi)databaseApi).Database;
        }

        /// <inheritdoc />
        public DbTransaction GetDbTransaction()
        {
            var unitOfWork = _unitOfWorkManager.Current;
            if (unitOfWork == null)
            {
                throw new Exception("A DbContext can only be created inside a unit of work!");
            }

            return ((TransactionApi)unitOfWork.FindTransactionApi())?.DbTransaction;
        }

        private IDatabase CreateDatabase()
        {
            var config = new DapperExtensionsConfiguration(
                        _dapperOptionsBuilder.DefaultMapper,
                        _dapperOptionsBuilder.MapperAssemblies,
                        _dapperOptionsBuilder.SqlDialect);
            var sqlGenerator = new SqlGeneratorImpl(config);
            return new Database(_dapperOptionsBuilder.GetDbConnection(), sqlGenerator);
        }

        private IDatabase CreateDatabase(IUnitOfWork unitOfWork)
        {
            var creationContext = new DatabaseCreationContext();
            using (DatabaseCreationContext.Use(creationContext))
            {
                var database = CreateDatabase();
                if (unitOfWork.Options.IsTransactional)
                {
                    var dbtransaction = unitOfWork.Options.IsolationLevel.HasValue
                    ? database.Connection.BeginTransaction(unitOfWork.Options.IsolationLevel.Value)
                    : database.Connection.BeginTransaction();

                    unitOfWork.AddTransactionApi(
                        new TransactionApi(
                            dbtransaction,
                            database
                        )
                    );
                }

                return database;
            }
        }
    }
}
