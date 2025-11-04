using System.Net;

namespace knowledgeBase.Controllers;

public abstract class BaseController
{
    protected HttpListenerContext _context;
    public abstract string HandleRequest(Dictionary<string, object> parameters);
    protected string Json(object data);
    protected string View(string templateName, object model);
    protected void Redirect(string url);
}