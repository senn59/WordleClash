namespace WordleClash.Core;

public class User
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string SessionId { get; init; }
    public required DateTime CreatedAt { get; init; }
    public List<GameLog> GameHistory { get; private set; } = new List<GameLog>();
}