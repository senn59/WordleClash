using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Core.DataAccess;

namespace WordleClash.Web.Pages;

public class SingleplayerModel : PageModel
{
   private readonly ILogger<IndexModel> _logger;

    private Game _wordle;
    
    public SingleplayerModel(ILogger<IndexModel> logger, Game game)
    {
        _logger = logger;
        _wordle = game;
    }

    public void OnGet()
    {
    }

    public void OnPost()
    {
        _wordle.MakeMove("table");
        _logger.LogInformation($"{_wordle.Tries}");
    }
}