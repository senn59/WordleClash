using WordleClash.Core;

namespace WordleClash.Web.Pages.Shared;

public class FieldModel
{
    public GameView Game { get; set; }
    public bool HighlightTiles { get; set; } = false;
}