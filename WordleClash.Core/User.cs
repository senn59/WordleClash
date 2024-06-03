namespace WordleClash.Core;

public class User
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string SessionId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public List<GameLog> GameHistory { get; private set; } = new List<GameLog>();
}