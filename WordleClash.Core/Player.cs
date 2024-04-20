using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class Player
{
    public int Id { get; set; }
    public bool IsOwner { get; set; }
    public required string Name { get; set; }
    public Color Color { get; set; }
    public bool? IsTurn { get; set; }

    public Player()
    {
        Id = 1;
    }
}