using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Web.Services;

namespace WordleClash.Web.Pages.Play;

public class IndexModel : PageModel
{
    private ILogger<IndexModel> _logger;
    private LobbyService _lobby;
    private string? _playerId;
    private string? _lobbyId;
    
    public string Code { get; set; }
    public IReadOnlyList<Player> Players { get; set; }

    public IndexModel(ILogger<IndexModel> logger,SessionService sessionService, LobbyService lobbyService)
    {
        _logger = logger;
        _lobby = lobbyService;
        _playerId = sessionService.GetPlayerId();
        _lobbyId = sessionService.GetLobbyId();
    }
    public IActionResult OnGet(string code)
    {
        var lobby = _lobby.Get(code);
        if (_playerId == null || _lobbyId == null)
        {
            _logger.LogWarning($"PlayerId or LobbyId is null");
            if (lobby == null)
            {
                return RedirectToPage("/Index");
            }

            return RedirectToPage("/Lobby/Join", new { code });
        }

        if (_lobbyId != code)
        {
            return RedirectToPage("/Player/Index", new {code = _lobbyId});
        }
        
        if (lobby == null)
        {
            _logger.LogInformation($"Lobby with code \"{code}\" does not exist");
            return RedirectToPage("/Index");
        }
        Code = code;
        Players = lobby.Players;
        return Page();
    }
}