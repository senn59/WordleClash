using WordleClash.Core.Entities;
using WordleClash.Core.Interfaces;
using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class WordHandler
{
    private readonly IWordRepository _wordRepository;
    public string Word { get; private init; }

    public WordHandler(IWordRepository wordRepository)
    {
        _wordRepository = wordRepository;
        Word = _wordRepository.GetRandom().ToUpper();
    }
    
    public LetterResult[] GetFeedback(string guessedWord)
    {
        var feedbackList = new List<LetterResult>();
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
                var feedbackListLetterCount = feedbackList.Count(r => r.Letter == letter);
                var wordLetterCount = Word.Count(c => c == letter);
                if (feedbackListLetterCount >= wordLetterCount)
                {
                    feedback = LetterFeedback.IncorrectLetter;
                }
            }

            feedbackList.Add(new LetterResult
            {
                Letter = letter,
                Feedback = feedback
            });
        }
        return feedbackList.ToArray();
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
        return _wordRepository.Get(input) != null;
    }

    public bool IsCorrectLength(string input)
    {
        return Word.Length == input.Length;
    }
}