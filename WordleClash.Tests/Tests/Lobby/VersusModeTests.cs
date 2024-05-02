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
        var turnHolderT1 = game.Players.Where(p => p.IsTurn == true).ToList()[0];
        Console.WriteLine($"{turnHolderT1.Name}'s turn");
        game.HandleGuess(turnHolderT1, dataAccess.Guess);
        var turnHolderT2 = game.Players.Where(p => p.IsTurn == true).ToList()[0];
        Console.WriteLine($"{turnHolderT2.Name}'s turn");
        Assert.That(turnHolderT2, Is.Not.EqualTo(turnHolderT1));
        game.HandleGuess(turnHolderT2, dataAccess.TargetWord);
    }
}