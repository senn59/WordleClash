using WordleClash.Core;
using WordleClash.Core.DataAccess;
using WordleClash.Core.Enums;

namespace WordleClash.Tests;

public class GameStatusTests
{
    [Test]
    public void NoCommonLetters()
    {
        var wordle = new Wordle(6, new MockDataAccess("zzzzz"));
        var res = wordle.MakeMove("xxxxx");

        Assert.That(res.Status, Is.EqualTo(GameStatus.InProgress));
    }
    
    [Test]
    public void OneLetterOff()
    {
        var wordle = new Wordle(6, new MockDataAccess("abcde"));
        var res = wordle.MakeMove("abcdf");

        Assert.That(res.Status, Is.EqualTo(GameStatus.InProgress));
    }
    
    [Test]
    public void CorrectInput()
    {
        var wordle = new Wordle(6, new MockDataAccess("abcde"));
        var res = wordle.MakeMove("abcde");

        Assert.That(res.Status, Is.EqualTo(GameStatus.Won));
    }
    
    [Test]
    public void TooManyTries()
    {
        var wordle = new Wordle(3, new MockDataAccess("vwxyz"));
        wordle.MakeMove("abcde");
        wordle.MakeMove("abcde");
        var res = wordle.MakeMove("abcde");
        Assert.That(res.Status, Is.EqualTo(GameStatus.Lost));
        res = wordle.MakeMove("vwxyz");
        Assert.That(res.Status, Is.EqualTo(GameStatus.Lost));
    }
}
