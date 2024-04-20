using WordleClash.Core;
using WordleClash.Core.Enums;

namespace WordleClash.Web.Models;

public class GameViewModel
{
    public int Tries { get; set; }
    public int MaxTries { get; set; }
    public GameStatus Status { get; set; }
    public IReadOnlyList<GuessResult> MoveHistory { get; set; }
    public static GameViewModel FromGame(Game game)
    {
        return new GameViewModel()
        {
            Tries = game.Tries,
            MaxTries = game.MaxTries,
            Status = game.GameStatus,
            MoveHistory = game.MoveHistory
        };
    }
}