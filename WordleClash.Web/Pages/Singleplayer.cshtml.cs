using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Core.Enums;
using WordleClash.Web.Services;
using Exception = System.Exception;

namespace WordleClash.Web.Pages;

public class SingleplayerModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private GameService _gameService;
    private SessionService _sessionService;

    [BindProperty] public string Guess { get; set; }
    [BindProperty] public bool NewGame { get; set; }
    public int Tries { get; set; }
    public int MaxTries { get; set; }
    public GameStatus Status { get; set; }
    public IReadOnlyList<GuessResult> MoveHistory { get; set; }
    
    public SingleplayerModel(ILogger<IndexModel> logger, GameService gameService, SessionService sessionService)
    {
        _logger = logger;
        _gameService = gameService;
        _sessionService = sessionService;
    }

    public void OnGet()
    {
        var id = _sessionService.GetOrCreateGameId();
        var wordle = _gameService.GetOrCreate(id);
        if (wordle.GameStatus == GameStatus.AwaitStart)
        {
            wordle.Start(6);
        }
        Tries = wordle.Tries;
        MaxTries = wordle.MaxTries;
        MoveHistory = wordle.MoveHistory;
        Status = wordle.GameStatus;
        _logger.LogInformation($"Got game {id}");
    }

    public IActionResult OnPost()
    {
        var id = _sessionService.GetOrCreateGameId();
        if (NewGame)
        {
            _gameService.DicardInstance(id);
            return new RedirectToPageResult("Singleplayer");
        }
        var wordle = _gameService.GetOrCreate(id);
        _logger.LogInformation($"Got game {id} ");
        try
        {
            wordle.TakeGuess(Guess);
            _logger.LogInformation($"User attempt: {Guess}");
        }
        catch (Exception e)
        {
            _logger.LogWarning($"{e.GetType()} thrown while trying to make move.");
        }
        return new RedirectToPageResult("Singleplayer");
    }
}