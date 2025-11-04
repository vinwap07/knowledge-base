using System.Net;

namespace knowledgeBase.Middleware;

public class AuthenticationMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpListenerContext context, Func<Task> next)
    {
        throw new NotImplementedException();
    }
}