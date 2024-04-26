using WordleClash.Core.Interfaces;
using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;

namespace WordleClash.Core;

public class Versus: IMultiplayerGame
{
    private const int MaxTries = 9;
    private readonly IDataAccess _dataAccess;
    private Game _game;
    public Lobby Lobby { get; set; }

    public IReadOnlyList<Player> Players { get; set; }
    
    public int MaxPlayers { get; private init; } = 2;
    public int RequiredPlayers { get; private init; } = 2;

    public Versus(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }
    
    public void StartGame()
    {
        // Players = players;
       // if (Lobby.Status != LobbyState.InLobby && Lobby.Status != LobbyState.PostGame)
       // {
       //     throw new GameAlreadyStartedException();
       // }
       // if (Lobby.Players.Count != RequiredPlayers)
       // {
       //     throw new InvalidPlayerCountException();
       // }
       _game = new Game(_dataAccess);
       _game.Start(MaxTries);
       SetFirstTurn();
       // Lobby.Status = LobbyState.InGame;
    }

    public void HandleGuess(Player player, string guess)
    {
        //TODO: could also just return instead of throwing exceptions
        if (!Players.Contains(player))
        {
            throw new InvalidPlayerException();
        }
        
        if (player.IsTurn == false)
        {
            throw new NotPlayersTurnException(player);
        }

        if (Players.Count(p => p.IsTurn == true) != 1)
        {
            throw new Exception("More than one player is a turn holder");
        }
        
        var guessResult = _game.TakeGuess(guess);
        if (guessResult.Status == GameStatus.Won)
        {
            player.IsWinner = true;
            // Status = LobbyState.PostGame;
        }
        SetNextTurn(player);
    }

    public void UpdatePlayers(IReadOnlyList<Player> players)
    {
        throw new NotImplementedException();
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
}