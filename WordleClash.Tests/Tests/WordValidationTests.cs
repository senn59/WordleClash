using WordleClash.Core;
using WordleClash.Core.Exceptions;

namespace WordleClash.Tests.Tests;

public class WordValidationTests
{
    
    [Test]
    public void GuessEmptyWord()
    {
        var wordle = new Game(3, new MockDataAccess("vwxyz", "vwxyz"));
        Assert.Throws<IncorrectLengthException>(() => wordle.TakeGuess(""));
    }
    
    [Test]
    public void GuessTooShortWord()
    {
        var wordle = new Game(3, new MockDataAccess("vwxyz", "vwxyz"));
        Assert.Throws<IncorrectLengthException>(() => wordle.TakeGuess("vwxy"));
    }
    
    [Test]
    public void GuessInvalidWord()
    {
        var wordle = new Game(3, new MockDataAccess("vwxyz", "vwxyz"));
        Assert.Throws<InvalidWordException>(() => wordle.TakeGuess("abcde"));
    }
    
    [Test]
    public void GuessValidWord()
    {
        var wordle = new Game(3, new MockDataAccess("vwxyz", "vwxyz"));
        Assert.DoesNotThrow(() => wordle.TakeGuess("vwxyz"));
    }
    
}