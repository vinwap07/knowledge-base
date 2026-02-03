using System.Data;

namespace knowledgeBase.DataBase;

public interface IDatabaseConnection
{
    Task<IDataReader> ExecuteReader(string sql, Dictionary<string, object> parameters = null);
    Task<int> ExecuteNonQuery(string sql, Dictionary<string, object> parameters = null);
    Task<object> ExecuteScalar(string sql, Dictionary<string, object> parameters = null);
}