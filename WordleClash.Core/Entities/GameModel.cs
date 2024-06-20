using WordleClash.Core.Enums;

namespace WordleClash.Core.Entities;

public class GameModel
{
    public required int Tries { get; init; }
    public required int MaxTries { get; init; }
    public required GameStatus Status { get; init; }
    public required IReadOnlyList<GuessResult> GuessHistory { get; init; } = [];
    public required string? TargetWord { get; init; }
    public static GameModel FromGame(Game game)
    {
        return new GameModel()
        {
            Tries = game.Tries,
            MaxTries = game.MaxTries,
            Status = game.Status,
            GuessHistory = game.GuessHistory,
            TargetWord = game.GetTargetWord()
        };
    }
}