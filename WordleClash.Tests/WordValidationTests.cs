using WordleClash.Core;
using WordleClash.Core.Exceptions;

namespace WordleClash.Tests;

public class WordValidationTests
{
    
    [Test]
    public void GuessEmptyWord()
    {
        var wordle = new Game(3, new MockDataAccess("vwxyz", "vwxyz"));
        Assert.Throws<IncorrectLengthException>(() => wordle.MakeMove(""));
    }
    
    [Test]
    public void GuessTooShortWord()
    {
        var wordle = new Game(3, new MockDataAccess("vwxyz", "vwxyz"));
        Assert.Throws<IncorrectLengthException>(() => wordle.MakeMove("vwxy"));
    }
    
    [Test]
    public void GuessInvalidWord()
    {
        var wordle = new Game(3, new MockDataAccess("vwxyz", "vwxyz"));
        Assert.Throws<InvalidWordException>(() => wordle.MakeMove("abcde"));
    }
    
    [Test]
    public void GuessValidWord()
    {
        var wordle = new Game(3, new MockDataAccess("vwxyz", "vwxyz"));
        Assert.DoesNotThrow(() => wordle.MakeMove("vwxyz"));
    }
    
}