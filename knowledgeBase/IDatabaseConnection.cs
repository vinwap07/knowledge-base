using System.Data;

namespace knowledgeBase;

public interface IDatabaseConnection
{
    IDataReader ExecuteReader(string sql, Dictionary<string, object> parameters = null);
    int ExecuteNonQuery(string sql, Dictionary<string, object> parameters = null);
    object ExecuteScalar(string sql, Dictionary<string, object> parameters = null);
}