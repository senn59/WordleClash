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
    private string? _playerId;
    private string? _lobbyId;
    
    [BindProperty]
    public string Name { get; set; }

    public CreateModel(ILogger<CreateModel> logger, LobbyService lobby, SessionService sessionService)
    {
        _logger = logger;
        _sessionService = sessionService;
        _lobby = lobby;
        _playerId = sessionService.GetPlayerId();
        _lobbyId = sessionService.GetLobbyId();
    }
    public void OnGet() {}

    public IActionResult OnPost()
    {
        if (_playerId != null || _lobbyId != null)
        {
            _logger.LogWarning($"Player {_playerId} is already in lobby {_lobbyId}");
            return new RedirectToPageResult("/Play/Index", new {code = _lobbyId});
        }
        var player = new Player()
        {
            Name = Name
        };

        string code;
        try
        {
            code = _lobby.Create(player);
        }
        catch
        {
            _logger.LogCritical("Something went wrong while creating a lobby");
            return new RedirectToPageResult("/Index");
        }
        _sessionService.SetPlayerSession(player.Id);
        _sessionService.SetLobbySession(code);
        return new RedirectToPageResult("/Play/Index", new {code});
    }
}