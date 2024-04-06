using WordleClash.Core.DataAccess;

namespace WordleClash.Core;

public class Wordle
{
    private string word;
    private int _tries = 0;
    private int _maxTries;
    private IDataAccess _dataAccess;

    public Wordle(int maxTries, IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
        _dataAccess.GetRow(1);
        _maxTries = maxTries;
        word = GenerateWord();
    }

    private string GenerateWord()
    {
        return "word";
    }
}