using System.Net;

namespace knowledgeBase.Controllers;

public abstract class BaseController
{
    public abstract Task<string> HandleRequest();
    protected string Json(object data);
    protected string View(string templateName, object model);
    protected void Redirect(string url);
}