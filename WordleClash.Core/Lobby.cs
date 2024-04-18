namespace WordleClash.Core;

public class Lobby
{
    private List<Player> _players;
    private IReadOnlyList<Player> Players => _players.AsReadOnly();
    public Player Winner { get; private set; }
    public string Code {get; private set; }

    public Lobby()
    {
        Code = GenerateCode();
    }

    public void Remove(Player player)
    {
        
    }

    private string GenerateCode()
    {
        return "A1B2C3";
    }
}