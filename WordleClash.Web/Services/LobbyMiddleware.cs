using WordleClash.Core;

namespace WordleClash.Web.Services;

public class LobbyMiddleware
{
    private readonly RequestDelegate _next;

    public LobbyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, SessionService sessionService, LobbyService lobbyService)
    {
        var playerId = sessionService.GetPlayerId();
        if (context.Request.Path.Value!.Contains("/Play/") || playerId == null)
        {
            await _next(context);
            return;
        }
        lobbyService.HandleLobbyLeave(playerId);
        await _next(context);
    }
}
