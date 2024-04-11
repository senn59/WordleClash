using WordleClash.Core.Enums;

namespace WordleClash.Tests.Tests;

public class WordFeedBackTests
{
    [Test]
    public void OneCorrectLetter()
    {
        var feedback = TestHelpers.ExtractFeedbackFromGuess("table", "chime");
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
        var feedback = TestHelpers.ExtractFeedbackFromGuess("zzllz", "xxxlx");
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
        var feedback = TestHelpers.ExtractFeedbackFromGuess("zzzlz", "lxxxx");
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
        var feedback = TestHelpers.ExtractFeedbackFromGuess("abczz", "xxcba");
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
        var feedback = TestHelpers.ExtractFeedbackFromGuess("zzllz", "xxxll");
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

}
