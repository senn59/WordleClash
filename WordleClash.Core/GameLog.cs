using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;

namespace WordleClash.Core;

public class GameLog
{
    public required int AttemptCount { get; init; }
    public required TimeSpan? Time { get; init; }
    public required int Status { get; init; }
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
            Status = (int)game.Status,
            Word = game.GetTargetWord() ?? string.Empty //only null if game is still in progress
        };
    }
}