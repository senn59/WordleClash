namespace WordleClash.Core;

public class GameLog
{
    public int Id { get; private init; }
    public int AttemptCount { get; private init; }
    public TimeSpan Time { get; private init; }
    public bool Status { get; private init; }
    public string Word { get; private init; }
}