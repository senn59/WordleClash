using WordleClash.Core;
using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;

namespace WordleClash.Tests.Tests.Lobby;

public class VersusModeTests
{
    [Test]
    public void StartGameInvalidPlayers()
    {
        var dataAccess = new MockDataAccess("abcde", "fghij");
        var game = new Versus(dataAccess);
        Assert.Throws<TooFewPlayersException>(() => game.StartGame());
    }

    [Test]
    public void StartMultipleGames()
    {
        var players = new List<Player>()
        {
            new Player { Name = "player1" },
            new Player { Name = "player2" }
        };
        var dataAccess = new MockDataAccess("abcde", "fghij");
        var game = new Versus(dataAccess);
        game.SetPlayers(players);
        game.StartGame();
        Assert.Throws<GameAlreadyStartedException>(() => game.StartGame());
    }
    
    [Test]
    public void StartWithTooManyPlayers()
    {
        var players = new List<Player>()
        {
            new Player { Name = "player1" },
            new Player { Name = "player2" },
            new Player { Name = "player3" }
        };
        var dataAccess = new MockDataAccess("abcde", "fghij");
        var game = new Versus(dataAccess);
        game.SetPlayers(players);
        Assert.Throws<TooManyPlayersException>(() => game.StartGame());
    }
    
    [Test]
    public void StartGame()
    {
        var players = new List<Player>()
        {
            new Player { Name = "player1" },
            new Player { Name = "player2" }
        };
        var dataAccess = new MockDataAccess("abcde", "fghij");
        var game = new Versus(dataAccess);
        game.SetPlayers(players);
        game.StartGame();
        Assert.Pass();
    }
    
    [Test]
    public void TakingTurns()
    {
        var dataAccess = new MockDataAccess("abcde", "fghij");
        var game = new Versus(dataAccess);
        var players = new List<Player>()
        {
            new Player { Name = "player1" },
            new Player { Name = "player2" }
        };
        
        game.SetPlayers(players);
        game.StartGame();

        var turnHolderTurnOne = TestHelpers.GetTurnHolder(game);
        game.HandleGuess(turnHolderTurnOne, dataAccess.Guess);
        
        var turnHolderTurnTwo = TestHelpers.GetTurnHolder(game);
        game.HandleGuess(turnHolderTurnTwo, dataAccess.Guess);
        
        var turnHolderTurnThree = TestHelpers.GetTurnHolder(game);
        game.HandleGuess(turnHolderTurnThree, dataAccess.TargetWord);
        Assert.Multiple(() =>
        {
            Assert.That(turnHolderTurnTwo.Id, Is.Not.EqualTo(turnHolderTurnOne.Id));
            Assert.That(turnHolderTurnThree.Id, Is.EqualTo(turnHolderTurnOne.Id));
        });
    }
    
    [Test]
    public void TakeMultipleTurnsAtOnce()
    {
        var dataAccess = new MockDataAccess("abcde", "fghij");
        var game = new Versus(dataAccess);
        var players = new List<Player>()
        {
            new Player { Name = "player1" },
            new Player { Name = "player2" }
        };
        
        game.SetPlayers(players);
        game.StartGame();

        var turnHolder = TestHelpers.GetTurnHolder(game);
        game.HandleGuess(turnHolder, dataAccess.Guess);
        Assert.Throws<NotPlayersTurnException>( () => game.HandleGuess(turnHolder, dataAccess.Guess));
    }
    
    [Test]
    public void TakeTurnWhileNotInGame()
    {
        var dataAccess = new MockDataAccess("abcde", "fghij");
        var game = new Versus(dataAccess);
        var players = new List<Player>()
        {
            new Player { Name = "player1" },
            new Player { Name = "player2" }
        };
        var invalidPlayer = new Player { Name = "Player3" };
        
        game.SetPlayers(players);
        game.StartGame();

        Assert.Throws<InvalidPlayerException>( () => game.HandleGuess(invalidPlayer, dataAccess.Guess));
    }
}