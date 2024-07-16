using System.Data;
using Npgsql;

namespace Camel.Bancho.Data;

public class NpgsqlConnectionProvider : IDbConnectionProvider
{
    private readonly string _connectionString;
    
    public NpgsqlConnectionProvider(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SqlConnection");
    }
    
    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}