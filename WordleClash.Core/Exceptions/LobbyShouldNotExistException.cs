namespace WordleClash.Core.Exceptions;

public class LobbyShouldNotExistException: Exception
{
    public LobbyShouldNotExistException() : base("Lobby instance should be destroyed") {}
}