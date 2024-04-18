using System.Collections.Concurrent;
using WordleClash.Core;
using WordleClash.Core.DataAccess;

namespace WordleClash.Web.Services;

public class GameService
{
    private readonly ConcurrentDictionary<string, Game> _games = new ConcurrentDictionary<string, Game>();
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private ILogger<GameService> _logger;

    public GameService(IServiceScopeFactory serviceScopeFactory, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<GameService>();
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Game GetOrCreate(string id)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dataAccess = scope.ServiceProvider.GetRequiredService<IDataAccess>();
        return _games.GetOrAdd(id, new Game(dataAccess));
    }

    public void DicardInstance(string id)
    {
        if (!_games.TryRemove(id, out _))
        {
            _logger.LogCritical($"Failed to delete the following game instance: {id}.");
            return;
        }
        _logger.LogInformation($"Instance {id} discarded, game instance count: {_games.Count}.");
    }
}