using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;
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
    public LobbyState State { get; private set; }
    public List<GameView> Games => _gameMode.GetGames();
    public Player? Winner => _gameMode.Players.FirstOrDefault(p => p.IsWinner == true);

    public LobbyController(IMultiplayerGame gameMode, string creator)
    {
        _lobby = new Lobby(creator, gameMode.MaxPlayers);
        _gameMode = gameMode;
        State = LobbyState.InLobby;
    }

    public void StartGame()
    {
        _gameMode.SetPlayers(Players);
        _gameMode.Start();
        State = LobbyState.InGame;
    }

    public GuessResult HandleGuess(Player player, string guess)
    {
        var result = _gameMode.HandleGuess(player, guess);
        if (result.Status is GameStatus.Won or GameStatus.Lost)
        {
            State = LobbyState.PostGame;
        }
        return result;
    }

    public Player Add(string name)
    {
        return _lobby.Add(name);
    }
    
    public void RemovePlayerById(string id)
    {
        _lobby.RemoveById(id);
        if (State != LobbyState.InGame) return;
        try
        {
            _gameMode.SetPlayers(Players);
        }
        catch (Exception e) when (e is TooFewPlayersException or TooManyPlayersException)
        {
            _gameMode.Restart();
            State = LobbyState.PostGame;
        }
    }

    public void Restart()
    {
        _gameMode.Restart();
        State = LobbyState.InLobby;
    }
}