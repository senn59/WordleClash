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

    public LobbyPlayer CreateVersus(string name)
    {
        var creator = new Player
        {
            Name = name
        };
        var lobby = new LobbyController(new Versus(_dataAccess), creator);
        if (_lobbies.TryAdd(lobby.Code, lobby))
        {
            return new LobbyPlayer
            {
                LobbyCode = lobby.Code,
                PlayerId = creator.Id
            };
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

    public LobbyPlayer? TryGetPlayerById(string playerId)
    {
        foreach (var lobby in _lobbies)
        {
            if (lobby.Value.Players.FirstOrDefault(p => p.Id == playerId) != null)
            {
                return new LobbyPlayer
                {
                    LobbyCode = lobby.Key,
                    PlayerId = playerId
                };
            }
        }
        return null;
    }

    public LobbyController? GetPlayerLobby(string playerId)
    {
        var lobby = TryGetPlayerById(playerId)?.LobbyCode;
        return lobby == null ? null : Get(lobby);
    }

    public void HandleLeave(string playerId)
    {
        var lobbyPlayer = TryGetPlayerById(playerId);
        if (lobbyPlayer == null) return;
        var lobby = Get(lobbyPlayer.LobbyCode);
        if (lobby == null) return;
        lobby.RemovePlayerById(playerId);
        if (lobby.Players.Count == 0)
        {
            _lobbies.Remove(lobby.Code, out _);
        }
    }

    public LobbyPlayer TryJoin(string playerName, string lobbyCode)
    {
        var lobbyToJoin = Get(lobbyCode);
        if (lobbyToJoin == null)
        {
            throw new Exception("Lobby does not exist");
        }

        var player = lobbyToJoin.Add(playerName);
        return new LobbyPlayer
        {
            LobbyCode = lobbyToJoin.Code,
            PlayerId = player.Id
        };
    }
}