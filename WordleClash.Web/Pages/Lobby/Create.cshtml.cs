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
        try
        {
            var playerInfo = _lobby.CreateVersusLobby(Name);
            _sessionService.SetPlayerInfo(playerInfo);
            return RedirectToPage("/Play/Index", new {code = playerInfo.LobbyCode});
        }
        catch
        {
            _logger.LogCritical("Something went wrong while creating lobby");
            return RedirectToPage("/Index");
        }
    }
}