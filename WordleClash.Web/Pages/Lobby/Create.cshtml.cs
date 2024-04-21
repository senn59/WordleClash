using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Web.Services;

namespace WordleClash.Web.Pages.Lobby;

public class CreateModel : PageModel
{
    private ILogger<CreateModel> _logger;
    private SessionService _sessionService;
    private LobbyService _lobby;
    
    [BindProperty]
    public string Name { get; set; }

    public CreateModel(ILogger<CreateModel> logger, LobbyService lobby, SessionService sessionService)
    {
        _logger = logger;
        _sessionService = sessionService;
        _lobby = lobby;
    }
    public void OnGet() {}

    public IActionResult OnPost()
    {
        if (_sessionService.HasLobbySessions())
        {
            _logger.LogWarning($"Player is already in lobby, redirecting");
            return RedirectToPage("/Play/Index", new {code = _sessionService.GetLobbyCode()});
        }
        
        var player = new Player() { Name = Name };
        string code;
        try
        {
            code = _lobby.Create(player);
        }
        catch
        {
            _logger.LogCritical("Something went wrong while creating lobby");
            return RedirectToPage("/Index");
        }
        
        _sessionService.SetLobbySessions(player.Id, code);
        return RedirectToPage("/Play/Index", new {code});
    }
}