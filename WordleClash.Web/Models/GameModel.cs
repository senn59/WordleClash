using WordleClash.Core;
using WordleClash.Core.Enums;

namespace WordleClash.Web.Models;

public class GameModel
{
    public int Tries { get; set; }
    public int MaxTries { get; set; }
    public GameStatus Status { get; set; }
    public IReadOnlyList<GuessResult> MoveHistory { get; set; }
    public static GameModel FromGame(Game game)
    {
        return new GameModel()
        {
            Tries = game.Tries,
            MaxTries = game.MaxTries,
            Status = game.GameStatus,
            MoveHistory = game.MoveHistory
        };
    }
}