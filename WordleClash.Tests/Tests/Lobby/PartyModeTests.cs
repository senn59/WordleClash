using WordleClash.Core;
using WordleClash.Core.Entities;
using WordleClash.Core.Exceptions;

namespace WordleClash.Tests.Tests.Lobby;

public class PartyModeTests
{
    
    [Test]
    public void StartGameInvalidPlayers()
    {
        var dataAccess = new MockWordRepository("abcde", "fghij");
        var game = new Party(dataAccess);
        Assert.Throws<TooFewPlayersException>(() => game.Start());
    }
    
    [Test]
    public void StartMultipleGames()
    {
        var players = new List<Player>()
        {
            new Player { Name = "player1" },
            new Player { Name = "player2" }
        };
        var dataAccess = new MockWordRepository("abcde", "fghij");
        var game = new Party(dataAccess);
        game.SetPlayers(players);
        game.Start();
        Assert.Throws<GameAlreadyStartedException>(() => game.Start());
    }
    
    [Test]
    public void StartWithTooManyPlayers()
    {
        var players = new List<Player>()
        {
            new Player { Name = "player1" },
            new Player { Name = "player2" },
            new Player { Name = "player3" },
            new Player { Name = "player4" },
            new Player { Name = "player5" }
        };
        var dataAccess = new MockWordRepository("abcde", "fghij");
        var game = new Party(dataAccess);
        game.SetPlayers(players);
        Assert.Throws<TooManyPlayersException>(() => game.Start());
    }
    
    [Test]
    public void StartGame()
    {
        var players = new List<Player>()
        {
            new Player { Name = "player1" },
            new Player { Name = "player2" }
        };
        var dataAccess = new MockWordRepository("abcde", "fghij");
        var game = new Party(dataAccess);
        game.SetPlayers(players);
        game.Start();
        Assert.Pass();
    }
    
    [Test]
    public void PlayCompleteGame()
    {
        var dataAccess = new MockWordRepository("abcde", "fghij");
        var game = new Party(dataAccess);
        var players = new List<Player>()
        {
            new Player { Name = "player1" },
            new Player { Name = "player2" }
        };
        
        game.SetPlayers(players);
        game.Start();

        game.HandleGuess(players[0], dataAccess.Guess);
        game.HandleGuess(players[1], dataAccess.Guess);
        game.HandleGuess(players[1], dataAccess.Guess);
        game.HandleGuess(players[0], dataAccess.Guess);
        game.HandleGuess(players[0], dataAccess.Guess);
        game.HandleGuess(players[0], dataAccess.Guess);
        game.HandleGuess(players[1], dataAccess.TargetWord);
        Assert.Multiple(() =>
        {
            Assert.That(players[1].IsWinner, Is.EqualTo(true));
        });
    }
    
    [Test]
    public void TakeGuessBeforeGameStart()
    {
        var dataAccess = new MockWordRepository("abcde", "fghij");
        var game = new Party(dataAccess);
        var players = new List<Player>()
        {
            new Player { Name = "player1" },
            new Player { Name = "player2" }
        };
        
        game.SetPlayers(players);
        Assert.Throws<GameNotStartedException>(() => game.HandleGuess(players[0], dataAccess.Guess));
    }
    
    [Test]
    public void PlayTwoGames()
    {
        var dataAccess = new MockWordRepository("abcde", "fghij");
        var game = new Party(dataAccess);
        var players = new List<Player>()
        {
            new Player { Name = "player1" },
            new Player { Name = "player2" }
        };
        
        game.SetPlayers(players);
        game.Start();

        game.HandleGuess(players[0], dataAccess.Guess);
        game.HandleGuess(players[1], dataAccess.Guess);
        game.HandleGuess(players[1], dataAccess.Guess);
        game.HandleGuess(players[0], dataAccess.Guess);
        game.HandleGuess(players[0], dataAccess.Guess);
        game.HandleGuess(players[0], dataAccess.Guess);
        game.HandleGuess(players[1], dataAccess.TargetWord);
        Assert.That(players[1].IsWinner, Is.EqualTo(true));
        game.Restart();
        game.Start();
        game.HandleGuess(players[0], dataAccess.TargetWord);
        Assert.That(players[0].IsWinner, Is.EqualTo(true));
    }
}