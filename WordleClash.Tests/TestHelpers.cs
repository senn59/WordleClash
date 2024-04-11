using WordleClash.Core;
using WordleClash.Core.Enums;

namespace WordleClash.Tests;

public class TestHelpers
{
    public static LetterFeedback[] ExtractFeedbackFromGuess(string targetWord, string guess)
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