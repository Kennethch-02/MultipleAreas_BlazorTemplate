using System.Data;
using System.Text;

namespace MultipleAreas_BlazorTemplate.Extensions
{
    public static class DataTableExtensions
    {
        // 1. Convertir DataTable a una lista de un tipo genérico
        public static List<T> ToList<T>(this DataTable dataTable) where T : new()
        {
            var properties = typeof(T).GetProperties();
            var result = new List<T>();

            foreach (DataRow row in dataTable.Rows)
            {
                var item = new T();
                foreach (var property in properties)
                {
                    if (dataTable.Columns.Contains(property.Name))
                    {
                        property.SetValue(item, Convert.ChangeType(row[property.Name], property.PropertyType));
                    }
                }
                result.Add(item);
            }
            return result;
        }

        // 2. Verificar si el DataTable está vacío
        public static bool IsEmpty(this DataTable dataTable)
        {
            return dataTable == null || dataTable.Rows.Count == 0;
        }

        // 3. Obtener una columna específica como una lista
        public static List<T> GetColumn<T>(this DataTable dataTable, string columnName)
        {
            var columnData = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                if (row[columnName] != DBNull.Value)
                {
                    columnData.Add((T)Convert.ChangeType(row[columnName], typeof(T)));
                }
            }
            return columnData;
        }

        // 4. Añadir una columna a un DataTable existente
        public static void AddColumn(this DataTable dataTable, string columnName, Type columnType)
        {
            if (!dataTable.Columns.Contains(columnName))
            {
                dataTable.Columns.Add(columnName, columnType);
            }
        }

        // 5. Fusionar dos DataTables
        public static DataTable Merge(this DataTable dataTable1, DataTable dataTable2)
        {
            var result = dataTable1.Copy();
            foreach (DataRow row in dataTable2.Rows)
            {
                result.ImportRow(row);
            }
            return result;
        }

        // 6. Filtrar DataTable basado en una expresión
        public static DataTable Filter(this DataTable dataTable, string filterExpression)
        {
            var dataView = new DataView(dataTable) { RowFilter = filterExpression };
            return dataView.ToTable();
        }

        // 7. Ordenar DataTable basado en una expresión
        public static DataTable Sort(this DataTable dataTable, string sortExpression)
        {
            var dataView = new DataView(dataTable) { Sort = sortExpression };
            return dataView.ToTable();
        }

        // 8. Clonar estructura de DataTable sin datos
        public static DataTable CloneStructure(this DataTable dataTable)
        {
            return dataTable.Clone();
        }

        // 9. Remover filas duplicadas basado en una columna
        public static DataTable RemoveDuplicates(this DataTable dataTable, string columnName)
        {
            var distinctTable = dataTable.Clone();
            var distinctRows = dataTable.AsEnumerable()
                                        .GroupBy(row => row[columnName])
                                        .Select(group => group.First());

            foreach (var row in distinctRows)
            {
                distinctTable.ImportRow(row);
            }
            return distinctTable;
        }

        // 10. Convertir DataTable a CSV
        public static string ToCsv(this DataTable dataTable, char delimiter = ',')
        {
            var csvData = new StringBuilder();
            var columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            csvData.AppendLine(string.Join(delimiter, columnNames));

            foreach (DataRow row in dataTable.Rows)
            {
                var fields = row.ItemArray.Select(field => field?.ToString());
                csvData.AppendLine(string.Join(delimiter, fields));
            }

            return csvData.ToString();
        }
    }
}
