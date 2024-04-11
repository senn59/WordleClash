using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using Exception = System.Exception;

namespace WordleClash.Web.Pages;

public class SingleplayerModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private const string GameSessionKey = "_Game";
    private GameService _gameService;
    private SessionService _sessionService;

    [BindProperty] public string Guess { get; set; }
    public int Tries { get; set; }
    public int MaxTries { get; set; }
    public IReadOnlyList<GuessResult> MoveHistory { get; set; }
    
    public SingleplayerModel(ILogger<IndexModel> logger, GameService gameService, SessionService sessionService)
    {
        _logger = logger;
        _gameService = gameService;
        _sessionService = sessionService;
    }

    public void OnGet()
    {
        var id = _sessionService.GetOrCreateGameId(HttpContext, GameSessionKey);
        var wordle = _gameService.GetOrCreate(id);
        Tries = wordle.Tries;
        MaxTries = wordle.MaxTries;
        MoveHistory = wordle.MoveHistory;
        _logger.LogInformation("Got the game instance");
    }

    public IActionResult OnPost()
    {
        var id = _sessionService.GetOrCreateGameId(HttpContext, GameSessionKey);
        var wordle = _gameService.GetOrCreate(id);
        _logger.LogInformation("Got the game instance");
        try
        {
            wordle.TakeGuess(Guess);
            _logger.LogInformation($"user guesses {Guess}");
        }
        catch (Exception e)
        {
            _logger.LogWarning($"{e.GetType()} thrown while trying to make move.");
        }
        _logger.LogInformation($"{wordle.Tries}");
        return new RedirectToPageResult("Singleplayer");
    }
}