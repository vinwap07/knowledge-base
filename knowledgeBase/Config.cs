namespace knowledgeBase;

public class Config
{
    public string DatabaseConnectionString { get; set; }
    public string ApiKey { get; set; }
    public string LogFilePath { get; set; }
    public string LogErrorFilePath { get; set; }
    public string StaticFilesPath { get; set; }
    public string TemplatesPath { get; set; }
}