using WordleClash.Core.DataAccess;

namespace WordleClash.Tests;
public class MockDataAccess : IDataAccess
{
    private string _word;
    public MockDataAccess(string word)
    {
        _word = word;
    }
    public List<string> GetWords()
    {
        return [_word];
    }
}
