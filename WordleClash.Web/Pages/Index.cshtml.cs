using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Web.Services;

namespace WordleClash.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private GameService _gameService;
    private SessionService _sessionService;

    public IndexModel(ILogger<IndexModel> logger, GameService gameService, SessionService sessionService)
    {
        _logger = logger;
        _gameService = gameService;
        _sessionService = sessionService;
    }

    public IActionResult OnGet()
    {
        //TODO change into middleware or caching
        var gameSession = _sessionService.GetGameId();
        if (gameSession == null) return Page();
        _logger.LogInformation($"Game instance found, discarding {gameSession}");
        _gameService.DicardInstance(gameSession);
        _sessionService.ClearGameId();
        return Page();
    }
}