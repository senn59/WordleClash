using WordleClash.Core;

namespace WordleClash.Web.Services;
public class GameMiddleware
{
    private readonly RequestDelegate _next;

    public GameMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, SessionService sessionService, GameService gameService)
    {
        var gameSession = sessionService.GetGameId();
        List<string> whiteListedPaths = new() { "/Singleplayer" };
        if (whiteListedPaths.Any(context.Request.Path.Value!.Contains) || gameSession == null)
        {
            
            await _next(context);
            return;
        }
        gameService.DicardInstance(gameSession);
        sessionService.ClearGameId();
        await _next(context);
    }
}
