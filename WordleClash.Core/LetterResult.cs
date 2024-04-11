using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class LetterResult
{
    public char Letter { get; init; }
    public LetterFeedback Feedback { get; init; }
}