using System.Collections.Concurrent;
using WordleClash.Core;
using WordleClash.Core.DataAccess;

namespace WordleClash.Web;

public class GameService
{
    private readonly ConcurrentDictionary<string, Game> _games = new ConcurrentDictionary<string, Game>();
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public GameService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Game GetOrCreate(string id)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dataAccess = scope.ServiceProvider.GetRequiredService<IDataAccess>();
        return _games.GetOrAdd(id, new Game(6, dataAccess));
    }
}