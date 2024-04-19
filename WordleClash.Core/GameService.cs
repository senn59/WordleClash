using System.Collections.Concurrent;
using WordleClash.Core.DataAccess;

namespace WordleClash.Core;

public class GameService
{
    private readonly ConcurrentDictionary<string, Game> _games = new ConcurrentDictionary<string, Game>();
    private readonly IDataAccess _dataAccess;

    public GameService(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public Game GetOrCreate(string id)
    {
        return _games.GetOrAdd(id, new Game(_dataAccess));
    }

    public void DicardInstance(string id)
    {
        _games.Remove(id, out _);
    }
}