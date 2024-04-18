using WordleClash.Core.Interfaces;
using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;

namespace WordleClash.Core;

public abstract class BaseLobby
{
    protected List<Player> PlayerList = new List<Player>();
    protected IDataAccess DataAccess { get; private set; }
    
    public LobbyStatus Status { get; protected set; }
    public int MaxPlayers { get; init; }
    public int RequiredPlayers { get; init; }
    public IReadOnlyList<Player> Players => PlayerList.AsReadOnly();
    public Player? Winner { get; protected set; }
    public string Code {get; set; }

    protected BaseLobby(IDataAccess dataAccess, Player creator, int maxPlayers, int requiredPlayers)
    {
        DataAccess = dataAccess;
        MaxPlayers = maxPlayers;
        RequiredPlayers = requiredPlayers;
        Status = LobbyStatus.Initialising;
        Code = GenerateCode();
        Add(creator);
        creator.IsOwner = true;
        Status = LobbyStatus.InLobby;
    }

    public abstract void StartGame();

    public void Add(Player player)
    {
        if (PlayerList.Count <= 0 && Status != LobbyStatus.Initialising)
        {
            throw new LobbyShouldNotExistException();
        } 
        if (PlayerList.Count >= MaxPlayers)
        {
            throw new LobbyFullException(MaxPlayers);
        }
        PlayerList.Add(player);
    }

    public void Remove(Player player)
    {
        PlayerList.Remove(player);
        if (player.IsOwner && PlayerList.Count >= 1)
        {
            PlayerList[0].IsOwner = true;
        }
    }

    private string GenerateCode()
    {
        return "A1B2C3";
    }
}