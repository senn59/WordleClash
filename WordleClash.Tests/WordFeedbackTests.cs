using WordleClash.Core;
using WordleClash.Core.Enums;

namespace WordleClash.Tests;

public class WordFeedBackTests
{
    [Test]
    public void OneCorrectLetter()
    {
        var feedback = ExtractFeedbackFromGuess("table", "chime");
        LetterFeedback[] expectedResult =
        [
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.CorrectPosition
        ];
        Assert.That(feedback, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public void CorrectLetterMultipleOccurences()
    {
        var feedback = ExtractFeedbackFromGuess("zzllz", "xxxlx");
        LetterFeedback[] expectedResult =
        [
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.CorrectPosition,
            LetterFeedback.IncorrectLetter,
        ];
        Assert.That(feedback, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public void IncorrectPosition()
    {
        var feedback = ExtractFeedbackFromGuess("zzzlz", "lxxxx");
        LetterFeedback[] expectedResult =
        [
            LetterFeedback.IncorrectPosition,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
        ];
        Assert.That(feedback, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public void AllDifferentFeedbackTypesAtOnce()
    {
        var feedback = ExtractFeedbackFromGuess("abczz", "xxcba");
        LetterFeedback[] expectedResult =
        [
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.CorrectPosition,
            LetterFeedback.IncorrectPosition,
            LetterFeedback.IncorrectPosition,
        ];
        Assert.That(feedback, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public void SameLetterCorrectAndIncorrectPosition()
    {
        var feedback = ExtractFeedbackFromGuess("zzllz", "xxxll");
        LetterFeedback[] expectedFeedback =
        [
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.IncorrectLetter,
            LetterFeedback.CorrectPosition,
            LetterFeedback.IncorrectPosition,
        ];
        Assert.That(feedback, Is.EqualTo(expectedFeedback));
    }

    private LetterFeedback[] ExtractFeedbackFromGuess(string targetWord, string guess)
    {
        var dataAccess = new MockDataAccess(targetWord, guess);
        var wordle = new Game(6, dataAccess);
        var res = wordle.TakeGuess(dataAccess.Guess);
        var feedback = new LetterFeedback[res.WordAnalysis.Length];
        for (var i = 0; i < res.WordAnalysis.Length; i++)
        {
            feedback[i] = res.WordAnalysis[i].Feedback;
        }

        return feedback;
    }
}
