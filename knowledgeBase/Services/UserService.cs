using knowledgeBase.Entities;
using knowledgeBase.Repositories;
using knowledgeBase.View_Models;

namespace knowledgeBase.Services;

public class UserService
{
    private UserRepository _userRepository;

    public bool RegisterNewEmployee(User user)
    {
        
    }

    public User Authenticate(string email, string password)
    {
        
    }

    public bool CanAccessArticle(int userId, int articleId)
    {
        
    }

    public UserProfile GetUserProfile(int userId)
    {
        
    }
}