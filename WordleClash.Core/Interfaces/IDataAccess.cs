namespace WordleClash.Core.Interfaces;

public interface IDataAccess
{
    List<string> GetWords();
    string GetRandomWord();
    string? GetWord(string word);
}