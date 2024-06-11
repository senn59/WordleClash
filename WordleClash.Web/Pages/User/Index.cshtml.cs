using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Web.Services;

namespace WordleClash.Web.Pages.Profile;

public class IndexModel : PageModel
{
    private ILogger<IndexModel> _logger;
    private SessionManager _sessionManager;

    public IndexModel(ILogger<IndexModel> logger, SessionManager sessionManager)
    {
        _logger = logger;
        _sessionManager = sessionManager;
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