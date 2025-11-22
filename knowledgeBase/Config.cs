using System.Text.Json;

namespace knowledgeBase;

public class Config
{
    public string DatabaseConnectionString { get; set; }
    public string ApiKey { get; set; }
    public string LogFilePath { get; set; }
    public string LogErrorFilePath { get; set; }
    public string StaticFilesPath { get; set; }
    public string TemplatesPath { get; set; }

    public Config(string databaseConnectionString, string logFilePath, string logErrorFilePath, string staticFilesPath,
        string templatesPath)
    {
        DatabaseConnectionString = databaseConnectionString;
        LogFilePath = logFilePath;
        LogErrorFilePath = logErrorFilePath;
        StaticFilesPath = staticFilesPath;
        TemplatesPath = templatesPath;
    }
    
    public static Config FromFile(string configPath)
    {
        string json = File.ReadAllText(configPath);
        return JsonSerializer.Deserialize<Config>(json);
    }
}

