using WordleClash.Core.Enums;

namespace WordleClash.Core.Entities;

public class Player
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public bool IsOwner { get; private set; }
    public required string Name { get; init; }
    public Color Color { get; init; }
    public bool? IsTurn { get; private set; } = false;
    public bool? IsWinner { get; private set; } = false;
    public Game? Game { get; private set; }

    public void SetTurn(bool? value)
    {
        IsTurn = value;
    }

    public void SetWinner(bool value = true)
    {
        IsWinner = value;
    }
    
    public void SetGame(Game game)
    {
        Game = game;
    }

    public void SetOwner()
    {
        IsOwner = true;
    }
}