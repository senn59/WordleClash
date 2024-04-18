using WordleClash.Core.DataAccess;
using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class VersusLobby: BaseLobby
{
    private const int MaxTries = 9;
    private Game _game;
    
    public VersusLobby(IDataAccess dataAccess): base(dataAccess)
    {
        RequiredPlayers = MaxPlayers = 2;
    }
    
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

    private void SetFirstTurn()
    {
        var r = new Random();
        var playerIndex = r.Next(0, Players.Count);
        ResetTurns();
        PlayerList[playerIndex].IsTurn = true;
    }

    private void ResetTurns()
    {
        PlayerList.ForEach(p => p.IsTurn = false);
    }
}