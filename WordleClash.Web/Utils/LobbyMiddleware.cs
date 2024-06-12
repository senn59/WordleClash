using WordleClash.Core;

namespace WordleClash.Web.Utils;

public class LobbyMiddleware
{
    private readonly RequestDelegate _next;

    public LobbyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, SessionManager sessionManager, LobbyService lobbyService, ServerEvents events)
    {
        var playerInfo = sessionManager.GetPlayerInfo();
        List<string> whiteListedPaths = new() { "/Play/", "/updates" };
        if (whiteListedPaths.Any(context.Request.Path.Value!.Contains) || playerInfo == null)
        {
            await _next(context);
            return;
        }
        var code = lobbyService.HandeLeave(playerInfo);
        await events.UpdatePlayers(code);
        await _next(context);
    }
}
