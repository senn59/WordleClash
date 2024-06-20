namespace WordleClash.Core.Interfaces;

public interface IWordRepository
{
    List<string> GetAll();
    string GetRandom();
    string? Get(string word);
    int? GetId(string word);
}