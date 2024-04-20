using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WordleClash.Web.Pages.Lobby;

public class CreateModel : PageModel
{
    private ILogger<CreateModel> _logger;
    
    [BindProperty]
    public string Name { get; set; }

    public CreateModel(ILogger<CreateModel> logger)
    {
        _logger = logger;
    }
    public void OnGet() {}

    public void OnPost()
    {
        _logger.LogInformation(Name);
    }
}