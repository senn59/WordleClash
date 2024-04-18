using WordleClash.Core;
using WordleClash.Core.Exceptions;

namespace WordleClash.Tests.Tests;

public class WordValidationTests
{
    
    [Test]
    public void GuessEmptyWord()
    {
        var wordle = new Game(new MockDataAccess("vwxyz", "vwxyz"));
        wordle.Start(3);
        Assert.Throws<IncorrectLengthException>(() => wordle.TakeGuess(""));
    }
    
    [Test]
    public void GuessTooShortWord()
    {
        var wordle = new Game(new MockDataAccess("vwxyz", "vwxyz"));
        wordle.Start(3);
        Assert.Throws<IncorrectLengthException>(() => wordle.TakeGuess("vwxy"));
    }
    
    [Test]
    public void GuessInvalidWord()
    {
        var wordle = new Game(new MockDataAccess("vwxyz", "vwxyz"));
        wordle.Start(3);
        Assert.Throws<InvalidWordException>(() => wordle.TakeGuess("abcde"));
    }
    
    [Test]
    public void GuessValidWord()
    {
        var wordle = new Game(new MockDataAccess("vwxyz", "vwxyz"));
        wordle.Start(3);
        Assert.DoesNotThrow(() => wordle.TakeGuess("vwxyz"));
    }
    
}