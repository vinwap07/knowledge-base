using System.Net;

namespace knowledgeBase.Middleware;

public interface IMiddleware
{
    Task InvokeAsync(HttpListenerContext context, Func<Task> next);
}