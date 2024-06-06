namespace WordleClash.Core.Interfaces;

public interface IWordRepository
{
    List<string> GetAll();
    string GetRandomWord();
    string? Get(string word);
    int? GetId(string word);
}