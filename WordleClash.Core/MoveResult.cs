using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class MoveResult
{
    public bool HasWon { get; set; }
    public LetterFeedback[] Feedback { get; set; }
}