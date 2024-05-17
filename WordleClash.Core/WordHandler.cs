using WordleClash.Core.Interfaces;
using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class WordHandler
{
    private readonly IDataAccess _dataAccess;
    public string Word { get; private init; }

    public WordHandler(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
        Word = _dataAccess.GetRandomWord().ToUpper();
    }
    
    public LetterResult[] GetFeedback(string guessedWord)
    {
        var feedbackList = new LetterResult[Word.Length];
        for (var i = 0; i < guessedWord.Length; i++)
        {
            LetterFeedback feedback;
            var letter = guessedWord[i];
            
            if (!Word.Contains(letter))
            {
                feedback = LetterFeedback.IncorrectLetter;
            } 
            else if (GetAllIndexesOf(letter).Contains(i))
            {
                feedback = LetterFeedback.CorrectPosition;
            }
            else
            {
                feedback = LetterFeedback.IncorrectPosition;
            }

            feedbackList[i] = new LetterResult
            {
                Letter = letter,
                Feedback = feedback
            };
        }
        return feedbackList;
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