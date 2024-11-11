using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Scanner.Helpers
{
    public class DatabaseHelper
    {
        private readonly string _baseConnectionString;

        public DatabaseHelper(IConfiguration config)
        {
            _baseConnectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>
        (string storedProcedureName, string server, string databaseName, string userId, string password , object parameters = null, bool isQuery = true)
        {
            // Replace the placeholders in the base connection string with the provided data
            var connectionString = _baseConnectionString
                .Replace("{SERVER}", server)
                .Replace("{DB_NAME}", databaseName)
                .Replace("{USER_ID}", userId)
                .Replace("{PASSWORD}", password);

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