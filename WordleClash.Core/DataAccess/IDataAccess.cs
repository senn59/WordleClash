namespace WordleClash.Core.DataAccess;

public interface IDataAccess
{
    List<string> GetWords();
    string GetRandomWord();
    string? GetWord(string word);
}