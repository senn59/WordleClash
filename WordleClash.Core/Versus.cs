using WordleClash.Core.Interfaces;
using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;

namespace WordleClash.Core;

public class Versus: IMultiplayerGame
{
    private const int MaxTries = 9;
    private IDataAccess _dataAccess;
    private Game _game;

    public IReadOnlyList<Player> Players { get; private set; } = new List<Player>();
    
    public int MaxPlayers => 2;
    public int RequiredPlayers => 2;

    public Versus(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
       _game = new Game(dataAccess, MaxTries);
    }
    
    public void Start()
    {
        ValidatePlayers();
        _game.Start();
        SetFirstTurn();
    }

    public GuessResult HandleGuess(Player player, string guess)
    {
        if (!Players.Contains(player))
        {
            throw new InvalidPlayerException();
        }
        
        if (player.IsTurn == false)
        {
            throw new NotPlayersTurnException(player);
        }
        
        var guessResult = _game.TakeGuess(guess);
        if (guessResult.Status == GameStatus.Won)
        {
            player.IsWinner = true;
        }
        SetNextTurn();
        return guessResult;
    }

    public void SetPlayers(IReadOnlyList<Player> players)
    {
        if (_game.Status == GameStatus.InProgress)
        {
            ValidatePlayers();
        }
        Players = players;
    }

    private void SetNextTurn()
    {
        //only works when we make the assumption that there are only 2 players
        foreach (var player in Players)
        {
            player.IsTurn = !player.IsTurn;

        }
    }

    private void SetFirstTurn()
    {
        var r = new Random();
        var playerIndex = r.Next(0, Players.Count);
        ResetTurnState();
        Players[playerIndex].IsTurn = true;
    }

    private void ResetTurnState()
    {
        foreach (var p in Players)
        {
            p.IsTurn = false;
        }
    }

    private void ValidatePlayers()
    {
        if (Players.Count < RequiredPlayers)
        {
            throw new TooFewPlayersException();
        }

        if (Players.Count > MaxPlayers)
        {
            throw new TooManyPlayersException();
        }
    }

    public void Restart()
    {
        if (_game.Status is not (GameStatus.Lost or GameStatus.Won)) return;
        _game = new Game(_dataAccess, MaxTries);
    }

    public List<GameView> GetGames()
    {
        return [GameView.FromGame(_game)];
    }
}