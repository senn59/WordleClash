namespace WordleClash.Web.Services;

public class SessionService
{
    private const string GameSessionKey = "_Game";
    
    private readonly ISession _session;
    public SessionService(IHttpContextAccessor httpContextAccessor)
    {
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

    public string GetLobbyId()
    {
        return "";
    }
}