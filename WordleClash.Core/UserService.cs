using WordleClash.Core.Interfaces;

namespace WordleClash.Core;

public class UserService
{
    private IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public CreateUserResult Create()
    {
        var sessionId = new Guid().ToString();
        return _userRepository.Create(sessionId);
    }

    public User Get(string sessionId)
    {
        return _userRepository.GetBySessionId(sessionId);
    }
}