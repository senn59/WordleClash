namespace WordleClash.Web.Services;

public class SessionService
{
    private const string GameSessionKey = "_Game";
    private const string Lobby = "_Lobby";
    private const string Player = "_Player";
    
    private readonly ISession _session;
    private ILogger<SessionService> _logger;
    
    public SessionService(IHttpContextAccessor httpContextAccessor, ILogger<SessionService> logger)
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
    
    public string? GetPlayerId()
    {
        return _session.GetString(Player);
    }
    
    public string? GetLobbyCode()
    {
        return _session.GetString(Lobby);
    }
    
    public bool HasLobbySessions()
    {
        var playerId = _session.GetString(Player);
        var lobbyCode = _session.GetString(Lobby);
        _logger.LogInformation($"Player session available? {playerId != null}");
        _logger.LogInformation($"Lobby session available? {lobbyCode != null}");
        return lobbyCode != null || playerId != null;
    }

    public void SetLobbySessions(string playerId, string lobbyCode)
    {
        _logger.LogInformation($"Setting sessions: player {playerId} to lobby {lobbyCode}");
        _session.SetString(Player, playerId);
        _session.SetString(Lobby, lobbyCode);
    }

    public void RemoveLobbySessions()
    {
        _session.Remove(Player);
        _session.Remove(Lobby);
    }

}