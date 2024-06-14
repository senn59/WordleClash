namespace WordleClash.Core.Exceptions;

public class UsernameTooLongException(int maxLength): Exception
{
    public int MaxLength { get; set; } = maxLength;
}