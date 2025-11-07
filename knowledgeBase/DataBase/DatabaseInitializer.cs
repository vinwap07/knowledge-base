namespace knowledgeBase.DataBase;

public class DatabaseInitializer
{
    private readonly IDatabaseConnection _connection;
    
    public DatabaseInitializer(IDatabaseConnection connection)
    {
        _connection = connection;
    }
    
    public async Task InitializeAsync()
    {
        await CreateTablesAsync();
        await SeedDataAsync();
    }
    
    private async Task CreateTablesAsync()
    {
        var file = File.Open("/createDB.sql", FileMode.Open);
        var reader = new StreamReader(file);
        string sql = reader.ReadToEnd();
        
        await _connection.ExecuteNonQuery(sql);
    }
    
    private async Task SeedDataAsync()
    {
        var result = await _connection.ExecuteScalar(
            "SELECT COUNT(*) FROM Users");
        bool isInt = int.TryParse((string?)result, out var userCount);
        
        if (userCount == 0)
        {
            var file = File.Open("/addEntities.sql", FileMode.Open);
            var reader = new StreamReader(file);
            string sql = reader.ReadToEnd();
        
            await _connection.ExecuteNonQuery(sql);
        }
    }
}