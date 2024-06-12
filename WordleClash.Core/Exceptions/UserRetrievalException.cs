namespace WordleClash.Core.Exceptions;

public class UserRetrievalException: Exception
{
    private static string _message = "Something went wrong while trying to retrieve the user";
    public UserRetrievalException()
        : base(_message)
    {
    }
    public UserRetrievalException(Exception inner)
        : base(_message, inner)
    {
    }
}