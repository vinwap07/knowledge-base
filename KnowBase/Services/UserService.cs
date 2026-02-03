using System.Text.Json;
using knowledgeBase.Entities;
using knowledgeBase.Repositories;
using knowledgeBase.View_Models;

namespace knowledgeBase.Services;

public class UserService
{
    private UserRepository _userRepository;
    private SessionRepository _sessionRepository;
    private UserRegisterValidator _userRegisterValidator = new UserRegisterValidator();
    private UserLoginValidator _userLoginValidator = new UserLoginValidator();

    public UserService(UserRepository userRepository, SessionRepository sessionRepository)
    {
        _userRepository = userRepository;
        _sessionRepository = sessionRepository;
    }

    public async Task<string> RegisterNewUser(User user)
    {
        var validationResult = await _userRegisterValidator.ValidateAsync(user);

        if (validationResult is { IsValid: false })
        {
            return validationResult.Errors.First().ToString();
        }

        user.RoleId = 1;
        user.Password = PasswordHasher.Hash(user.Password);
        
        var isExistst = (await _userRepository.GetById(user.Email)).Name != null;
        if (!isExistst)
        {
            if (await _userRepository.Create(user))
            {
                return "OK";
            }
        }
        return "Email already exists";
    }

    public async Task<bool> UpdateUser(User user)
    { 
        var isUpdated = await _userRepository.Update(user);
        return isUpdated;
    }

    public async Task<bool> DeleteUserProfile(string sessionId)
    {
        await _sessionRepository.Delete(sessionId);
        var user = await _sessionRepository.GetUserBySessionId(sessionId);
        return await _userRepository.Delete(user.Email);
    }
    
    public async Task<bool> LogoutUser(string sessionId)
    {
        return await _sessionRepository.Delete(sessionId);
    }

    public async Task<(bool, string, User)> Authenticate(User incomingUser)
    {
        var validationResult = await _userRegisterValidator.ValidateAsync(incomingUser);
        var result = string.Empty;
        
        if (validationResult is { IsValid: false })
        {
            result = validationResult.Errors.First().ToString();
        }
        
        var isAuth = false;
        var user = await _userRepository.GetById(incomingUser.Email);
        if (user.Email != null && user.Email != string.Empty)
        {
            if (PasswordHasher.Validate(user.Password, incomingUser.Password))
            {
                isAuth = true;
            }
            else
            {
                result = "Неправильный пароль или почтовый адрес";
            }
        }
        
        return (isAuth, result, user);
    }

    public async Task<User> GetUserById(string email)
    {
        var user = await _userRepository.GetById(email);
        return user;
    }

    public async Task<string> GetUserRole(User user)
    {
        var role = await _userRepository.GetRoleByEmail(user.Email);
        return role;
    }

    public async Task<string> GetRoleBySessionId(string sessionId)
    {
        var user = await _sessionRepository.GetUserBySessionId(sessionId);
        if (user == null)
        {
            return "unknown";
        }
        
        var role = await _userRepository.GetRoleByRoleId(user.RoleId);
        return role;
    }

    public async Task<User> GetUserBySessionId(string sessionId)
    {
        var user = await _sessionRepository.GetUserBySessionId(sessionId);
        return user;
    }

    public async Task<string> CreateSession(User user)
    {
        var sessionId = Guid.NewGuid().ToString() + "-" + DateTime.Now.Ticks;
        var endTime = DateTime.UtcNow.AddHours(1);
        
        _sessionRepository.Create(new Session() {SesisonId = sessionId, UserEmail = user.Email, EndTime = endTime});
        return sessionId;
    }

    public async Task<bool> AddModerator(string email, string role)
    {
        if (role != "Admin")
        {
            return false;
        }
        var user = await GetUserById(email);
        user.RoleId = 3;
        var isChanged = await UpdateUser(user);
        return isChanged;
    }
}