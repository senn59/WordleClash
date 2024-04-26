using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Web.Services;

namespace WordleClash.Web.Pages.Play;

public class IndexModel : PageModel
{
    private ILogger<IndexModel> _logger;
    private LobbyService _lobby;
    private SessionService _sessionService;
    
    public string Code { get; set; }
    public IReadOnlyList<Player> Players { get; set; }

    public IndexModel(ILogger<IndexModel> logger,SessionService sessionService, LobbyService lobbyService)
    {
        _logger = logger;
        _lobby = lobbyService;
        _sessionService = sessionService;
    }
    public IActionResult OnGet(string code)
    {
        var playerId = _sessionService.GetPlayerId();
        if (playerId == null)
        {
            return RedirectToPage("/Index");
        }

        var lobby = _lobby.GetPlayerLobby(playerId);
        if (lobby == null || lobby.Code != code)
        {
            return RedirectToPage("/Index");
        }
        
        Code = code;
        Players = lobby.Players;
        return Page();
    }
}