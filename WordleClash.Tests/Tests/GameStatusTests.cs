using WordleClash.Core;
using WordleClash.Core.DataAccess;
using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;

namespace WordleClash.Tests;

public class GameStatusTests
{
    [Test]
    public void NoCommonLetters()
    {
        var dataAccess = new MockDataAccess("zzzzz", "xxxxx");
        var wordle = new Game(6, dataAccess);
        var res = wordle.TakeGuess(dataAccess.Guess);

        Assert.That(res.Status, Is.EqualTo(GameStatus.InProgress));
    }
    
    [Test]
    public void OneLetterOff()
    {
        var dataAccess = new MockDataAccess("abcde", "abcdf");
        var wordle = new Game(6, dataAccess);
        var res = wordle.TakeGuess(dataAccess.Guess);

        Assert.That(res.Status, Is.EqualTo(GameStatus.InProgress));
    }
    
    [Test]
    public void CorrectInput()
    {
        var dataAccess = new MockDataAccess("abcde", "abcde");
        var wordle = new Game(6, dataAccess);
        var res = wordle.TakeGuess(dataAccess.TargetWord);

        Assert.That(res.Status, Is.EqualTo(GameStatus.Won));
    }
    
    [Test]
    public void TooManyTries()
    {
        var dataAccess = new MockDataAccess("abcde", "vwxyz");
        var wordle = new Game(3, dataAccess);
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
        var wordle = new Game(3, dataAccess);
        wordle.TakeGuess(dataAccess.Guess);
        wordle.TakeGuess(dataAccess.Guess);
        var res = wordle.TakeGuess(dataAccess.TargetWord);
        Assert.That(res.Status, Is.EqualTo(GameStatus.Won));
    }
    
}
