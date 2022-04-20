using System.Data;

namespace Gateway.API.Infrastructure.Dapper
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
    }
}