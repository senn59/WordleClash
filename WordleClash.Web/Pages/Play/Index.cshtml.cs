using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Web.Services;

namespace WordleClash.Web.Pages.Play;

public class IndexModel : PageModel
{
    private LobbyService _lobby;
    private string? _playerId;
    private string? _lobbyId;
    
    public string Code { get; set; }
    public IReadOnlyList<Player> Players { get; set; }

    public IndexModel(SessionService sessionService, LobbyService lobbyService)
    {
        _lobby = lobbyService;
        _playerId = sessionService.GetPlayerId();
        _lobbyId = sessionService.GetLobbyId();
    }
    public IActionResult OnGet(string code)
    {
        if (_playerId == null || _lobbyId == null || _lobbyId != code)
        {
            return Redirect("/");
        }
        Console.WriteLine(code);
        
        var lobby = _lobby.Get(code);
        if (lobby == null)
        {
            return Redirect("/");
        }
        Code = code;
        Players = lobby.Players;
        return Page();
    }
}