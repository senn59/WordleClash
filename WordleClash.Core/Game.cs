using WordleClash.Core.Interfaces;
using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;

namespace WordleClash.Core;

public class Game
{
    private readonly WordHandler _wordHandler;
    
    public int MaxTries { get; private init; }
    public int Tries { get; private set; }
    private readonly List<GuessResult> _guessHistory = [];
    public IReadOnlyList<GuessResult> GuessHistory => _guessHistory.AsReadOnly();
    public GameStatus Status { get; private set; } = GameStatus.AwaitStart;

    public Game(IDataAccess dataAccess, int maxTries)
    {
        MaxTries = maxTries;
        _wordHandler = new WordHandler(dataAccess);
    }
    
    public void Start()
    {
        if (Status != GameStatus.AwaitStart)
        {
            throw new GameAlreadyStartedException();
        }
        Status = GameStatus.InProgress;
    }

    public GuessResult TakeGuess(string input)
    {
        if (Status != GameStatus.InProgress)
        {
            return new GuessResult
            {
                Status = Status,
                WordAnalysis = []
            };
        }
        
        input = input.ToUpper();
        ValidateMove(input);
        Tries++;
        Console.WriteLine(_wordHandler.Word);
        
        //not sure if the 2nd statement is necessary as it shouldnt really be possible anyways
        if (_wordHandler.IsMatchingWord(input) && Tries <= MaxTries) 
        {
            Status = GameStatus.Won;
        }
        else if (Tries >= MaxTries)
        {
            Status = GameStatus.Lost;
        }
        else
        {
            Status = GameStatus.InProgress;
        }

        var result = new GuessResult()
        {
            Status = Status,
            WordAnalysis = _wordHandler.GetFeedback(input),
        };
        
        _guessHistory.Add(result);
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