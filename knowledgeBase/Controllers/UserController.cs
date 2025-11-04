using knowledgeBase.Services;

namespace knowledgeBase.Controllers;

public class UserController : BaseController
{
    private UserService _userService;
    
    public string Profile(Dictionary<string, object> parameters);
    public string Login(Dictionary<string, object> parameters);
    public string Authenticate(Dictionary<string, object> parameters);
    public string Logout(Dictionary<string, object> parameters);
    
    public override string HandleRequest(Dictionary<string, object> parameters)
    {
        throw new NotImplementedException();
    }
}