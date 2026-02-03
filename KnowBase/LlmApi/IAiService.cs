namespace knowledgeBase;

public interface IAiService
{
    Task<string> SendRequest(string text);
}