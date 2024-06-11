using WordleClash.Core;

namespace WordleClash.Web.Services;

public class SessionManager
{
    private const string GameSessionKey = "_Game";
    private const string Player = "_Player";
    private const string Lobby = "_Lobby";
    
    private readonly ISession _session;
    private ILogger<SessionManager> _logger;
    
    public SessionManager(IHttpContextAccessor httpContextAccessor, ILogger<SessionManager> logger)
    {
        _logger = logger;
        if (httpContextAccessor.HttpContext == null)
        {
            throw new InvalidOperationException("HTTPContext cant be null");
        }
        _session = httpContextAccessor.HttpContext.Session;
    }
    public string GetOrCreateGameId()
    {
        if (string.IsNullOrEmpty(_session.GetString(GameSessionKey)))
        {
            _session.SetString(GameSessionKey, Guid.NewGuid().ToString());
        }
        return _session.GetString(GameSessionKey) ?? throw new NullReferenceException("Couldnt find gameId");
    }

    public void ClearGameId()
    {
        var id = _session.GetString(GameSessionKey);
        if (id == null) return;
        _session.Remove(GameSessionKey);
    }

    public string? GetGameId()
    {
        return _session.GetString(GameSessionKey);
    }

    public void SetPlayerInfo(LobbyPlayer info)
    {
        _session.SetString(Player, info.PlayerId);
        _session.SetString(Lobby, info.LobbyCode);
    }

    public LobbyPlayer? GetPlayerInfo()
    {
        var player = _session.GetString(Player);
        var lobby = _session.GetString(Lobby);
        if (player == null || lobby == null)
        {
            return null;
        }
        return new LobbyPlayer()
        {
            PlayerId = player,
            LobbyCode = lobby
        };
    }

    public void RemovePlayerSession()
    {
        _session.Remove(Player);
    }

}