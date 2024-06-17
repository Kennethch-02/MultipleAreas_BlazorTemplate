using System.Data;
using System.Data.SqlClient;

namespace MultipleAreas_BlazorTemplate.Data
{
    public class DataBaseService
    {
        private readonly IConfiguration? _configuration;
        private readonly string? _connectionString;

        public DataBaseService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public DataTable ExecuteStoredProcedure(string storedProcedureName, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    var dataTable = new DataTable();
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }

                    return dataTable;
                }
            }
        }

        public void ExecuteBulkCopy(DataTable table, string destinationTableName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = destinationTableName;
                    bulkCopy.WriteToServer(table);
                }
            }
        }

        public void ExecuteScript(string script)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(script, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
