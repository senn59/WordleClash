using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class GameModel
{
    public int Tries { get; init; }
    public int MaxTries { get; init; }
    public GameStatus Status { get; init; }
    public IReadOnlyList<GuessResult> GuessHistory { get; init; } = [];
    public static GameModel FromGame(Game game)
    {
        return new GameModel()
        {
            Tries = game.Tries,
            MaxTries = game.MaxTries,
            Status = game.Status,
            GuessHistory = game.GuessHistory
        };
    }
}