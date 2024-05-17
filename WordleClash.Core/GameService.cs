using System.Collections.Concurrent;
using WordleClash.Core.Interfaces;

namespace WordleClash.Core;

public class GameService
{
    private readonly ConcurrentDictionary<string, Game> _games = new();
    private readonly IDataAccess _dataAccess;

    public GameService(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public Game GetOrCreate(string id, int maxTries)
    {
        return _games.GetOrAdd(id, new Game(_dataAccess, maxTries));
    }

    public void DicardInstance(string id)
    {
        _games.Remove(id, out _);
    }
}