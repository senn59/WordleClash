namespace WordleClash.Core;

public class GameLog
{
    public required int AttemptCount { get; init; }
    public required TimeSpan? Time { get; init; }
    public required bool Status { get; init; }
    public required string Word { get; init; }
}