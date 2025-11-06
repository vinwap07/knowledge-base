using System.Net;
using knowledgeBase.Services;

namespace knowledgeBase.Controllers;

public class AIController : BaseController
{
    public HttpListenerContext Context { get; set; }
    private AiService _aiService;
    private UserService _userService;
    
    public string Ask(Dictionary<string, object> parameters);
    public string ProcessQuestion(Dictionary<string, object> parameters);
    public string SubmitFeedback(Dictionary<string, object> parameters);
    
    public async override Task<string> HandleRequest()
    {
        throw new NotImplementedException();
    }
}
