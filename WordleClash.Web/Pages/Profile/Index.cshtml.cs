using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Web.Services;

namespace WordleClash.Web.Pages.Profile;

public class IndexModel : PageModel
{
    private ILogger<IndexModel> _logger;
    private SessionService _sessionService;

    public IndexModel(ILogger<IndexModel> logger, SessionService sessionService)
    {
        _logger = logger;
        _sessionService = sessionService;
    }

    public void OnGet()
    {
        _logger.LogInformation("Trying to retrieve account");
    }

    public IActionResult OnPostCreate()
    {
        _logger.LogInformation("Trying to create account");
        return RedirectToPage("Index");
    }
}