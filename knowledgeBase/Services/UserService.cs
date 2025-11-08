using knowledgeBase.Entities;
using knowledgeBase.Repositories;
using knowledgeBase.View_Models;

namespace knowledgeBase.Services;

public class UserService
{
    private UserRepository _userRepository;
    private RoleRepository _roleRepository;
    private SessionRepository _sessionRepository;

    public async Task<bool> RegisterNewUser(User user)
    {
        // TODO: валидация данных 
        return await _userRepository.Create(user);
    }

    public async Task<bool> DeleteUserProfile(string sessionId)
    {
        await _sessionRepository.Delete(sessionId);
        var user = await _sessionRepository.GetUserBySessionId(sessionId);
        return await _userRepository.Delete(user.Email);
    }

    public async Task<User> Authenticate(string email, string password)
    {
        var user = await _userRepository.GetById(email);

        if (user != null)
        {
            if (user.Password == password)
            {
                return user;
            }
            
            return null;
        }
        
        return null;
    }

    public async Task<UserProfile> GetUserProfileBySessionId(string id)
    {
        var user = await _sessionRepository.GetUserBySessionId(id);
        var userProfile = new UserProfile() { Email = user.Email, Name = user.Name };
        
        var role = await _roleRepository.GetById(user.RoleId);
        userProfile.Role = role.Name;
        
        return userProfile;
    }

    public async Task<string> CreateSession(string clientEmail)
    {
        var sessionId = Guid.NewGuid().ToString() + "-" + DateTime.Now.Ticks;
        var endTime = DateTime.UtcNow.AddHours(1);
        
        _sessionRepository.Create(new Session() {SesisonId = sessionId, User = clientEmail, EndTime = endTime});
        return sessionId;
    }
}