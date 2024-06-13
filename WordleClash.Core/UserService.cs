using WordleClash.Core.Interfaces;

namespace WordleClash.Core;

public class UserService
{
    private IUserRepository _userRepository;
    private IGameLogRepository _gameLogRepository;
    
    public UserService(IUserRepository userRepository, IGameLogRepository gameLogRepository)
    {
        _userRepository = userRepository;
        _gameLogRepository = gameLogRepository;
    }

    public CreateUserResult Create()
    {
        var sessionId = Guid.NewGuid().ToString();
        return _userRepository.Create(sessionId, GenerateName(sessionId));
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

    public User? GetFromSession(string sessionId)
    {
        try
        {
            return _userRepository.GetFromSessionId(sessionId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public void AddGameLog(GameLog log, string sessionId)
    {
        _gameLogRepository.AddToUser(log, sessionId);
    }
    
    private string GenerateName(string id)
    {
        var shortenedId = id.Substring(Math.Max(0, id.Length - 10));
        return $"user-{shortenedId}";
    }
}