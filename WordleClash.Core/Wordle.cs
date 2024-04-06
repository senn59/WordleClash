using WordleClash.Core.DataAccess;

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
        return words[_random.Next(1, words.Count + 1)];
    }
}