using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core;
using WordleClash.Web.Utils;

namespace WordleClash.Web.Pages.User;

public class IndexModel : PageModel
{
    private ILogger<IndexModel> _logger;
    private SessionManager _sessionManager;
    private UserService _userService;
    
    [BindProperty]
    public new required Core.User User { get; set; }

    public IndexModel(ILogger<IndexModel> logger, SessionManager sessionManager, UserService userService)
    {
        _logger = logger;
        _sessionManager = sessionManager;
        _userService = userService;
    }

    public IActionResult OnGet(string username)
    {
        _logger.LogInformation($"Trying to retrieve account \"{username}\"");
        var user = _userService.Get(username);
        if (user == null)
        {
            return RedirectToPage("/Index");
        }

        User = user;
        return Page();
    }

    public IActionResult OnPostCreate()
    {
        _logger.LogInformation("Trying to create account");
        if (_sessionManager.GetUserSession() != null)
        {
            return RedirectToPage("/Index");
        }
        var result = _userService.Create(); //TODO: catch
        _sessionManager.SetUserSession(result.SessionId);
        return RedirectToPage("Index", new {username = result.Username});
    }
}