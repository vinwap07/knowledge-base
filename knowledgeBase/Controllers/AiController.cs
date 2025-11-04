using knowledgeBase.Services;

namespace knowledgeBase.Controllers;

public class AIController : BaseController
{
    private AiService _aiService;
    private UserService _userService;
    
    public string Ask(Dictionary<string, object> parameters);
    public string ProcessQuestion(Dictionary<string, object> parameters);
    public string SubmitFeedback(Dictionary<string, object> parameters);
    
    public override string HandleRequest(Dictionary<string, object> parameters)
    {
        throw new NotImplementedException();
    }
}
