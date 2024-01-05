using Microsoft.Data.SqlClient;

namespace SampleDotnetCleanArchitecture.Infrastructure.SqlServer
{
    internal static class SqlCommandExtensions
    {
        public static void AddParameter(this SqlCommand command, string name, object value)
        {
            if (command == null) return;

            var parameter = command.CreateParameter();
            parameter.ParameterName = name;

            if (value != null)
            {
                parameter.Value = value;
            }
            else
            {
                parameter.Value = DBNull.Value;
                parameter.IsNullable = true;
            }

            command.Parameters.Add(parameter);
        }
    }
}
