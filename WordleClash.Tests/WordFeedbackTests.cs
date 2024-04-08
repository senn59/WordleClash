using WordleClash.Core;
using WordleClash.Core.DataAccess;
using WordleClash.Core.Enums;

namespace WordleClash.Tests;

public class WordFeedBackTests
{
    [Test]
    public void OneCorrectLetter()
    {
        var dataAccess = new MockDataAccess("table", "chime");
        var wordle = new Game(6, dataAccess);
        var res = wordle.MakeMove(dataAccess.Guess);

        LetterFeedback[] expectedResult =
        [
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.CorrectPosition
        ];
        Assert.That(res.Feedback, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public void CorrectLetterMultipleOccurences()
    {
        var dataAccess = new MockDataAccess("zzllz", "xxxlx");
        var wordle = new Game(6, dataAccess);
        var res = wordle.MakeMove(dataAccess.Guess);

        LetterFeedback[] expectedResult =
        [
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.CorrectPosition,
            LetterFeedback.IncorrectLetter,
        ];
        Assert.That(res.Feedback, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public void IncorrectPosition()
    {
        var dataAccess = new MockDataAccess("zzzlz", "lxxxx");
        var wordle = new Game(6, dataAccess);
        var res = wordle.MakeMove(dataAccess.Guess);

        LetterFeedback[] expectedResult =
        [
            LetterFeedback.IncorrectPosition,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
        ];
        Assert.That(res.Feedback, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public void AllDifferentFeedbackTypesAtOnce()
    {
        var dataAccess = new MockDataAccess("abczz", "xxcba");
        var wordle = new Game(6, dataAccess);
        var res = wordle.MakeMove(dataAccess.Guess);

        LetterFeedback[] expectedResult =
        [
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.CorrectPosition,
            LetterFeedback.IncorrectPosition,
            LetterFeedback.IncorrectPosition,
        ];
        Assert.That(res.Feedback, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public void SameLetterCorrectAndIncorrectPosition()
    {
        var dataAccess = new MockDataAccess("zzllz", "xxxll");
        var wordle = new Game(6, dataAccess);
        var res = wordle.MakeMove(dataAccess.Guess);

        LetterFeedback[] expectedResult =
        [
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.CorrectPosition,
            LetterFeedback.IncorrectPosition,
        ];
        Assert.That(res.Feedback, Is.EqualTo(expectedResult));
    }
}
