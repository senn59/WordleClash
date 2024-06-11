using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Web.Services;

namespace WordleClash.Web.Pages.Lobby;

public class CreateModel : PageModel
{
    private ILogger<CreateModel> _logger;
    private SessionManager _sessionManager;
    private LobbyService _lobby;
    
    [BindProperty]
    public string Name { get; set; }

    public CreateModel(ILogger<CreateModel> logger, LobbyService lobby, SessionManager sessionManager)
    {
        _logger = logger;
        _sessionManager = sessionManager;
        _lobby = lobby;
    }
    public void OnGet() {}

    public IActionResult OnPost()
    {
        try
        {
            var playerInfo = _lobby.CreateVersusLobby(Name);
            _sessionManager.SetPlayerInfo(playerInfo);
            return RedirectToPage("/Play/Index", new {code = playerInfo.LobbyCode});
        }
        catch
        {
            _logger.LogCritical("Something went wrong while creating lobby");
            return RedirectToPage("/Index");
        }
    }
}