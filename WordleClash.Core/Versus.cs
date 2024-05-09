using WordleClash.Core.Interfaces;
using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;
using WordleClash.Web.Models;

namespace WordleClash.Core;

public class Versus: IMultiplayerGame
{
    private const int MaxTries = 9;
    private IDataAccess _dataAccess;
    private Game _game;

    public IReadOnlyList<Player> Players { get; private set; } = new List<Player>();
    
    public int MaxPlayers { get; private init; } = 2;
    public int RequiredPlayers { get; private init; } = 2;

    public Versus(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
       _game = new Game(dataAccess, MaxTries);
    }
    
    public void StartGame()
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
        SetNextTurn(player);
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

    private void SetNextTurn(Player player)
    {
        var playerIndex = Players.ToList().IndexOf(player);
        int nextPlayerIndex;
        if (playerIndex == Players.Count - 1)
        {
            nextPlayerIndex = 0;
        }
        else
        {
            nextPlayerIndex = playerIndex + 1;
        }
        ResetTurnState();
        Players[nextPlayerIndex].IsTurn = true;
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

    public void RestartGame()
    {
        if (_game.Status is not (GameStatus.Lost or GameStatus.Won)) return;
        _game = new Game(_dataAccess, MaxTries);
        StartGame();
    }

    public List<GameView> GetGames()
    {
        return [GameView.FromGame(_game)];
    }
}