using WordleClash.Core;
using WordleClash.Core.Exceptions;

namespace WordleClash.Tests.Tests.CoreGameLogic;

public class WordValidationTests
{
    
    [Test]
    public void GuessEmptyWord()
    {
        var wordle = new Game(new MockWordRepository("vwxyz", "vwxyz"), 3);
        wordle.Start();
        Assert.Throws<IncorrectLengthException>(() => wordle.TakeGuess(""));
    }
    
    [Test]
    public void GuessTooShortWord()
    {
        var wordle = new Game(new MockWordRepository("vwxyz", "vwxyz"), 3);
        wordle.Start();
        Assert.Throws<IncorrectLengthException>(() => wordle.TakeGuess("vwxy"));
    }
    
    [Test]
    public void GuessInvalidWord()
    {
        var wordle = new Game(new MockWordRepository("vwxyz", "vwxyz"), 3);
        wordle.Start();
        Assert.Throws<InvalidWordException>(() => wordle.TakeGuess("abcde"));
    }
    
    [Test]
    public void GuessValidWord()
    {
        var wordle = new Game(new MockWordRepository("vwxyz", "vwxyz"), 3);
        wordle.Start();
        Assert.DoesNotThrow(() => wordle.TakeGuess("vwxyz"));
    }
    
}