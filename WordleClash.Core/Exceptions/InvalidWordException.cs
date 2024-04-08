namespace WordleClash.Core.Exceptions;

public class InvalidWordException: Exception
{
    public InvalidWordException(string input) : base($"{input} is not a valid word.") {}
}