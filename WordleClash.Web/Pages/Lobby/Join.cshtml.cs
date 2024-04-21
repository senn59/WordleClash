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
    private string? _playerId;
    private string? _lobbyId;

    [BindProperty]
    public string? Code { get; set; }
    [BindProperty]
    public string Name { get; set; }
    
    public JoinModel(ILogger<JoinModel> logger, LobbyService lobbyService, SessionService sessionService)
    {
        _logger = logger;
        _lobby = lobbyService;
        _sessionService = sessionService;
        _playerId = sessionService.GetPlayerId();
        _lobbyId = sessionService.GetLobbyId();
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
        if (_playerId != null || _lobbyId != null)
        {
            _logger.LogWarning($"Player {_playerId} is already in lobby {_lobbyId}");
            return new RedirectToPageResult("/Play/Index", new {code = _lobbyId});
        }
        if (Code == null)
        {
            _logger.LogInformation("Code has a value of null");
            return new RedirectToPageResult("/Lobby/Join");
        }

        var lobby = _lobby.Get(Code);
        if (lobby == null)
        {
            _logger.LogInformation("Lobby not found");
            return new RedirectToPageResult("/Lobby/Join");
        }
        var player = new Player()
        {
            Name = Name
        };
        try
        {
            lobby.Add(player);
        }
        catch (Exception e)
        {
            _logger.LogWarning($"{e.GetType()} thrown while trying to make move.");
            return new RedirectToPageResult("/Index");
        }
        _sessionService.SetPlayerSession(player.Id);
        _sessionService.SetLobbySession(lobby.Code);
        _logger.LogInformation($"Player {player.Id} added to lobby {lobby.Code}");
        return new RedirectToPageResult("/Play/Index", new {Code});
    }
}