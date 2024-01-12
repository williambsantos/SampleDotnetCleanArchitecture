using System.Data;
using System.Data.Common;

namespace SampleDotnetCleanArchitecture.Infrastructure.SqlServer.Interfaces
{
    public interface IDatabaseRepository : IDisposable
    {
        Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType, params DbParameter[]? parameters);
        Task<object?> ExecuteScalarAsync(string commandText, CommandType commandType, params DbParameter[]? parameters);
        Task<DbDataReader> ExecuteReaderAsync(string commandText, CommandType commandType, params DbParameter[]? parameters);
    }
}
