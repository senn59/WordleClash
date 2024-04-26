using WordleClash.Core.Interfaces;

namespace WordleClash.Core;

public class LobbyController
{
    private Lobby _lobby;
    private IMultiplayerGame _multiplayerGame;
    
    public IReadOnlyList<Player> Players => _lobby.Players;
    public int MaxPlayers => _multiplayerGame.MaxPlayers;
    public int RequiredPlayers => _multiplayerGame.RequiredPlayers;
    public string Code => _lobby.Code;

    public LobbyController(IMultiplayerGame multiplayerGame, Player creator)
    {
        _lobby = new Lobby(creator, multiplayerGame.MaxPlayers);
        _multiplayerGame = multiplayerGame;
    }

    public void StartGame()
    {
        _multiplayerGame.Players = Players;
        _multiplayerGame.StartGame();
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