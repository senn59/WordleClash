using WordleClash.Core.DataAccess;
using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;

namespace WordleClash.Core;

public class Wordle
{
    private readonly string _word;
    private readonly int _maxTries;
    private readonly IDataAccess _dataAccess;
    
    public int Tries { get; private set; }

    public Wordle(int maxTries, IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
        _maxTries = maxTries;
        _word = dataAccess.GetRandomWord();
    }

    public MoveResult MakeMove(string input)
    {
        ValidateMove(input);
        Tries++;
        var feedback = GetWordFeedback(input);
        GameStatus status;
        
        //not sure if the 2nd statement is necessary as it shouldnt really be possible anyways
        if (IsCorrectWord(feedback) && Tries <= _maxTries) 
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
            Feedback = feedback
        };
    }

    private void ValidateMove(string input)
    {
        if (input.Length != _word.Length)
        {
            throw new InvalidWordException($"Word is not the correct length of {_word.Length}");
        }

        if (_dataAccess.GetWord(input) == null)
        {
            throw new InvalidWordException($"Word is not a valid word");
        }
    }

    private LetterFeedback[] GetWordFeedback(string input)
    {
        var feedback = new LetterFeedback[_word.Length];

        for (int i = 0; i < input.Length; i++)
        {
            var letter = input[i];
            if (!_word.Contains(letter))
            {
                feedback[i] = LetterFeedback.IncorrectLetter;
                continue;
            }

            if (GetAllIndexesOf(_word, letter).Contains(i))
            {
                feedback[i] = LetterFeedback.CorrectPosition;
                continue;
            }

            feedback[i] = LetterFeedback.IncorrectPosition;
        }

        return feedback;
    }

    private List<int> GetAllIndexesOf(string input, char letter)
    {
        var occurences = new List<int>();
        var index = input.IndexOf(letter);
        while (index != -1)
        {
            occurences.Add(index);
            index = input.IndexOf(letter, index + 1);
        }

        return occurences;
    }

    private bool IsCorrectWord(LetterFeedback[] feedback)
    {

        //TODO: has flaw that if array is empty true is returned
        foreach (var letterFeedback in feedback)
        {
            if (letterFeedback is LetterFeedback.IncorrectLetter or LetterFeedback.IncorrectPosition)
            {
                return false;
            }
        }
        return true;
    }
}