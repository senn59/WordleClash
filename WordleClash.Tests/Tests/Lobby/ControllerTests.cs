using WordleClash.Core;
using WordleClash.Core.Enums;

namespace WordleClash.Tests.Tests.Lobby;

public class ControllerTests
{
    [Test]
    public void InLobbyState()
    {
        var dataAccess = new MockWordDao("abcde", "fghij");
        var gameMode = new Versus(dataAccess);
        var lobby = new LobbyController(gameMode, "player1");
        lobby.Add("player2");
        Assert.That(lobby.State, Is.EqualTo(LobbyState.InLobby));
    }
    
    [Test]
    public void IngameState()
    {
        var dataAccess = new MockWordDao("abcde", "fghij");
        var gameMode = new Versus(dataAccess);
        var lobby = new LobbyController(gameMode, "player1");
        lobby.Add("player2");
        lobby.StartGame();
        Assert.That(lobby.State, Is.EqualTo(LobbyState.InGame));
        var turnHolderTurnOne = lobby.Players.First(p => p.IsTurn == true);
        lobby.HandleGuess(turnHolderTurnOne, dataAccess.Guess);
        var turnHolderTurnTwo = lobby.Players.First(p => p.IsTurn == true);
        lobby.HandleGuess(turnHolderTurnTwo, dataAccess.Guess);
        Assert.That(lobby.State, Is.EqualTo(LobbyState.InGame));
    }
    
    [Test]
    public void PostGameState()
    {
        var dataAccess = new MockWordDao("abcde", "fghij");
        var gameMode = new Versus(dataAccess);
        var lobby = new LobbyController(gameMode, "player1");
        lobby.Add("player2");
        lobby.StartGame();
        var turnHolderTurnOne = lobby.Players.First(p => p.IsTurn == true);
        lobby.HandleGuess(turnHolderTurnOne, dataAccess.TargetWord);
        Assert.That(lobby.State, Is.EqualTo(LobbyState.PostGame));
    }
    
    [Test]
    public void NewGame()
    {
        var dataAccess = new MockWordDao("abcde", "fghij");
        var gameMode = new Versus(dataAccess);
        var lobby = new LobbyController(gameMode, "player1");
        lobby.Add("player2");
        lobby.StartGame();
        var turnHolderTurnOne = lobby.Players.First(p => p.IsTurn == true);
        lobby.HandleGuess(turnHolderTurnOne, dataAccess.TargetWord);
        Assert.That(lobby.State, Is.EqualTo(LobbyState.PostGame));
        lobby.Restart();
        Assert.That(lobby.State, Is.EqualTo(LobbyState.InLobby));
    }
}