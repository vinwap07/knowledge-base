using System.Data;
using Npgsql;

namespace knowledgeBase.DataBase;

public class PostgresDbConnection : IDatabaseConnection, IDisposable
{
    private readonly string _connectionString;
    private NpgsqlConnection _connection;

    public PostgresDbConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task OpenAsync()
    {
        await _connection.OpenAsync();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    public async Task<IDataReader> ExecuteReader(string sql, Dictionary<string, object> parameters = null)
    {
        await EnsureConnectionOpen();
        using var command = CreateCommand(sql, parameters);
        return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
    }

    public async Task<int> ExecuteNonQuery(string sql, Dictionary<string, object> parameters = null)
    {
        await EnsureConnectionOpen();
        using var connection = CreateConnection();
        using var command = CreateCommand(sql, parameters, connection);
        return await command.ExecuteNonQueryAsync();
    }

    public async Task<object> ExecuteScalar(string sql, Dictionary<string, object> parameters = null)
    {
        await EnsureConnectionOpen();
        using var connection = CreateConnection();
        using var command = CreateCommand(sql, parameters, connection);
        return await command.ExecuteScalarAsync();
    }

    private NpgsqlConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
    
    private async Task EnsureConnectionOpen()
    {
        if (_connection == null)
        {
            _connection = new NpgsqlConnection(_connectionString);
        }
        
        if (_connection.State != ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
    }

    private NpgsqlCommand CreateCommand(string sql, Dictionary<string, object> parameters, NpgsqlConnection conn = null)
    {
        var cmd = (conn ?? _connection).CreateCommand();
        cmd.CommandText = sql;

        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
            }
        }
        
        return cmd;
    }
}