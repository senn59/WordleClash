using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class GuessResult
{
    public required GameStatus Status { get; set; }
    public required LetterFeedback[] Feedback { get; set; }
}