using knowledgeBase.Services;

namespace knowledgeBase.Controllers;

public class ArticleController : BaseController
{
    private ArticleService _articleService;
    
    public string Show(Dictionary<string, object> parameters);
    public string Index(Dictionary<string, object> parameters);
    public string Search(Dictionary<string, object> parameters);
    public string Create(Dictionary<string, object> parameters);
    public string Store(Dictionary<string, object> parameters);
    
    public override string HandleRequest(Dictionary<string, object> parameters)
    {
        throw new NotImplementedException();
    }
}