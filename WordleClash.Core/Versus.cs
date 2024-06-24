using WordleClash.Core.Entities;
using WordleClash.Core.Interfaces;
using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;

namespace WordleClash.Core;

public class Versus: IMultiplayerGame
{
    private const int MaxTries = 9;
    private readonly IWordRepository _wordRepository;
    private Game _game;

    public IReadOnlyList<Player> Players { get; private set; } = new List<Player>();
    
    public int MaxPlayers => 2;
    public int RequiredPlayers => 2;

    public Versus(IWordRepository wordRepository)
    {
        _wordRepository = wordRepository;
       _game = new Game(wordRepository, MaxTries);
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
            player.SetWinner();
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
        Players[nextPlayerIndex].SetTurn(true);
    }

    private void SetFirstTurn()
    {
        var r = new Random();
        var playerIndex = r.Next(0, Players.Count);
        ResetTurnState();
        Players[playerIndex].SetTurn(true);
    }

    private void ResetTurnState()
    {
        foreach (var player in Players)
        {
            player.SetTurn(false);
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
        _game = new Game(_wordRepository, MaxTries);
        foreach (var player in Players)
        {
            player.SetWinner(false);
        }
    }

    public List<GameModel> GetGames()
    {
        return [GameModel.FromGame(_game)];
    }
}