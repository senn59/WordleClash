using WordleClash.Core.DataAccess;

namespace WordleClash.Tests;
public class MockDataAccess : IDataAccess
{
    public string TargetWord { get; private set; }
    public string Guess { get; private set; }
    public MockDataAccess(string targetWord, string guess)
    {
        TargetWord = targetWord;
        Guess = guess;
    }
    public List<string> GetWords()
    {
        return [TargetWord, Guess];
    }

    public string GetRandomWord()
    {
        return TargetWord;
    }

    public string? GetWord(string word)
    {
        return word == TargetWord || word == Guess ? word : null;
    }
}
