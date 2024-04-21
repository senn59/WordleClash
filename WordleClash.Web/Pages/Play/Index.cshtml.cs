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
        var lobby = _lobby.Get(code);
        if (!_sessionService.HasLobbySessions())
        {
            if (lobby == null)
            {
                return RedirectToPage("/Index");
            }
            return RedirectToPage("/Lobby/Join", new { code });
        }

        var lobbyCode = _sessionService.GetLobbyCode();
        if (lobbyCode != code)
        {
            _logger.LogWarning($"Player in lobby {lobbyCode} is trying to access {code}");
            return RedirectToPage("/Player/Index", new {code = lobbyCode});
        }
        
        if (lobby == null)
        {
            _logger.LogWarning($"Lobby \"{code}\" does not exist");
            return RedirectToPage("/Index");
        }
        
        Code = code;
        Players = lobby.Players;
        return Page();
    }
}