using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class GameView
{
    public int Tries { get; init; }
    public int MaxTries { get; init; }
    public GameStatus Status { get; init; }
    public IReadOnlyList<GuessResult> GuessHistory { get; init; } = [];
    public static GameView FromGame(Game game)
    {
        return new GameView()
        {
            Tries = game.Tries,
            MaxTries = game.MaxTries,
            Status = game.Status,
            GuessHistory = game.GuessHistory
        };
    }
}