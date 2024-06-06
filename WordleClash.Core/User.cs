namespace WordleClash.Core;

public class User
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string SessionId { get; init; }
    public DateTime CreatedAt { get; init; }
    public List<GameLog> GameHistory { get; private set; } = new List<GameLog>();
}