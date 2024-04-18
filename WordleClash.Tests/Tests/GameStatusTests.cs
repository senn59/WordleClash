using WordleClash.Core;
using WordleClash.Core.Enums;

namespace WordleClash.Tests.Tests;

public class GameStatusTests
{
    [Test]
    public void NoCommonLetters()
    {
        var dataAccess = new MockDataAccess("zzzzz", "xxxxx");
        var wordle = new Game(dataAccess);
        wordle.Start(6);
        var res = wordle.TakeGuess(dataAccess.Guess);

        Assert.That(res.Status, Is.EqualTo(GameStatus.InProgress));
    }
    
    [Test]
    public void OneLetterOff()
    {
        var dataAccess = new MockDataAccess("abcde", "abcdf");
        var wordle = new Game(dataAccess);
        wordle.Start(6);
        var res = wordle.TakeGuess(dataAccess.Guess);

        Assert.That(res.Status, Is.EqualTo(GameStatus.InProgress));
    }
    
    [Test]
    public void CorrectInput()
    {
        var dataAccess = new MockDataAccess("abcde", "abcde");
        var wordle = new Game(dataAccess);
        wordle.Start(6);
        var res = wordle.TakeGuess(dataAccess.TargetWord);

        Assert.That(res.Status, Is.EqualTo(GameStatus.Won));
    }
    
    [Test]
    public void TooManyTries()
    {
        var dataAccess = new MockDataAccess("abcde", "vwxyz");
        var wordle = new Game(dataAccess);
        wordle.Start(3);
        wordle.TakeGuess(dataAccess.Guess);
        wordle.TakeGuess(dataAccess.Guess);
        var res = wordle.TakeGuess(dataAccess.Guess);
        Assert.That(res.Status, Is.EqualTo(GameStatus.Lost));
        res = wordle.TakeGuess(dataAccess.TargetWord);
        Assert.That(res.Status, Is.EqualTo(GameStatus.Lost));
    }
    
    [Test]
    public void GuessCorrectOnLastTry()
    {
        var dataAccess = new MockDataAccess("abcde", "vwxyz");
        var wordle = new Game(dataAccess);
        wordle.Start(6);
        wordle.TakeGuess(dataAccess.Guess);
        wordle.TakeGuess(dataAccess.Guess);
        var res = wordle.TakeGuess(dataAccess.TargetWord);
        Assert.That(res.Status, Is.EqualTo(GameStatus.Won));
    }
    
}
