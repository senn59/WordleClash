namespace WordleClash.Core.Exceptions;

public class IncorrectLengthException: Exception
{
    public IncorrectLengthException(int correctLength) : base($"Word is not the correct length of {correctLength}") {}
}