using WordleClash.Core;

namespace WordleClash.Web.Services;

public class LobbyMiddleware
{
    private readonly RequestDelegate _next;

    public LobbyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, SessionService sessionService, LobbyService lobbyService, ServerEvents events)
    {
        var playerId = sessionService.GetPlayerId();
        List<string> whiteListedPaths = new() { "/Play/", "/updates" };
        if (whiteListedPaths.Any(context.Request.Path.Value!.Contains) || playerId == null)
        {
            await _next(context);
            return;
        }
        lobbyService.HandleLobbyLeave(playerId);
        await events.UpdatePlayers("");
        await _next(context);
    }
}
