using WordleClash.Core.Exceptions;

namespace WordleClash.Tests.Tests.Lobby;

public class LobbyTests
{
    [Test]
    public void CreateLobby()
    {
        var lobby = new Core.Lobby("player1", 5);
        Assert.Multiple(() =>
        {
            Assert.That(lobby.Players, Has.Count.EqualTo(1));
            Assert.That(lobby.Players[0].IsOwner, Is.EqualTo(true));
        });
    }
    
    [Test]
    public void AddAdditionalPlayerToLobby()
    {
        var lobby = new Core.Lobby("player1", 5);
        lobby.Add("");
        Assert.That(lobby.Players, Has.Count.EqualTo(2));
        Assert.That(lobby.Players[1].IsOwner, Is.EqualTo(false));
    }
    
    [Test]
    public void AddTooManyPlayers()
    {
        var lobby = new Core.Lobby("player1", 2);
        lobby.Add("");
        Assert.Throws<LobbyFullException>(() => lobby.Add(""));
    }
    
    [Test]
    public void RemoveOriginalCreatorFromLobby()
    {
        var lobby = new Core.Lobby("player1", 2);
        lobby.Add("player2");
        lobby.RemoveById(lobby.Players[0].Id);
        Assert.Multiple(() =>
        {
            Assert.That(lobby.Players, Has.Count.EqualTo(1));
            Assert.That(lobby.Players[0].Name, Is.EqualTo("player2"));
            Assert.That(lobby.Players[0].IsOwner, Is.EqualTo(true));
        });
    }
    
    [Test]
    public void LobbyHasCode()
    {
        var lobby = new Core.Lobby("player1", 2);
        Assert.That(lobby.Code, Is.Not.EqualTo(string.Empty));
    }
}