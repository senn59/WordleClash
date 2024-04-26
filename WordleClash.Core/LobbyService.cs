using System.Collections.Concurrent;
using WordleClash.Core.Interfaces;

namespace WordleClash.Core;

public class LobbyService
{
    private readonly ConcurrentDictionary<string, LobbyController> _lobbies = new();
    private readonly IDataAccess _dataAccess;

    public LobbyService(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public string CreateVersus(Player creator)
    {
        var lobby = new LobbyController(new Versus(_dataAccess), creator);
        if (_lobbies.TryAdd(lobby.Code, lobby))
        {
            return lobby.Code;
        }

        throw new Exception("Could not create lobby");
    }

    public LobbyController? Get(string id)
    {
        return _lobbies.GetValueOrDefault(id);
    }

    public void Dicard(string id)
    {
        _lobbies.Remove(id, out _);
    }
}