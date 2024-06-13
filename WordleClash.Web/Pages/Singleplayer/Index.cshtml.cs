using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Core.Enums;
using WordleClash.Web.Utils;
using Exception = System.Exception;

namespace WordleClash.Web.Pages.Singleplayer;

public class SingleplayerModel : PageModel
{
    private readonly ILogger<SingleplayerModel> _logger;
    private GameService _gameService;
    private SessionManager _sessionManager;
    private UserService _userService;
    private const int DefaultMaxTries = 6;

    [BindProperty] public string Guess { get; set; }
    [BindProperty] public bool NewGame { get; set; }
    
    public GameModel Game { get; set; }
    
    
    public SingleplayerModel(ILogger<SingleplayerModel> logger, GameService gameService, SessionManager sessionManager, UserService userService)
    {
        _logger = logger;
        _gameService = gameService;
        _sessionManager = sessionManager;
        _userService = userService;
    }

    public void OnGet()
    {
        var id = _sessionManager.GetOrCreateGameId();
        var wordle = _gameService.GetOrCreate(id, DefaultMaxTries);
        if (wordle.Status == GameStatus.AwaitStart)
        {
            wordle.Start();
        }
        Game = GameModel.FromGame(wordle);
        _logger.LogInformation($"Got game {id}");
    }

    public IActionResult OnPostNewGame()
    {
        var id = _sessionManager.GetOrCreateGameId();
        _gameService.DicardInstance(id);
        return RedirectToPage("/Singleplayer/Index");
    }

    public PartialViewResult OnPost()
    {
        var id = _sessionManager.GetOrCreateGameId();
        var wordle = _gameService.GetOrCreate(id, DefaultMaxTries);
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
        if (wordle.Status is GameStatus.Won or GameStatus.Lost)
        {
            try
            {
                var log = GameLog.FromGame(wordle);
                TrySaveGameLog(log);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        return Partial("Partials/SingleplayerField", GameModel.FromGame(wordle));
    }

    private void TrySaveGameLog(GameLog log)
    {
        var userSession = _sessionManager.GetUserSession();
        if (userSession == null) return;
        var user = _userService.GetFromSession(userSession);
        if (user == null) return;
        _userService.AddGameLog(log, userSession);
    }
}