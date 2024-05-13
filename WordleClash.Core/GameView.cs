using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class GameView
{
    public int Tries { get; set; }
    public int MaxTries { get; set; }
    public GameStatus Status { get; set; }
    public IReadOnlyList<GuessResult> MoveHistory { get; set; }
    public static GameView FromGame(Game game)
    {
        return new GameView()
        {
            Tries = game.Tries,
            MaxTries = game.MaxTries,
            Status = game.Status,
            MoveHistory = game.MoveHistory
        };
    }
}