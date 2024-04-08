using WordleClash.Core.DataAccess;
using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class WordHandler
{
    private readonly IDataAccess _dataAccess;
    public string Word { get; private init; }

    public WordHandler(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
        Word = _dataAccess.GetRandomWord();
    }
    
    
    public LetterFeedback[] GetFeedback(string guessedWord)
    {
        var feedback = new LetterFeedback[Word.Length];

        for (int i = 0; i < guessedWord.Length; i++)
        {
            var letter = guessedWord[i];
            if (!Word.Contains(letter))
            {
                feedback[i] = LetterFeedback.IncorrectLetter;
                continue;
            }

            if (GetAllIndexesOf(letter).Contains(i))
            {
                feedback[i] = LetterFeedback.CorrectPosition;
                continue;
            }

            feedback[i] = LetterFeedback.IncorrectPosition;
        }

        return feedback;
    }
    
    private List<int> GetAllIndexesOf(char letter)
    {
        var occurences = new List<int>();
        var index = Word.IndexOf(letter);
        while (index != -1)
        {
            occurences.Add(index);
            index = Word.IndexOf(letter, index + 1);
        }

        return occurences;
    }
    
    public bool IsMatchingWord(string input)
    {
        return input.Equals(Word, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsExistingWord(string input)
    {
        return _dataAccess.GetWord(input) != null;
    }

    public bool IsCorrectLength(string input)
    {
        return Word.Length == input.Length;
    }
}