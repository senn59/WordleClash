using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class Player
{
    public string Id { get; }
    public bool IsOwner { get; set; }
    public required string Name { get; init; }
    public Color Color { get; set; }
    public bool? IsTurn { get; set; }
    public bool? IsWinner { get; set; }

    public Player()
    {
        Id = Guid.NewGuid().ToString();
    }
}