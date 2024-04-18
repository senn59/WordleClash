using WordleClash.Core.Interfaces;
using WordleClash.Core.Enums;

namespace WordleClash.Core;

public abstract class BaseLobby
{
    protected List<Player> PlayerList = new List<Player>();
    protected IDataAccess DataAccess { get; private set; }
    protected LobbyStatus Status { get; set; } = LobbyStatus.InLobby;
    
    public int MaxPlayers { get; init; }
    public int RequiredPlayers { get; init; }
    public IReadOnlyList<Player> Players => PlayerList.AsReadOnly();
    public Player? Winner { get; protected set; }
    public string Code {get; set; }

    protected BaseLobby(IDataAccess dataAccess)
    {
        DataAccess = dataAccess;
        Code = GenerateCode();
    }

    public abstract void StartGame();

    public void Join(Player player)
    {
        if (PlayerList.Count <= 0)
        {
            throw new Exception("Lobby should be destroyed");
        } 
        if (PlayerList.Count >= MaxPlayers)
        {
            throw new Exception("Lobby full");
        }
        PlayerList.Add(player);
    }

    public void Kick(Player player)
    {
        PlayerList.Remove(player);
    }

    private string GenerateCode()
    {
        return "A1B2C3";
    }
}