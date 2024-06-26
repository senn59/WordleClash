using WordleClash.Core.Entities;
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

    public Game(IWordRepository wordRepository, int maxTries, string? word = null)
    {
        MaxTries = maxTries;
        if (word != null)
        {
            _wordHandler = new WordHandler(wordRepository, word);
        }
        _wordHandler = new WordHandler(wordRepository);
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

        var result = new GuessResult
        {
            Status = Status,
            WordAnalysis = _wordHandler.GetFeedback(input)
        };
        
        _guessHistory.Add(result);
        return result;
    }

    public string? GetTargetWord()
    {
        if (Status is GameStatus.Won or GameStatus.Lost)
        {
            return _wordHandler.Word;
        }
        
        return null;
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