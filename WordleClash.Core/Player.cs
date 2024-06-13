using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class Player
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public bool IsOwner { get; private set; }
    public required string Name { get; init; }
    public Color Color { get; init; }
    public bool? IsTurn { get; private set; } = false;
    public bool? IsWinner { get; private set; } = false;

    public void SetTurn(bool? value)
    {
        IsTurn = value;
    }

    public void SetWinner()
    {
        IsWinner = true;
    }

    public void SetOwner()
    {
        IsOwner = true;
    }
}