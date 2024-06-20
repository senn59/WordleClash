using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WordleClash.Core.Exceptions;
using WordleClash.Core.Services;
using WordleClash.Web.Pages.User.Partials;
using WordleClash.Web.Utils;

namespace WordleClash.Web.Pages.User;

public class IndexModel : PageModel
{
    private ILogger<IndexModel> _logger;
    private SessionManager _sessionManager;
    private UserService _userService;
    public new required Core.Entities.User User { get; set; }
    public bool IsSelectedUser { get; set; } = false;

    [BindProperty] 
    public string NewUsername { get; set; } = "";

    // [BindProperty] 
    // public string SessionId { get; set; } = "";

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
        //TODO: catch exceptions
        if (user == null)
        {
            return RedirectToPage("/Index");
        }

        User = user;
        IsSelectedUser = _sessionManager.GetUserSession() == User.SessionId;
        // user.GameHistory.AddRange(Enumerable.Repeat(new GameLog
        // {
        //     AttemptCount = 3,
        //     Status = GameStatus.Won,
        //     Time = null,
        //     Word = "TABLE"
        // }, 10));
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

    public IActionResult OnPostLogin()
    {
        return RedirectToPage("/Index");
    }
    
    public IActionResult OnPostUpdateName()
    {
        if (!ModelState.IsValid)
        {
            return Content("ERROR!");
        }

        var userSession = _sessionManager.GetUserSession();
        if (userSession == null) return Content("User not found!");
        try
        {
            _userService.ChangeUsername(userSession, NewUsername);
        }
        catch (UsernameTakenException)
        {
            return Content($"already exists");
        }
        catch (UsernameTooLongException e)
        {
            return Content($"Max length is {e.MaxLength}");
        }
        catch (UsernameInvalidException)
        {
            return Content("Invalid username!");
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return Content("ERROR!");
        }
        
        return Content(NewUsername);
    }

    public IActionResult OnPostDelete()
    {
        var userSession = _sessionManager.GetUserSession();
        if (userSession == null)
        {
            return RedirectToPage("/Index");
        }

        var user = _userService.GetFromSession(userSession);
        if (user == null)
        {
            return RedirectToPage("/Index");
        }
        _userService.Delete(user.Id);
        _sessionManager.ClearUserSession();
        return RedirectToPage("/Index");
    }

    public IActionResult OnPostResetData()
    {
        var userSession = _sessionManager.GetUserSession();
        if (userSession == null)
        {
            return RedirectToPage("/Index");
        }
        
        var user = _userService.GetFromSession(userSession);
        if (user == null)
        {
            return RedirectToPage("/Index");
        }
        _userService.ResetData(user.Id);
        return RedirectToPage("/User/Index");
    }

    public IActionResult OnGetLog(string username, int id)
    {
        if (!ModelState.IsValid)
        {
            return Content("<h3>ERROR</h3>");
        }
        
        // var logs = Enumerable.Repeat(new GameLog
        // {
        //     AttemptCount = 3,
        //     Status = GameStatus.Won,
        //     Time = null,
        //     Word = "TABLE"
        // }, 10);
        var user = _userService.Get(username);
        if (user == null)
        {
            return Content("");
        }
        
        var logModel = new LogPartialModel
        {
            Logs = _userService.GetLogsByPage(user.Id, id),
            CurrentPage = id + 1
        };

        return Partial("Partials/Log", logModel);
    }
}