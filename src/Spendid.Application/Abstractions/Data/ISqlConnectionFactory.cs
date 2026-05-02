using System.Data;

namespace Spendid.Application.Abstractions.Data;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection(); 
}
