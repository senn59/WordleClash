using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class GuessResult
{
    public required GameStatus Status { get; init; }
    public required LetterResult[] WordAnalysis { get; init; }
}