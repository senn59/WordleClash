namespace WordleClash.Core.Interfaces;

public interface IMultiplayerGame
{
    int MaxPlayers { get; }
    int RequiredPlayers { get; }
    IReadOnlyList<Player> Players { get; set; }

    void StartGame();
    void HandleGuess(Player player, string guess);
    void UpdatePlayers(IReadOnlyList<Player> players);
}