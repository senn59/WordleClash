using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WordleClash.Web.Pages.Lobby;

public class CreateModel : PageModel
{
    private ILogger<CreateModel> _logger;
    public CreateModel(ILogger<CreateModel> logger)
    {
        _logger = logger;
    }
    public void OnGet()
    {
        
    }
}