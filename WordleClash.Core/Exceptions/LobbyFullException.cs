namespace WordleClash.Core.Exceptions;

public class LobbyFullException(int maxPlayers) : Exception($"Lobby already full, max of {maxPlayers} allowed.");