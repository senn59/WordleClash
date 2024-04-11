using WordleClash.Core.DataAccess;
using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;

namespace WordleClash.Core;

public class Game
{
    private readonly WordHandler _wordHandler;
    
    public int MaxTries { get; private set; }
    public int Tries { get; private set; }
    private readonly List<MoveResult> _moveHistory = [];
    public IReadOnlyList<MoveResult> MoveHistory => _moveHistory;

    public Game(int maxTries, IDataAccess dataAccess)
    {
        MaxTries = maxTries;
        _wordHandler = new WordHandler(dataAccess);
    }

    public MoveResult MakeMove(string input)
    {
        ValidateMove(input);
        Tries++;
        Console.WriteLine(_wordHandler.Word);
        Console.WriteLine(Tries);
        GameStatus status;
        
        //not sure if the 2nd statement is necessary as it shouldnt really be possible anyways
        if (_wordHandler.IsMatchingWord(input) && Tries <= MaxTries) 
        {
            status = GameStatus.Won;
        }
        else if (Tries >= MaxTries)
        {
            status = GameStatus.Lost;
        }
        else
        {
            status = GameStatus.InProgress;
        }

        var result = new MoveResult()
        {
            Status = status,
            Feedback = _wordHandler.GetFeedback(input)
        };
        
        _moveHistory.Add(result);
        return result;
    }

    private void ValidateMove(string input)
    {
        if (!_wordHandler.IsCorrectLength(input))
        {
            throw new IncorrectLengthException(_wordHandler.Word.Length);
        }

        if (!_wordHandler.IsExistingWord(input))
        {
            throw new InvalidWordException(input);
        }
    }
}