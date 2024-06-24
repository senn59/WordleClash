using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Core.Entities;
using WordleClash.Core.Enums;
using WordleClash.Core.Services;
using WordleClash.Web.Utils;
using Exception = System.Exception;

namespace WordleClash.Web.Pages.Play;

public class IndexModel : PageModel
{
    private ILogger<IndexModel> _logger;
    private LobbyService _lobbyService;
    private SessionManager _sessionManager;
    private ServerEvents _serverEvents;
    private LobbyPlayer? _playerInfo;

    [BindProperty]
    public string Guess { get; set; }
    public LobbyController? Lobby { get; set; }
    public Player? ThisPlayer { get; set; }

    public IndexModel(ILogger<IndexModel> logger, SessionManager sessionManager, LobbyService lobbyService, ServerEvents serverEvents)
    {
        _logger = logger;
        _lobbyService = lobbyService;
        _sessionManager = sessionManager;
        _serverEvents = serverEvents;
        
        _playerInfo = _sessionManager.GetPlayerInfo();
        if (_playerInfo == null) return;
        Lobby = _lobbyService.GetPlayerLobby(_playerInfo);
        ThisPlayer = Lobby?.Players.FirstOrDefault(p => p.Id == _playerInfo.PlayerId);
    }

    public async void OnPostStartGame()
    {
        if (_playerInfo == null || Lobby == null || ThisPlayer?.IsOwner != true)
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
        if (_playerInfo == null || Lobby == null || ThisPlayer?.IsOwner != true || Lobby.State != LobbyState.PostGame)
        {
            return;
        }
        Lobby.Restart();
        await _serverEvents.UpdateField(Lobby.Code);
    }

    public async void OnPost()
    {
        if (Lobby == null || ThisPlayer == null || !ModelState.IsValid)
        {
            return;
        }
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
        if (_playerInfo == null || Lobby == null || Lobby.Code != code)
        {
            return RedirectToPage("/Lobby/Join", new { code });
        }
        
        await _serverEvents.UpdatePlayers(code);
        return Page();
    }

    public PartialViewResult OnGetPlayers()
    {
        if (_playerInfo == null)
        {
            throw new Exception("player id = null");
        }
        if (Lobby == null)
        {
            throw new Exception($"player {_playerInfo} is not apart of any lobby");
        }
        return Partial("Partials/Players", Lobby.Players);
    }
   
    public PartialViewResult OnGetOpponent()
    {
        if (_playerInfo == null)
        {
            throw new Exception("player id = null");
        }
        if (Lobby == null)
        {
            throw new Exception($"player {_playerInfo} is not apart of any lobby");
        }
        return Partial("Partials/Opponent", GetOpponent());
    }

    public PartialViewResult OnGetField()
    {
        if (_playerInfo == null)
        {
            throw new Exception("player id = null");
        }
        if (Lobby == null)
        {
            throw new Exception($"player {_playerInfo} is not apart of any lobby");
        }
        return Partial("Partials/MultiplayerField", this);
    }
    
    public PartialViewResult OnGetOverview()
    {
        if (_playerInfo == null)
        {
            throw new Exception("player id = null");
        }
        if (Lobby == null)
        {
            throw new Exception($"player {_playerInfo} is not apart of any lobby");
        }
        return Partial("Shared/LetterOverview", Lobby.Games[0].GuessHistory);
    }

    public Player? GetOpponent()
    {
        return Lobby?.Players.FirstOrDefault(p => p.Id != _playerInfo?.PlayerId);
    }
}