namespace WordleClash.Web;

public class SessionService
{
    public string GetOrCreateGameId(HttpContext context, string key)
    {
        
        if (string.IsNullOrEmpty(context.Session.GetString(key)))
        {
            context.Session.SetString(key, Guid.NewGuid().ToString());
        }
        return context.Session.GetString(key) ?? throw new NullReferenceException("Couldnt find gameId");
    }
}