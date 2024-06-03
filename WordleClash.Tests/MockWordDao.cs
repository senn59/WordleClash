using WordleClash.Core.Interfaces;

namespace WordleClash.Tests;
public class MockWordDao : IWordDao
{
    public string TargetWord { get; private set; }
    public string Guess { get; private set; }
    public MockWordDao(string targetWord, string guess)
    {
        TargetWord = targetWord.ToUpper();
        Guess = guess.ToUpper();
    }
    public List<string> GetAll()
    {
        return [TargetWord, Guess];
    }

    public string GetRandomWord()
    {
        return TargetWord;
    }

    public string? Get(string word)
    {
        word = word.ToUpper();
        return word == TargetWord || word == Guess ? word : null;
    }

    public int? GetId(string word)
    {
        throw new NotImplementedException();
    }
}
