using WordleClash.Core.DataAccess;
using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class Wordle
{
    private string _word;
    private int _tries;
    private int _maxTries;
    private IDataAccess _dataAccess;

    public Wordle(int maxTries, IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
        _maxTries = maxTries;
        _word = dataAccess.GetRandomWord();
    }

    public MoveResult MakeMove(string input)
    {
        if (input.Length != _word.Length)
        {
            throw new Exception();
        }

        if (_dataAccess.GetWord(input) == null)
        {
            throw new Exception();
        }
        
        _tries++;
        var feedback = GetWordFeedback(input);
        GameStatus status;
        
        //not sure if the 2nd statement is necessary as it shouldnt really be possible anyways
        if (IsCorrectWord(feedback) && _tries <= _maxTries) 
        {
            status = GameStatus.Won;
        }
        else if (_tries >= _maxTries)
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