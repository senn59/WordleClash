using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Web.Models;
using WordleClash.Web.Services;

namespace WordleClash.Web.Pages.Play;

public class IndexModel : PageModel
{
    private ILogger<IndexModel> _logger;
    private LobbyService _lobby;
    private SessionService _sessionService;
    private ServerEvents _serverEvents;
    
    public LobbyController Lobby { get; set; }
    public IEnumerable<Player> Others => Lobby.Players.Where(p => p.Id != _sessionService.GetPlayerId());

    public IndexModel(ILogger<IndexModel> logger, SessionService sessionService, LobbyService lobbyService, ServerEvents serverEvents)
    {
        _logger = logger;
        _lobby = lobbyService;
        _sessionService = sessionService;
        _serverEvents = serverEvents;
    }
    public async Task<IActionResult> OnGet(string code)
    {
        var playerId = _sessionService.GetPlayerId();
        if (playerId == null)
        {
            return RedirectToPage("/Index");
        }

        var lobby = _lobby.GetLobbyByPlayerId(playerId);
        if (lobby == null || lobby.Code != code)
        {
            return RedirectToPage("/Index");
        }
        
        Lobby = lobby;
        await _serverEvents.UpdatePlayers(code);
        return Page();
    }

    public PartialViewResult OnGetPlayers()
    {
        var playerId = _sessionService.GetPlayerId();
        if (playerId == null)
        {
            throw new Exception("player id = null");
        }

        var lobby = _lobby.GetLobbyByPlayerId(playerId);
        if (lobby == null)
        {
            throw new Exception($"player {playerId} is not apart of any lobby");
        }
        return Partial("Players", lobby.Players);
    }
    
    public PartialViewResult OnGetOpponent()
    {
        var playerId = _sessionService.GetPlayerId();
        if (playerId == null)
        {
            throw new Exception("player id = null");
        }

        var lobby = _lobby.GetLobbyByPlayerId(playerId);
        if (lobby == null)
        {
            throw new Exception($"player {playerId} is not apart of any lobby");
        }
        return Partial("Versus/Opponent", lobby.Players.FirstOrDefault(p => p.Id == playerId));
    }
}