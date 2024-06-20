using WordleClash.Core.Entities;

namespace WordleClash.Core.Interfaces;

public interface IMultiplayerGame
{
    int MaxPlayers { get; }
    int RequiredPlayers { get; }
    IReadOnlyList<Player> Players { get; }

    void Start();
    GuessResult HandleGuess(Player player, string guess);
    void SetPlayers(IReadOnlyList<Player> players);
    public List<GameModel> GetGames();
    void Restart();
}