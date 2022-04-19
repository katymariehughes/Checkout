using Microsoft.Data.SqlClient;
using System.Data;

namespace Gateway.API.Infrastructure.Dapper
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("Database");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
