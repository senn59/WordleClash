using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Core.Services;
using WordleClash.Web.Utils;

namespace WordleClash.Web.Pages.Lobby;

public class JoinModel : PageModel
{
    private ILogger<JoinModel> _logger;
    private LobbyService _lobby;
    private SessionManager _sessionManager;

    [BindProperty]
    public string? Code { get; set; }
    [BindProperty]
    public string Name { get; set; }
    
    public JoinModel(ILogger<JoinModel> logger, LobbyService lobbyService, SessionManager sessionManager)
    {
        _logger = logger;
        _lobby = lobbyService;
        _sessionManager = sessionManager;
    }
    
    public IActionResult OnGet(string? code)
    {
        if (code == null)
        {
            return Page();
        }
        
        if (!_lobby.LobbyExists(code))
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
            var playerInfo = _lobby.TryJoinLobby(Name, Code);
            _sessionManager.SetPlayerInfo(playerInfo);
            _logger.LogInformation($"Player {playerInfo.PlayerId} added to lobby {playerInfo.LobbyCode}");
            return RedirectToPage("/Play/Index", new {Code});
        }
        catch (Exception e)
        {
            _logger.LogWarning($"{e.Message} thrown while trying to add player to lobby.");
            return RedirectToPage("/Lobby/Join");
        }
    }
}