using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;

namespace WordleClash.Core.Entities;

public class GameLog
{
    public required int AttemptCount { get; init; }
    public required TimeSpan? Time { get; init; }
    public required GameStatus Status { get; init; }
    public DateTime Date { get; init; } = DateTime.Now.AddHours(-15);
    public required string Word { get; init; }

    public static GameLog FromGame(Game game)
    {
        if (game.Status is GameStatus.Won or GameStatus.Lost)
        {
            throw new GameInProgressException();
        }
        
        return new GameLog
        {
            AttemptCount = game.Tries,
            Time = null,
            Status = game.Status,
            Word = game.GetTargetWord() ?? string.Empty //only null if game is still in progress
        };
    }

    public static string GetTimeDifference(DateTime date)
    {
        const int second = 1;
        const int minute = 60 * second;
        const int hour = 60 * minute;
        const int day = 24 * hour;
        const int month = 30 * day;
        const string suffix = "ago";
        var difference = DateTime.Now.Subtract(date);
        return difference.TotalSeconds switch
        {
            < minute => $"{(int)difference.TotalSeconds}s {suffix}",
            < hour => $"{(int)difference.TotalMinutes}m {suffix}",
            < day => $"{(int)difference.TotalHours}h {suffix}",
            < month => $"{(int)difference.TotalDays}d {suffix}",
            _ => $"{(int)difference.TotalDays / 365}y ago"
        };
    }
}