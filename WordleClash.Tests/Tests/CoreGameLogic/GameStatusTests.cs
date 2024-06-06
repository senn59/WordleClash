using WordleClash.Core;
using WordleClash.Core.Enums;

namespace WordleClash.Tests.Tests.CoreGameLogic;

public class GameStatusTests
{
    [Test]
    public void NoCommonLetters()
    {
        var dataAccess = new MockWordRepository("zzzzz", "xxxxx");
        var wordle = new Game(dataAccess, 6);
        wordle.Start();
        var res = wordle.TakeGuess(dataAccess.Guess);

        Assert.That(res.Status, Is.EqualTo(GameStatus.InProgress));
    }
    
    [Test]
    public void OneLetterOff()
    {
        var dataAccess = new MockWordRepository("abcde", "abcdf");
        var wordle = new Game(dataAccess, 6);
        wordle.Start();
        var res = wordle.TakeGuess(dataAccess.Guess);

        Assert.That(res.Status, Is.EqualTo(GameStatus.InProgress));
    }
    
    [Test]
    public void CorrectInput()
    {
        var dataAccess = new MockWordRepository("abcde", "abcde");
        var wordle = new Game(dataAccess, 6);
        wordle.Start();
        var res = wordle.TakeGuess(dataAccess.TargetWord);

        Assert.That(res.Status, Is.EqualTo(GameStatus.Won));
    }
    
    [Test]
    public void TooManyTries()
    {
        var dataAccess = new MockWordRepository("abcde", "vwxyz");
        var wordle = new Game(dataAccess, 3);
        wordle.Start();
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
        var dataAccess = new MockWordRepository("abcde", "vwxyz");
        var wordle = new Game(dataAccess, 6);
        wordle.Start();
        wordle.TakeGuess(dataAccess.Guess);
        wordle.TakeGuess(dataAccess.Guess);
        var res = wordle.TakeGuess(dataAccess.TargetWord);
        Assert.That(res.Status, Is.EqualTo(GameStatus.Won));
    }
    
}
