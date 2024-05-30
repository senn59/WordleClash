using Microsoft.Extensions.Caching.Memory;
using WordleClash.Core.Interfaces;

namespace WordleClash.Core;

public class LobbyService
{
    private readonly IDataAccess _dataAccess;
    private readonly IMemoryCache _cache;

    public LobbyService(IDataAccess dataAccess, IMemoryCache cache)
    {
        _dataAccess = dataAccess;
        _cache = cache;
    }

    public PlayerLobbyInfo CreateVersusLobby(string name)
    {
        var lobby = new LobbyController(new Versus(_dataAccess), name);
        _cache.Set(lobby.Code, lobby);
        return new PlayerLobbyInfo
        {
            LobbyCode = lobby.Code,
            PlayerId = lobby.Players[0].Id
        };
    }

    public bool LobbyExists(string code)
    {
        return _cache.TryGetValue<LobbyController>(code, out _);
    }

    public string HandleLobbyLeave(PlayerLobbyInfo playerInfo)
    {
        var lobby = GetLobby(playerInfo.LobbyCode);
        if (lobby == null) return "";
        lobby.RemovePlayerById(playerInfo.PlayerId);
        if (lobby.Players.Count == 0)
        {
            DiscardLobby(lobby.Code);
        }
        return lobby.Code;
    }

    public PlayerLobbyInfo TryJoinLobby(string playerName, string lobbyCode)
    {
        var lobbyToJoin = GetLobby(lobbyCode);
        if (lobbyToJoin == null)
        {
            throw new Exception("Lobby does not exist");
        }

        var player = lobbyToJoin.Add(playerName);
        return new PlayerLobbyInfo
        {
            LobbyCode = lobbyToJoin.Code,
            PlayerId = player.Id
        };
    }

    public LobbyController? GetPlayerLobby(PlayerLobbyInfo playerInfo)
    {
        return GetLobby(playerInfo.LobbyCode);
    }
    
    private LobbyController? GetLobby(string id)
    {
         _cache.TryGetValue<LobbyController>(id, out var lobby);
         return lobby;
    }

    private void DiscardLobby(string id)
    {
        _cache.Remove(id);
    }
}