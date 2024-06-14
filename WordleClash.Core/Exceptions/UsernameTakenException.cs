namespace WordleClash.Core.Exceptions;

public class UsernameTakenException(string username) : Exception
{
    public string Username { get; private set; } = username;
}
