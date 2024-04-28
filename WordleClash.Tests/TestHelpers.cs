using WordleClash.Core;
using WordleClash.Core.Enums;
using WordleClash.Core.Interfaces;

namespace WordleClash.Tests;

public class TestHelpers
{
    public static LetterFeedback[] ExtractFeedbackFromGuess(string targetWord, string guess)
    {
        var dataAccess = new MockDataAccess(targetWord, guess);
        var wordle = new Game(dataAccess);
        wordle.Start(6);
        var res = wordle.TakeGuess(dataAccess.Guess);
        var feedback = new LetterFeedback[res.WordAnalysis.Length];
        for (var i = 0; i < res.WordAnalysis.Length; i++)
        {
            feedback[i] = res.WordAnalysis[i].Feedback;
        }

        return feedback;
    }

    public static LobbyController CreateVersusLobby(IDataAccess dataAccess, int additionalPlayers = 0)
    {
        var creator = new Player()
        {
            Name = "player1"
        };
        IMultiplayerGame mode = new Versus(dataAccess);
        return new LobbyController(mode, creator);
    }
}