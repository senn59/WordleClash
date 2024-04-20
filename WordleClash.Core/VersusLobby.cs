using WordleClash.Core.Interfaces;
using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;

namespace WordleClash.Core;

public class VersusLobby: BaseLobby
{
    private const int MaxTries = 9;
    private Game _game;
    
    public VersusLobby(IDataAccess dataAccess, Player creator): base(dataAccess, creator, 2, 2) {}
    
    public override void StartGame()
    {
       if (Status != LobbyStatus.InLobby && Status != LobbyStatus.PostGame)
       {
           throw new GameAlreadyStartedException();
       }
       if (Players.Count != RequiredPlayers)
       {
           throw new InvalidPlayerCountException();
       }
       //TODO: create overloading constructor that calls start
       _game = new Game(DataAccess);
       _game.Start(MaxTries);
       SetFirstTurn();
       Status = LobbyStatus.InGame;
    }

    public void HandleGuess(Player player, string guess)
    {
        //TODO: could also just return instead of throwing exceptions
        if (!PlayerList.Contains(player))
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
            Winner = player;
            Status = LobbyStatus.PostGame;
        }
        SetNextTurn(player);
    }

    private void SetNextTurn(Player player)
    {
        var playerIndex = PlayerList.IndexOf(player);
        int nextPlayerIndex;
        if (playerIndex == PlayerList.Count - 1)
        {
            nextPlayerIndex = 0;
        }
        else
        {
            nextPlayerIndex = playerIndex + 1;
        }
        ResetTurnState();
        PlayerList[nextPlayerIndex].IsTurn = true;
    }

    private void SetFirstTurn()
    {
        var r = new Random();
        var playerIndex = r.Next(0, Players.Count);
        ResetTurnState();
        PlayerList[playerIndex].IsTurn = true;
    }

    private void ResetTurnState()
    {
        PlayerList.ForEach(p => p.IsTurn = false);
    }
}