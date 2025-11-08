using System.Net;
using System.Text.Json;

namespace knowledgeBase.Controllers;

public abstract class BaseController
{
    public HttpListenerContext Context { get; set; }
    public abstract Task<string> HandleRequest();
    protected string ToJson(object data)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        
        return JsonSerializer.Serialize(data, options);
    }
    protected void Redirect(string url)
    {
        Context.Response.Redirect(url);
    }
    
    protected async Task<string> ReadRequestBodyAsync(HttpListenerRequest request)
    {
        try
        {
            using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
            return await reader.ReadToEndAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to read request body", ex);
        }
    }
}