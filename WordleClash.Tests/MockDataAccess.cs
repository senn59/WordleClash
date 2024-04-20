using WordleClash.Core.Interfaces;

namespace WordleClash.Tests;
public class MockDataAccess : IDataAccess
{
    public string TargetWord { get; private set; }
    public string Guess { get; private set; }
    public MockDataAccess(string targetWord, string guess)
    {
        TargetWord = targetWord.ToUpper();
        Guess = guess.ToUpper();
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
        word = word.ToUpper();
        return word == TargetWord || word == Guess ? word : null;
    }
}
