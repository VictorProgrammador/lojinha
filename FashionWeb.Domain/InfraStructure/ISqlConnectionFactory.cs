using System.Data;

namespace FashionWeb.Domain.InfraStructure
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
