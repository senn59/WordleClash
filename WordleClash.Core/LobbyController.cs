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

    public LobbyController(IMultiplayerGame gameMode, Player creator)
    {
        _lobby = new Lobby(creator, gameMode.MaxPlayers);
        _gameMode = gameMode;
        State = LobbyState.InLobby;
    }

    public void StartGame()
    {
        if (State != LobbyState.InGame)
        {
            throw new GameAlreadyStartedException();
        }
        if (Players.Count != RequiredPlayers)
        {
            throw new InvalidPlayerCountException();
        }
        
        _gameMode.SetPlayers(Players);
        _gameMode.StartGame();
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
        var player = new Player() { Name = name };
        _lobby.Add(player);
        return player;
    }
    
    public void RemovePlayerById(string id)
    {
        _lobby.RemoveById(id);
        if (State == LobbyState.InGame)
        {
            _gameMode.SetPlayers(Players);
        }
    }
}