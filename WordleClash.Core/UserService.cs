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
        var sessionId = Guid.NewGuid().ToString();
        return _userRepository.Create(sessionId);
    }

    public User? Get(string name)
    {
        try
        {
            return _userRepository.GetByName(name);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}