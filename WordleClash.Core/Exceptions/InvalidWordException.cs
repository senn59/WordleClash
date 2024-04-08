namespace WordleClash.Core.Exceptions;

public class InvalidWordException: Exception
{
    public InvalidWordException(string message) : base(message) {}
}