using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class MoveResult
{
    public required GameStatus Status { get; set; }
    public required LetterFeedback[] Feedback { get; set; }
}