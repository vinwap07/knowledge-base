using System.Text.Json;

namespace knowledgeBase;

public class Config
{
    public string DatabaseConnectionString { get; set; }
    public string ApiKey { get; set; }
    public string StaticFilesPath { get; set; }
    public int Port { get; set; }

    public Config(string databaseConnectionString, string apiKey, string staticFilesPath, int port)
    {
        DatabaseConnectionString = databaseConnectionString;
        ApiKey = apiKey;
        StaticFilesPath = staticFilesPath;
        Port = port;
    }
    
    public static Config FromFile(string configPath)
    {
        string json = File.ReadAllText(configPath);
        return JsonSerializer.Deserialize<Config>(json);
    }
}

