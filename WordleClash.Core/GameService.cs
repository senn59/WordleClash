using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using WordleClash.Core.Interfaces;

namespace WordleClash.Core;

public class GameService
{
    private readonly ConcurrentDictionary<string, Game> _games = new();
    private readonly IMemoryCache _cache;
    private readonly IWordDao _wordDao;

    public GameService(IWordDao wordDao, IMemoryCache cache)
    {
        _wordDao = wordDao;
        _cache = cache;
    }

    public Game GetOrCreate(string id, int maxTries)
    {
        _cache.TryGetValue<Game>(id, out var game);
        if (game != null) return game;
        game = new Game(_wordDao, maxTries);
        _cache.Set(id, game);
        return game;
    }

    public void DicardInstance(string id)
    {
        _cache.Remove(id);
    }
}