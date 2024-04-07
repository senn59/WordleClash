using WordleClash.Core;
using WordleClash.Core.DataAccess;
using WordleClash.Core.Enums;

namespace WordleClash.Tests;

public class Tests
{
    private Wordle _wordle;

    [Test]
    public void OneCorrectLetter()
    {
        _wordle = new Wordle(6, new MockDataAccess("table"));
        var res = _wordle.MakeMove("chime");

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