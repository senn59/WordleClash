using WordleClash.Core.Interfaces;

namespace WordleClash.Core;

public class LobbyController
{
    private Lobby _lobby;
    private IMultiplayerGame _gameMode;
    
    public IReadOnlyList<Player> Players => _lobby.Players;
    public int MaxPlayers => _gameMode.MaxPlayers;
    public int RequiredPlayers => _gameMode.RequiredPlayers;
    public string Code => _lobby.Code;

    public LobbyController(IMultiplayerGame gameMode, Player creator)
    {
        _lobby = new Lobby(creator, gameMode.MaxPlayers);
        _gameMode = gameMode;
    }

    public void StartGame()
    {
        _gameMode.Players = Players;
        _gameMode.StartGame();
    }

    public void Add(Player player)
    {
        _lobby.Add(player);
    }
    
    public void Remove(Player player)
    {
        _lobby.RemoveById(player.Id);
    }
}