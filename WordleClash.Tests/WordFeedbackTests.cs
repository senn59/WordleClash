using WordleClash.Core;
using WordleClash.Core.DataAccess;
using WordleClash.Core.Enums;

namespace WordleClash.Tests;

public class WordFeedBackTests
{
    [Test]
    public void OneCorrectLetter()
    {
        var wordle = new Wordle(6, new MockDataAccess("table"));
        var res = wordle.MakeMove("chime");

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
        var wordle = new Wordle(6, new MockDataAccess("zzllz"));
        var res = wordle.MakeMove("xxxlx");

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
        var wordle = new Wordle(6, new MockDataAccess("zzzlz"));
        var res = wordle.MakeMove("lxxxx");

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
        var wordle = new Wordle(6, new MockDataAccess("abczz"));
        var res = wordle.MakeMove("xxcba");

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
}

public class MockDataAccess : IDataAccess
{
    private string _word;
    public MockDataAccess(string word)
    {
        _word = word;
    }
    public List<string> GetWords()
    {
        return [_word];
    }
}