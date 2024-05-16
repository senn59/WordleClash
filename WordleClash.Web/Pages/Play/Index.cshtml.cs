using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Core.Enums;
using WordleClash.Web.Services;
using Exception = System.Exception;

namespace WordleClash.Web.Pages.Play;

public class IndexModel : PageModel
{
    private ILogger<IndexModel> _logger;
    private LobbyService _lobbyService;
    private SessionService _sessionService;
    private ServerEvents _serverEvents;
    private string? _playerId;

    [BindProperty]
    public string Guess { get; set; }
    public LobbyController? Lobby { get; set; }
    public Player? ThisPlayer { get; set; }

    public IndexModel(ILogger<IndexModel> logger, SessionService sessionService, LobbyService lobbyService, ServerEvents serverEvents)
    {
        _logger = logger;
        _lobbyService = lobbyService;
        _sessionService = sessionService;
        _serverEvents = serverEvents;
        
        _playerId = _sessionService.GetPlayerId();
        if (_playerId == null) return;
        Lobby = _lobbyService.GetLobbyByPlayerId(_playerId);
        ThisPlayer = Lobby?.Players.FirstOrDefault(p => p.Id == _playerId);
    }

    public async void OnPostStartGame()
    {
        if (_playerId == null || Lobby == null || ThisPlayer?.IsOwner != true)
        {
            return;
        }
        try
        {
            Lobby.StartGame();
        }
        catch (Exception e)
        {
            _logger.LogCritical($"{e.Message} thrown while trying to start game");
        }
        await _serverEvents.UpdateField(Lobby.Code);
    }

    public async void OnPostNewGame()
    {
        if (_playerId == null || Lobby == null || ThisPlayer?.IsOwner != true || Lobby.State != LobbyState.PostGame)
        {
            return;
        }
        Lobby.Restart();
        await _serverEvents.UpdateField(Lobby.Code);
    }

    public async void OnPost()
    {
        if (Lobby == null || ThisPlayer == null || !ModelState.IsValid) return;
        try
        {
            Lobby.HandleGuess(ThisPlayer, Guess);
        }
        catch (Exception e)
        {
            _logger.LogWarning($"{e.Message} while trying to take a guess");
        }
        await _serverEvents.UpdateField(Lobby.Code);
    }
    
    public async Task<IActionResult> OnGet(string code)
    {
        if (_playerId == null)
        {
            return RedirectToPage("/Index");
        }
        if (Lobby == null || Lobby.Code != code)
        {
            return RedirectToPage("/Index");
        }
        
        await _serverEvents.UpdatePlayers(code);
        return Page();
    }

    public PartialViewResult OnGetPlayers()
    {
        if (_playerId == null)
        {
            throw new Exception("player id = null");
        }
        if (Lobby == null)
        {
            throw new Exception($"player {_playerId} is not apart of any lobby");
        }
        return Partial("Partials/Players", Lobby.Players);
    }
   
    public PartialViewResult OnGetOpponent()
    {
        if (_playerId == null)
        {
            throw new Exception("player id = null");
        }
        if (Lobby == null)
        {
            throw new Exception($"player {_playerId} is not apart of any lobby");
        }
        return Partial("Partials/Opponent", GetOpponent());
    }

    public PartialViewResult OnGetField()
    {
        if (_playerId == null)
        {
            throw new Exception("player id = null");
        }
        if (Lobby == null)
        {
            throw new Exception($"player {_playerId} is not apart of any lobby");
        }
        return Partial("Partials/MultiplayerField", this);
    }

    public Player? GetOpponent()
    {
        return Lobby?.Players.FirstOrDefault(p => p.Id != _playerId);
    }
}