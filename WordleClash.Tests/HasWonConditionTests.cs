using WordleClash.Core;
using WordleClash.Core.DataAccess;

namespace WordleClash.Tests;

public class HasWonConditionTests
{
    [Test]
    public void NoCommonLetters()
    {
        var wordle = new Wordle(6, new MockDataAccess("zzzzz"));
        var res = wordle.MakeMove("xxxxx");

        Assert.That(res.HasWon, Is.EqualTo(false));
    }
    
    [Test]
    public void OneLetterOff()
    {
        var wordle = new Wordle(6, new MockDataAccess("abcde"));
        var res = wordle.MakeMove("abcdf");

        Assert.That(res.HasWon, Is.EqualTo(false));
    }
    
    [Test]
    public void CorrectInput()
    {
        var wordle = new Wordle(6, new MockDataAccess("abcde"));
        var res = wordle.MakeMove("abcde");

        Assert.That(res.HasWon, Is.EqualTo(true));
    }
}