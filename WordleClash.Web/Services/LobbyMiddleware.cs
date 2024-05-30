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
        var playerInfo = sessionService.GetPlayerInfo();
        List<string> whiteListedPaths = new() { "/Play/", "/updates" };
        if (whiteListedPaths.Any(context.Request.Path.Value!.Contains) || playerInfo == null)
        {
            await _next(context);
            return;
        }
        var code = lobbyService.HandleLobbyLeave(playerInfo);
        await events.UpdatePlayers(code);
        await _next(context);
    }
}
