using WordleClash.Core.Entities;

namespace WordleClash.Web.Pages.User.Partials;

public class LogPartialModel
{
    public required List<GameLog> Logs { get; init; }
    public required int CurrentPage { get; init; }
    public static int StartPage => 1;
}