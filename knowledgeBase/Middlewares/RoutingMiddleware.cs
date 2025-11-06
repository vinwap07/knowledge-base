using System.Net;
using System.Text;
using knowledgeBase.Controllers;
namespace knowledgeBase.Middleware;

public class RoutingMiddleware : IMiddleware
{
    
    public async Task InvokeAsync(HttpListenerContext context, Func<Task> next)
    {
        var request = context.Request;
        string path = request.Url.LocalPath;
        string controllerPath = (path.Substring(1, path.Length - 1)).Substring(0, path.IndexOf('/', 1) - 1);

        BaseController controller = controllerPath switch
        {
            "home" => new HomeController { Context = context },
            "article" => new ArticleController { Context = context },
            "llm" => new AIController { Context = context },
            "user" => new UserController { Context = context },
            _ => throw new FileNotFoundException()
        };
        
        var result = await controller.HandleRequest();
    }
}

