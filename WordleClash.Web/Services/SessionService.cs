namespace WordleClash.Web;

public class SessionService
{
    private const string GameSessionKey = "_Game";
    public string GetOrCreateGameId(HttpContext context)
    {
        
        if (string.IsNullOrEmpty(context.Session.GetString(GameSessionKey)))
        {
            context.Session.SetString(GameSessionKey, Guid.NewGuid().ToString());
        }
        return context.Session.GetString(GameSessionKey) ?? throw new NullReferenceException("Couldnt find gameId");
    }

    public void ClearGameId(HttpContext context)
    {
        var id = context.Session.GetString(GameSessionKey);
        if (id == null) return;
        context.Session.Remove(GameSessionKey);
    }
}