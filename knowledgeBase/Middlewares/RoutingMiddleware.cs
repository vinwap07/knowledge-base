using System.Net;

namespace knowledgeBase.Middleware;

public class RoutingMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpListenerContext context, Func<Task> next)
    {
        throw new NotImplementedException();
    }
}