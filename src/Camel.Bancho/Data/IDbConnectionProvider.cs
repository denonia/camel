using System.Data;

namespace Camel.Bancho.Data;

public interface IDbConnectionProvider
{
    public IDbConnection CreateConnection();
}