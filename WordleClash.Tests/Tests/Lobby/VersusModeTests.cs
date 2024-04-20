using WordleClash.Core;
using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;

namespace WordleClash.Tests.Tests.Lobby;

public class VersusModeTests
{
    [Test]
    public void StartGame()
    {
        var p1 = new Player() { Name = "player1" };
        var p2 = new Player() { Name = "player2" };
        var dataAccess = new MockDataAccess("abcde", "fghij");
        var lobby = new VersusLobby(dataAccess, p1);
        lobby.Add(p2);
        lobby.StartGame();
        Assert.That(lobby.Status, Is.EqualTo(LobbyStatus.InGame));
    }

    [Test]
    public void StartMultipleGames()
    {
        
        var p1 = new Player() { Name = "player1" };
        var p2 = new Player() { Name = "player2" };
        var dataAccess = new MockDataAccess("abcde", "fghij");
        var lobby = new VersusLobby(dataAccess, p1);
        lobby.Add(p2);
        lobby.StartGame();
        Assert.Throws<GameAlreadyStartedException>(() => lobby.StartGame());
    }
    
    [Test]
    public void TakingTurns()
    {
        
        var p1 = new Player() { Name = "player1" };
        var p2 = new Player() { Name = "player2" };
        var dataAccess = new MockDataAccess("abcde", "fghij");
        var lobby = new VersusLobby(dataAccess, p1);
        lobby.Add(p2);
        lobby.StartGame();
        var turnHolderT1 = lobby.Players.Where(p => p.IsTurn == true).ToList()[0];
        Console.WriteLine($"{turnHolderT1.Name}'s turn");
        lobby.HandleGuess(turnHolderT1, dataAccess.Guess);
        var turnHolderT2 = lobby.Players.Where(p => p.IsTurn == true).ToList()[0];
        Console.WriteLine($"{turnHolderT2.Name}'s turn");
        Assert.That(turnHolderT2, Is.Not.EqualTo(turnHolderT1));
        lobby.HandleGuess(turnHolderT2, dataAccess.TargetWord);
    }
}