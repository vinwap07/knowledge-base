using System.Net;
using System.Text;
using System.Text.Json;
using knowledgeBase.Entities;
using knowledgeBase.Services;

namespace knowledgeBase.Controllers;

public class UserController : BaseController
{
    public HttpListenerContext Context { get; set; }
    private UserService _userService;
    
    public async override Task<string> HandleRequest()
    {
        switch (Context.Request.HttpMethod)
        {
            case "GET":
	            return await GetUserProfile();
                break;
            case "POST":
	            return await RegisterNewUser();
                break;
            case "DELETE":
	            return await DeleteUserProfile();
                break;
            default:
	            throw new FormatException("Unknown HTTP method");
        }
    }

    private async Task<string> DeleteUserProfile()
    {
	    var cookie = Context.Request.Cookies["SessionId"];
	    var sessionId = cookie?.Value;
	    
	    var IsDeleted = await _userService.DeleteUserProfile(sessionId);
	    return IsDeleted ? "True" : "False";
    }

    private async Task<string> GetUserProfile()
    {
	    var cookie = Context.Request.Cookies["SessionId"];
	    var sessionId = cookie?.Value;
		
	    var userProfile = await _userService.GetUserProfileBySessionId(sessionId);
	    if (userProfile.Email == string.Empty)
	    {
		    return "False";
	    }

	    return JsonSerializer.Serialize(userProfile);
    }

    private async Task<string> RegisterNewUser()
    {
        var body = await ReadRequestBodyAsync(Context.Request);

        if (string.IsNullOrWhiteSpace(body))
        {
            throw new ArgumentException("Request body is empty");
        }
        
        try
	    {
		    var options = new JsonSerializerOptions
		    {
			    PropertyNameCaseInsensitive = true
		    };

		    var user = JsonSerializer.Deserialize<User>(body, options);
		    _userService.RegisterNewUser(user);
		    var sessionId = await _userService.CreateSession(user.Email);
		    SetSessionCookie(sessionId);

		    return "True";
	    }
	    catch (JsonException ex)
	    {
		    throw new ArgumentException($"Invalid JSON format: {ex.Message}");
	    }
    }
    
    private void SetSessionCookie(string sessionId)
    {
	    var expires = DateTime.Now.AddMinutes(60).ToString("R");
	    var cookieValue = $"SessionID={sessionId}; Expires={expires}; Path=/; HttpOnly; SameSite=Strict";
    
	    Context.Response.Headers.Add("Set-Cookie", cookieValue);
    }
}