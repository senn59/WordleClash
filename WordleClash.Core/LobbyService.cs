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

    public PlayerLobbyInfo CreateVersusLobby(string name)
    {
        var lobby = new LobbyController(new Versus(_dataAccess), name);
        if (_lobbies.TryAdd(lobby.Code, lobby))
        {
            return new PlayerLobbyInfo
            {
                LobbyCode = lobby.Code,
                PlayerId = lobby.Players[0].Id
            };
        }

        throw new Exception("Could not create lobby");
    }

    public bool LobbyExists(string code)
    {
        return _lobbies.ContainsKey(code);
    }

    public LobbyController? GetLobbyByPlayerId(string playerId)
    {
        var lobby = GetPlayerInfo(playerId)?.LobbyCode;
        return lobby == null ? null : GetLobby(lobby);
    }

    public string HandleLobbyLeave(string playerId)
    {
        var lobbyPlayer = GetPlayerInfo(playerId);
        if (lobbyPlayer == null) return "";
        
        var lobby = GetLobby(lobbyPlayer.LobbyCode);
        if (lobby == null) return "";
        
        lobby.RemovePlayerById(playerId);
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
    
    private LobbyController? GetLobby(string id)
    {
        return _lobbies.GetValueOrDefault(id);
    }

    private void DiscardLobby(string id)
    {
        _lobbies.Remove(id, out _);
    }

    private PlayerLobbyInfo? GetPlayerInfo(string playerId)
    {
        foreach (var lobby in _lobbies)
        {
            if (lobby.Value.Players.FirstOrDefault(p => p.Id == playerId) != null)
            {
                return new PlayerLobbyInfo
                {
                    LobbyCode = lobby.Key,
                    PlayerId = playerId
                };
            }
        }
        return null;
    }

}