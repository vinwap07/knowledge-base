using System.Net;
using System.Text;
using System.Text.Json;
using knowledgeBase.Entities;
using knowledgeBase.Services;
using knowledgeBase.View_Models;

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
	            switch (Context.Request.Url.LocalPath)
	            {
		            case "/user/login":
			            return await LoginUser();
			            break;
		            case "/user/register":
			            return await RegisterNewUser();
			            break;
		            case "/user/logout":
			            return await LogoutUser();
			            break;
		            case "user/delete":
			            return await DeleteUserProfile();
			            break;
		            case "user/update":
			            return await UpdateUserProfile();
			            break;
		            case "/user/toFavorite":
			            return await AddArticleToFavorite();
			            break;
		            case "/user/toUnFavorite":
			            return await RemoveArticleFromFavorite();
			            break;
		            case "user/favorite":
			            return await GetFavoriteArticles();
		            default:
			            throw new FormatException("Unknown HTTP method");
	            }
	            break;
            case "DELETE":
	            return await DeleteUserProfile();
                break;
            default:
	            throw new FormatException("Unknown HTTP method");
        }
    }

    private async Task<string> UpdateUserProfile()
    {
	    var body = await ReadRequestBodyAsync(Context.Request);

	    if (string.IsNullOrWhiteSpace(body))
	    {
		    throw new ArgumentException("Request body is empty");
	    }
	    
	    var cookie = Context.Request.Cookies["SessoinId"];
	    if (cookie == null)
	    {
		    return "False";
	    }
        
	    try
	    {
		    var options = new JsonSerializerOptions
		    {
			    PropertyNameCaseInsensitive = true
		    };

		    var user = JsonSerializer.Deserialize<User>(body, options);
		    if (user == null)
		    {
			    throw new ArgumentException("Invalid request body");
		    }
		    
		    return await _userService.UpdateUser(user);
	    }
	    catch (JsonException ex)
	    {
		    throw new ArgumentException($"Invalid JSON format: {ex.Message}");
	    }
    }

    private async Task<string> GetFavoriteArticles()
    {
	    var cookie = Context.Request.Cookies["SessoinId"];
	    if (cookie == null)
	    {
		    return "False";
	    }
	    
	    var articles = await _userService.GetAllArticlesBySessionId(cookie.Value);
	    return articles.Count > 0 ? JsonSerializer.Serialize(articles) : "False";
    }
    
    private async Task<string> AddArticleToFavorite()
    {
	    var query = Context.Request.QueryString;
	    var articleId = query["articleId"];
	    var cookie = Context.Request.Cookies["SessionId"];
	    var sessionId = cookie?.Value;
	    if (articleId == null || sessionId == null)
	    {
		    return "False";
	    }
	    
	    return await _userService.AddArticleToFavorite(sessionId, int.Parse(articleId)) ? "True" : "False";
    }

    private async Task<string> RemoveArticleFromFavorite()
    {
	    var query = Context.Request.QueryString;
	    var articleId = query["articleId"];
	    var cookie = Context.Request.Cookies["SessionId"];
	    var sessionId = cookie?.Value;
	    if (articleId == null || sessionId == null)
	    {
		    return "False";
	    }
	    
	    return await _userService.RemoveArticleFromFavorite(sessionId, int.Parse(articleId)) ? "True" : "False";
    }

    private async Task<string> LogoutUser()
    {
	    var cookie = Context.Request.Cookies["SessionId"];
	    if (cookie == null)
	    {
		    return "False";
	    }
	    
	    var isLogout = await _userService.LogoutUser(cookie.Value);

	    if (!isLogout)
	    {
		    throw new FormatException("Logout failed");
	    }
	    
	    cookie = new Cookie("SessionID", "")
	    {
		    Expires = DateTime.Now.AddDays(-1),
		    Path = "/"
	    };
	    
	    Context.Response.Cookies.Add(cookie);
	    Context.Response.StatusCode = (int)HttpStatusCode.OK;
	    return "True";
    }

    private async Task<string> LoginUser()
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
		    var userProfile = await _userService.Authenticate(user.Email, user.Password);
		    var sessionId = await _userService.CreateSession(user.Email);
		    SetSessionCookie(sessionId);
			
		    return userProfile.Email != string.Empty ? JsonSerializer.Serialize(userProfile) : "Failed";
	    }
	    catch (JsonException ex)
	    {
		    throw new ArgumentException($"Invalid JSON format: {ex.Message}");
	    }
    }

    private async Task<string> DeleteUserProfile()
    {
	    var cookie = Context.Request.Cookies["SessionId"];
	    var isDeleted = cookie != null && await _userService.DeleteUserProfile(cookie.Value);
	    return isDeleted ? "True" : "False";
    }

    private async Task<string> GetUserProfile()
    {
	    var cookie = Context.Request.Cookies["SessionId"];
	    var sessionId = cookie?.Value;
	    var userProfile = cookie != null ? await _userService.GetUserProfileBySessionId(cookie.Value) : null;
	    return userProfile != null ? JsonSerializer.Serialize(userProfile) : "False";
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
		    if (user == null)
		    {
			    throw new ArgumentException("Invalid request body");
		    }
		    
		    var sessionId = await _userService.RegisterNewUser(user) ? await _userService.CreateSession(user.Email) : null;
		    if (sessionId != null)
		    {
			    SetSessionCookie(sessionId);
			    var userProfile = await _userService.GetUserProfileBySessionId(sessionId);
			    return JsonSerializer.Serialize(userProfile);
		    }

		    return "False";
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