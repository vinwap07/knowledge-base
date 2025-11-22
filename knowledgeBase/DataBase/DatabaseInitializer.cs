namespace knowledgeBase.DataBase;

public class DatabaseInitializer
{
    private readonly IDatabaseConnection _connection;
    
    public DatabaseInitializer(IDatabaseConnection connection)
    {
        _connection = connection;
    }
    
    public async Task InitializeAsync(string sqlCreateFilePath, string sqlInsertFilePath)
    {
        await CreateTablesAsync(sqlCreateFilePath);
        await SeedDataAsync(sqlInsertFilePath);
    }
    
    private async Task CreateTablesAsync(string sqlCreateFilePath)
    {
        if (!File.Exists(sqlCreateFilePath))
        {
            throw new FileNotFoundException($"SQL файл не найден: {sqlCreateFilePath}");
        }

        try
        {
            using var fileStream = File.Open(sqlCreateFilePath, FileMode.Open);
            using var reader = new StreamReader(fileStream);
        
            string sql = await reader.ReadToEndAsync();
        
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new InvalidOperationException("SQL файл пуст");
            }

            await _connection.ExecuteNonQuery(sql);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при выполнении SQL скрипта: {ex.Message}", ex);
        }
    }
    
    private async Task SeedDataAsync(string sqlInsertFilePath)
    {
        if (!File.Exists(sqlInsertFilePath))
        {
            throw new FileNotFoundException($"SQL файл не найден: {sqlInsertFilePath}");
        }

        try
        {
            using var fileStream = File.Open(sqlInsertFilePath, FileMode.Open);
            using var reader = new StreamReader(fileStream);
        
            string sql = await reader.ReadToEndAsync();
        
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new InvalidOperationException("SQL файл пуст");
            }

            await _connection.ExecuteNonQuery(sql);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при выполнении SQL скрипта: {ex.Message}", ex);
        }
    }
}