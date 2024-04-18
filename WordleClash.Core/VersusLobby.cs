using WordleClash.Core.Interfaces;
using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class VersusLobby: BaseLobby
{
    private const int MaxTries = 9;
    private Game _game;
    
    public VersusLobby(IDataAccess dataAccess, Player creator): base(dataAccess, creator, 2, 2) {}
    
    public override void StartGame()
    {
        if (Status != LobbyStatus.InLobby)
        {
            throw new Exception("Game already started");
        }
        if (Players.Count != RequiredPlayers)
        {
            throw new Exception($"Not enough players to start the game");
        }
        //TODO: create overloading constructor that calls start
        _game = new Game(DataAccess);
        _game.Start(MaxTries);
        SetFirstTurn();
        Status = LobbyStatus.InGame;
    }

    public void HandleGuess(Player player, string guess)
    {
        if (!PlayerList.Contains(player))
        {
            throw new Exception("Player not in lobby");
        }
        
        if (player.IsTurn == false)
        {
            throw new Exception("It's not the player's turn");
        }
        
        //TODO: check if other players also have IsTurn state
        _game.TakeGuess(guess);
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