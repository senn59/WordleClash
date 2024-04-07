using WordleClash.Core.DataAccess;
using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class Wordle
{
    private string _word;
    private int _tries = 0;
    private int _maxTries;
    private IDataAccess _dataAccess;
    private Random _random = new Random();

    public Wordle(int maxTries, IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
        _maxTries = maxTries;
        _word = GenerateWord();
        Console.WriteLine(_word);
    }

    private string GenerateWord()
    {
        var words = _dataAccess.GetWords();
        Console.WriteLine(words.Count);
        return words[_random.Next(0, words.Count)];
    }

    public MoveResult MakeMove(string input)
    {
        return new MoveResult()
        {
            HasWon = false,
            Feedback = GetWordFeedback(input),
        };
    }

    private LetterFeedback[] GetWordFeedback(string input)
    {
        LetterFeedback[] arr = new LetterFeedback[_word.Length];

        for (int i = 0; i < input.Length; i++)
        {
            var letter = input[i];
            if (!_word.Contains(letter))
            {
                arr[i] = LetterFeedback.IncorrectLetter;
                continue;
            }


            if (AllIndexesOf(_word, letter).Contains(i))
            {
                arr[i] = LetterFeedback.CorrectPosition;
                continue;
            }

            arr[i] = LetterFeedback.IncorrectPosition;
        }

        return arr;
    }

    private List<int> AllIndexesOf(string input, char letter)
    {
        var occurences = new List<int>();
        int index = input.IndexOf(letter);
        while (index != -1)
        {
            occurences.Add(index);
            index = input.IndexOf(letter, index + 1);
        }

        return occurences;
    }
}