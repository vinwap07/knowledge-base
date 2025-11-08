using System.Net;
using knowledgeBase.Services;

namespace knowledgeBase.Controllers;

public class ArticleController : BaseController
{
    public HttpListenerContext Context { get; set; }
    
    private ArticleService _articleService;
    public async override Task<string> HandleRequest()
    {
        switch (Context.Request.HttpMethod)
        {
            case "GET":
                if (Context.Request.Url.LocalPath == "/article/favorite")
                {
                    
                }
                else if (Context.Request.Url.LocalPath == "/article/all")
                {
                    
                }
                else
                {
                    
                }
                break;
            case "POST":
                if (Context.Request.Url.LocalPath == "/article/toFavorite")
                {
                    return await AddAToFavorite();
                }
                else if (Context.Request.Url.LocalPath == "/article/toUnfavorite")
                {
                    return await RemoveFromFavorite();
                }
                break;
            case "DELETE":
                
                break;
            default:
                
        }
    }

    private async Task<string> AddAToFavorite()
    {
        var query = Context.Request.QueryString;
        var articleId = query["articleId"];
        var cookie = Context.Request.Cookies["SessionId"];
        var sessionId = cookie?.Value;
        
        return await _articleService.AddToFavorite(int.Parse(articleId), sessionId) ? "True" : "False";
    }

    private async Task<string> RemoveFromFavorite()
    {
        var query = Context.Request.QueryString;
        var articleId = query["articleId"];
        var cookie = Context.Request.Cookies["SessionId"];
        var sessionId = cookie?.Value;
        
        return await _articleService.RemoveFromFavorite(int.Parse(articleId), sessionId) ? "True" : "False";
    }
}