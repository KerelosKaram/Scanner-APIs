using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TestBaseCopy.Helpers
{
    public class DatabaseHelper
{
    private readonly string _baseConnectionString;

    public DatabaseHelper(IConfiguration config)
    {
        _baseConnectionString = config.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, string databaseName, object parameters = null, bool isQuery = true)
    {
        // Replace the {DBName} placeholder in the base connection string with the provided database name
        var connectionString = _baseConnectionString.Replace("{DB_NAME}", databaseName);

        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            if (isQuery)
            {
                // Use QueryAsync for SELECT operations (returns data)
                var result = await connection.QueryAsync<T>(
                    storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                    return result;
            }
            else
            {
                // Use ExecuteAsync for INSERT, UPDATE, DELETE operations (returns affected rows count)
                await connection.ExecuteAsync(
                    storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
                return Enumerable.Empty<T>(); // Return an empty result for commands
            }
        }
    }
}

}