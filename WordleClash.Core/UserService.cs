using System.Text.RegularExpressions;
using WordleClash.Core.Exceptions;
using WordleClash.Core.Interfaces;

namespace WordleClash.Core;

public class UserService
{
    private IUserRepository _userRepository;
    private IGameLogRepository _gameLogRepository;
    private const int MaxUsernameLength = 30; //defined as VARCHAR(30) in database
    private const string UsernameRegexPattern = @"^[a-zA-Z0-9._^*()!$]+$";
    
    public UserService(IUserRepository userRepository, IGameLogRepository gameLogRepository)
    {
        _userRepository = userRepository;
        _gameLogRepository = gameLogRepository;
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

    public void ChangeUsername(string sessionId, string name)
    {
        name = name.Trim();
        ValidateUsername(name);
        _userRepository.ChangeName(sessionId, name);
    }

    public void AddGameLog(GameLog log, string sessionId)
    {
        _gameLogRepository.AddToUser(log, sessionId);
    }

    private void ValidateUsername(string name)
    {
        if (name.Length > MaxUsernameLength)
        {
            throw new UsernameTooLongException(MaxUsernameLength);
        }
        var regex = new Regex(UsernameRegexPattern);
        if (!regex.IsMatch(name))
        {
            throw new UsernameInvalidException();
        }
    }
}