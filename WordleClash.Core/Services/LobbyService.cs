using Microsoft.Extensions.Caching.Memory;
using WordleClash.Core.Entities;
using WordleClash.Core.Interfaces;

namespace WordleClash.Core.Services;

public class LobbyService
{
    private readonly IWordRepository _wordRepository;
    private readonly IMemoryCache _cache;

    public LobbyService(IWordRepository wordRepository, IMemoryCache cache)
    {
        _wordRepository = wordRepository;
        _cache = cache;
    }

    public LobbyPlayer CreateVersusLobby(string name)
    {
        var lobby = new LobbyController(new Versus(_wordRepository), name);
        _cache.Set(lobby.Code, lobby);
        return new LobbyPlayer
        {
            LobbyCode = lobby.Code,
            PlayerId = lobby.Players[0].Id
        };
    }

    public bool LobbyExists(string code)
    {
        return _cache.TryGetValue<LobbyController>(code, out _);
    }

    public string HandeLeave(LobbyPlayer playerInfo)
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

    public LobbyPlayer TryJoinLobby(string playerName, string lobbyCode)
    {
        var lobbyToJoin = GetLobby(lobbyCode);
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

    public LobbyController? GetPlayerLobby(LobbyPlayer playerInfo)
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