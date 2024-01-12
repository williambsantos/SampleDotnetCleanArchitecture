using System.Data;
using System.Data.Common;
using SampleDotnetCleanArchitecture.Infrastructure.SqlServer.Interfaces;

namespace SampleDotnetCleanArchitecture.Infrastructure.SqlServer.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private Lazy<DbConnection> _dbConnectionLazy;

        public DatabaseRepository(DbConnection dbConnection)
        {
            if (dbConnection == null)
                throw new ArgumentNullException(paramName: nameof(dbConnection));

            _dbConnectionLazy = new(() =>
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                return dbConnection;
            });
        }

        public void Dispose()
        {
            if (_dbConnectionLazy.IsValueCreated)
                _dbConnectionLazy.Value.Dispose();
        }

        public async Task<DbDataReader> ExecuteReaderAsync(string commandText, CommandType commandType, params DbParameter[]? parameters)
        {
            var command = InternalCreateCommand(commandText, commandType, parameters);
            return await command.ExecuteReaderAsync();
        }

        private DbCommand InternalCreateCommand(string commandText, CommandType commandType, DbParameter[]? parameters)
        {
            var command = _dbConnectionLazy.Value.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            if (parameters != null)
                command.Parameters.AddRange(parameters);
            return command;
        }

        public async Task<object?> ExecuteScalarAsync(string commandText, CommandType commandType, params DbParameter[]? parameters)
        {
            var command = InternalCreateCommand(commandText, commandType, parameters);
            return await command.ExecuteScalarAsync();
        }

        public async Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType, params DbParameter[]? parameters)
        {
            var command = InternalCreateCommand(commandText, commandType, parameters);
            return await command.ExecuteNonQueryAsync();
        }
    }
}
