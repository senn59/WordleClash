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

    public Player Add(string name)
    {
        var player = new Player() { Name = name };
        _lobby.Add(player);
        return player;
    }
    
    public void RemovePlayerById(string id)
    {
        _lobby.RemoveById(id);
    }
}