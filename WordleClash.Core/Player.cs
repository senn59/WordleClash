using WordleClash.Core.Enums;

namespace WordleClash.Core;

public class Player
{
    public int Id { get; set; }
    public bool IsOwner { get; set; }
    public string Name { get; private set; }
    public Color Color { get; private set; }

    public Player()
    {
        Id = 1;
    }

    public void Join(Lobby lobby)
    {
        
    }
}