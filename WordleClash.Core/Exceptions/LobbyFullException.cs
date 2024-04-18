namespace WordleClash.Core.Exceptions;

public class LobbyFullException: Exception
{
    public LobbyFullException(int maxPlayers) : base($"Lobby already full, max of {maxPlayers} allowed.") {}
}