namespace WordleClash.Core.Exceptions;

public class IncorrectLengthException(int correctLength)
    : Exception($"Word is not the correct length of {correctLength}");