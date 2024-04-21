using System.Collections.Concurrent;
using WordleClash.Core.Interfaces;

namespace WordleClash.Core;

public class LobbyService
{
    private readonly ConcurrentDictionary<string, VersusLobby> _lobbies = new();
    private readonly IDataAccess _dataAccess;

    public LobbyService(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public string Create(Player creator)
    {
        var lobby = new VersusLobby(_dataAccess, creator);
        if (_lobbies.TryAdd(lobby.Code, lobby))
        {
            return lobby.Code;
        }

        throw new Exception("Could not create lobby");
    }

    public VersusLobby? Get(string id)
    {
        return _lobbies.GetValueOrDefault(id);
    }

    public void Dicard(string id)
    {
        _lobbies.Remove(id, out _);
    }
}