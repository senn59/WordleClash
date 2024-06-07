using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using WordleClash.Core.Interfaces;

namespace WordleClash.Core;

public class GameService
{
    private readonly IMemoryCache _cache;
    private readonly IWordRepository _wordRepository;

    public GameService(IWordRepository wordRepository, IMemoryCache cache)
    {
        _wordRepository = wordRepository;
        _cache = cache;
    }

    public Game GetOrCreate(string id, int maxTries)
    {
        _cache.TryGetValue<Game>(id, out var game);
        if (game != null) return game;
        game = new Game(_wordRepository, maxTries);
        _cache.Set(id, game);
        return game;
    }

    public void DicardInstance(string id)
    {
        _cache.Remove(id);
    }
}