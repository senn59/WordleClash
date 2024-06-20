using System.Text.RegularExpressions;
using WordleClash.Core.Entities;
using WordleClash.Core.Exceptions;
using WordleClash.Core.Interfaces;

namespace WordleClash.Core.Services;

public class UserService
{
    private IUserRepository _userRepository;
    private IGameLogRepository _gameLogRepository;
    private const int MaxUsernameLength = 30; //defined as VARCHAR(30) in database
    private const string UsernameRegexPattern = @"^[a-zA-Z0-9._^*()!$]+$";
    private const int PageSize = 10;
    private const int StartPage = 1;
    
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
            var user = _userRepository.GetByName(name);
            var gameLogs = _gameLogRepository.GetFromUserIdByPage(user.Id, PageSize, StartPage);
            user.GameHistory.AddRange(gameLogs);
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public List<GameLog> GetLogsByPage(int userId, int page)
    {
        return _gameLogRepository.GetFromUserIdByPage(userId, PageSize, page);
    }

    public User? GetFromSession(string sessionId)
    {
        try
        {
            var user = _userRepository.GetFromSessionId(sessionId);
            var gameLogs = _gameLogRepository.GetFromUserIdByPage(user.Id, PageSize, StartPage);
            user.GameHistory.AddRange(gameLogs);
            return user;
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

    public void Delete(int id)
    {
        _userRepository.DeleteById(id);
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