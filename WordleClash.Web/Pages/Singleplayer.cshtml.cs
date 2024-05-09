using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Core.Enums;
using WordleClash.Web.Models;
using WordleClash.Web.Services;
using Exception = System.Exception;

namespace WordleClash.Web.Pages;

public class SingleplayerModel : PageModel
{
    private readonly ILogger<SingleplayerModel> _logger;
    private GameService _gameService;
    private SessionService _sessionService;
    private const int DefaultMaxTries = 6;

    [BindProperty] public string Guess { get; set; }
    [BindProperty] public bool NewGame { get; set; }
    
    public GameView Game { get; set; }
    
    
    public SingleplayerModel(ILogger<SingleplayerModel> logger, GameService gameService, SessionService sessionService)
    {
        _logger = logger;
        _gameService = gameService;
        _sessionService = sessionService;
    }

    public void OnGet()
    {
        var id = _sessionService.GetOrCreateGameId();
        var wordle = _gameService.GetOrCreate(id);
        if (wordle.Status == GameStatus.AwaitStart)
        {
            wordle.Start(DefaultMaxTries);
        }
        Game = GameView.FromGame(wordle);
        _logger.LogInformation($"Got game {id}");
    }

    public IActionResult OnPost()
    {
        var id = _sessionService.GetOrCreateGameId();
        if (NewGame)
        {
            _gameService.DicardInstance(id);
            return RedirectToPage("Singleplayer");
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
        return RedirectToPage("Singleplayer");
    }
}