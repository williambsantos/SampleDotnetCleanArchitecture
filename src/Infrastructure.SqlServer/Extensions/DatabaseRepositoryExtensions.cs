using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace SampleDotnetCleanArchitecture.Infrastructure.SqlServer.Extensions
{
    public static class SqlCommandExtensions
    {
        public static void AddParameter(this ICollection<DbParameter> parameters, string name, object? value)
        {
            if (parameters == null) return;

            var parameter = new SqlParameter
            {
                ParameterName = name
            };

            if (value != null)
            {
                parameter.Value = value;
            }
            else
            {
                parameter.Value = DBNull.Value;
                parameter.IsNullable = true;
            }

            parameters.Add(parameter);
        }
    }
}
