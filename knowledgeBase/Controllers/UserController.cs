using System.Net;
using knowledgeBase.Services;

namespace knowledgeBase.Controllers;

public class UserController : BaseController
{
    public HttpListenerContext Context { get; set; }
    private UserService _userService;
    
    public string Profile(Dictionary<string, object> parameters);
    public string Login(Dictionary<string, object> parameters);
    public string Authenticate(Dictionary<string, object> parameters);
    public string Logout(Dictionary<string, object> parameters);
    
    public async override Task<string> HandleRequest()
    {
        throw new NotImplementedException();
    }
}