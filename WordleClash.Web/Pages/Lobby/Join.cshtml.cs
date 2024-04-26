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
        
        var lobby = _lobby.Get(Code);
        if (lobby == null)
        {
            _logger.LogInformation("Lobby not found");
            return RedirectToPage("/Lobby/Join");
        }

        try
        {
            var player = lobby.Add(Name);
            _sessionService.SetPlayerId(player.Id);
            _logger.LogInformation($"Player {player.Id} added to lobby {lobby.Code}");
        }
        catch (Exception e)
        {
            _logger.LogWarning($"{e.GetType()} thrown while trying to add player to lobby.");
            return RedirectToPage("/Index");
        }
        
        return RedirectToPage("/Play/Index", new {Code});
    }
}