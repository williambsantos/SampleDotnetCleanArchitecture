using System.Data;
using System.Data.Common;
using System.Reflection;

namespace SampleDotnetCleanArchitecture.Infrastructure.SqlServer.Tests.Repositories
{
    public static class DataTableExtensions
    {
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                if (string.IsNullOrWhiteSpace(prop.Name))
                    continue;

                //Setting column names as Property names
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    _ = dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType));
                else
                    dataTable.Columns.Add(prop.Name, prop.PropertyType);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    if (Props[i] == null)
                        continue;

                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static DbDataReader ToDataReader<T>(this List<T> items) =>
            items.ToDataTable().CreateDataReader();
    }
}
