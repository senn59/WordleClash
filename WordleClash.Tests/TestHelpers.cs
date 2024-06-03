using WordleClash.Core;
using WordleClash.Core.Enums;
using WordleClash.Core.Interfaces;

namespace WordleClash.Tests;

public class TestHelpers
{
    public static LetterFeedback[] ExtractFeedbackFromGuess(string targetWord, string guess)
    {
        var dataAccess = new MockWordDao(targetWord, guess);
        var wordle = new Game(dataAccess, 6);
        wordle.Start();
        var res = wordle.TakeGuess(dataAccess.Guess);
        var feedback = new LetterFeedback[res.WordAnalysis.Length];
        for (var i = 0; i < res.WordAnalysis.Length; i++)
        {
            feedback[i] = res.WordAnalysis[i].Feedback;
        }

        return feedback;
    }

    public static LobbyController CreateVersusLobby(IWordDao wordDao, int additionalPlayers = 0)
    {
        IMultiplayerGame mode = new Versus(wordDao);
        return new LobbyController(mode, "player1");
    }

    public static Player GetTurnHolder(Versus game)
    {
        return game.Players.First(p => p.IsTurn == true);
    }
}