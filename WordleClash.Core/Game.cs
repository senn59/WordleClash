using WordleClash.Core.DataAccess;
using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;

namespace WordleClash.Core;

public class Game
{
    private readonly WordHandler _wordHandler;
    private readonly int _maxTries;
    private readonly IDataAccess _dataAccess;
    
    public int Tries { get; private set; }

    public Game(int maxTries, IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
        _maxTries = maxTries;
        _wordHandler = new WordHandler(_dataAccess);
    }

    public MoveResult MakeMove(string input)
    {
        ValidateMove(input);
        Tries++;
        GameStatus status;
        
        //not sure if the 2nd statement is necessary as it shouldnt really be possible anyways
        if (_wordHandler.IsMatchingWord(input) && Tries <= _maxTries) 
        {
            status = GameStatus.Won;
        }
        else if (Tries >= _maxTries)
        {
            status = GameStatus.Lost;
        }
        else
        {
            status = GameStatus.InProgress;
        }
        
        return new MoveResult()
        {
            Status = status,
            Feedback = _wordHandler.GetFeedback(input)
        };
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