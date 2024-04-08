using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Data;

namespace WordleClash.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DataAccess _dataAccess;

    public IndexModel(ILogger<IndexModel> logger, DataAccess dataAccess)
    {
        _logger = logger;
        _dataAccess = dataAccess;
    }

    public void OnGet()
    {
        var wordle = new Game(6, _dataAccess);
    }
}