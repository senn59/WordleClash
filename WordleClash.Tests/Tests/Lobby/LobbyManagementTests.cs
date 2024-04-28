using WordleClash.Core;
using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;

namespace WordleClash.Tests.Tests.Lobby;

//should not have to be tested for addition lobbies as only functions found in BaseLobby are tested.
public class LobbyManagementTests
{
    [Test]
    public void CreateLobby()
    {
        var lobby = TestHelpers.CreateVersusLobby(new MockDataAccess("", ""));
        Assert.Multiple(() =>
        {
            Assert.That(lobby.Players, Has.Count.EqualTo(1));
            Assert.That(lobby.Players[0].IsOwner, Is.EqualTo(true));
            //NOT IMPLEMENTED
            // Assert.That(lobby.Status, Is.EqualTo(LobbyState.InLobby));
        });
    }
    
    [Test]
    public void AddAdditionalPlayerToLobby()
    {
        var lobby = TestHelpers.CreateVersusLobby(new MockDataAccess("", ""));
        lobby.Add("");
        Assert.That(lobby.Players, Has.Count.EqualTo(2));
    }
    
    [Test]
    public void AddTooManyPlayers()
    {
        var lobby = TestHelpers.CreateVersusLobby(new MockDataAccess("", ""));
        lobby.Add("");
        Assert.Throws<LobbyFullException>(() => lobby.Add(""));
    }
    
    [Test]
    public void RemoveOriginalCreatorFromLobby()
    {
        var lobby = TestHelpers.CreateVersusLobby(new MockDataAccess("", ""));
        lobby.Add("player2");
        lobby.RemovePlayerById(lobby.Players[0].Id);
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
        var lobby = TestHelpers.CreateVersusLobby(new MockDataAccess("", ""));
        Assert.That(lobby.Code, Is.Not.EqualTo(string.Empty));
    }
}