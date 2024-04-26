using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Web.Services;

namespace WordleClash.Web.Pages.Lobby;

public class JoinModel : PageModel
{
    private ILogger<JoinModel> _logger;
    private LobbyService _lobby;
    private SessionService _sessionService;

    [BindProperty]
    public string? Code { get; set; }
    [BindProperty]
    public string Name { get; set; }
    
    public JoinModel(ILogger<JoinModel> logger, LobbyService lobbyService, SessionService sessionService)
    {
        _logger = logger;
        _lobby = lobbyService;
        _sessionService = sessionService;
    }
    
    public IActionResult OnGet(string? code)
    {
        if (code == null)
        {
            return Page();
        }
        
        if (_lobby.Get(code) == null)
        {
            Code = null;
            return Page();
        }
        
        Code = code;
        return Page();
    }

    public IActionResult OnPost()
    {
        if (Code == null)
        {
            _logger.LogInformation("Code is null");
            return RedirectToPage("/Lobby/Join");
        }

        if (!ModelState.IsValid) return RedirectToPage("/Lobby/Join");
        try
        {
            var lobbyPlayer = _lobby.TryJoin(Name, Code);
            _sessionService.SetPlayerId(lobbyPlayer.PlayerId);
            _logger.LogInformation($"Player {lobbyPlayer.PlayerId} added to lobby {lobbyPlayer.LobbyCode}");
            return RedirectToPage("/Play/Index", new {Code});
        }
        catch (Exception e)
        {
            _logger.LogWarning($"{e.GetType()} thrown while trying to add player to lobby.");
            return RedirectToPage("/Lobby/Join");
        }
    }
}