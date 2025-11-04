using System.Net;

namespace knowledgeBase.Middleware;

public class BodyParsingMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpListenerContext context, Func<Task> next)
    {
        throw new NotImplementedException();
    }
}