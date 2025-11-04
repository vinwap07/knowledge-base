namespace knowledgeBase;

public class Router
{
    private Dictionary<string, Route> _routes;
    
    // Регистрирует маршрут (URL -> контроллер/действие)
    public void RegisterRoute(string pattern, string controller, string action)
    {
        
    }
    
    // Находит подходящий маршрут для URL
    public Route MatchRoute(string url)
    {
        
    }
    
    // Извлекает параметры из URL (/articles/123 -> id=123)
    public Dictionary<string, object> ExtractParameters(string url, Route route)
    {
        
    }
}

public class Route
{
    public string Pattern { get; set; }
    public string Controller { get; set; }
    public string Action { get; set; }
}