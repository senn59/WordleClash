using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class LetterResult
{
    public required char Letter { get; init; }
    public required LetterFeedback Feedback { get; init; }
}