namespace WordleClash.Core.Exceptions;

public class NotPlayersTurnException(Player p) : Exception($"It's not {p.Name}'s turn");
    