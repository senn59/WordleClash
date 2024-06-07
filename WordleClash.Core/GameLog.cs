namespace WordleClash.Core;

public class GameLog
{
    public int Id { get; init; }
    public int AttemptCount { get; init; }
    public TimeSpan Time { get; init; }
    public bool Status { get; init; }
    public string Word { get; init; }
}