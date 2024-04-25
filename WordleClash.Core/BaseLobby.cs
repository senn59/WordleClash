using WordleClash.Core.Interfaces;
using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;

namespace WordleClash.Core;

public abstract class BaseLobby
{
    private const int CodeLength = 6;
    
    protected List<Player> PlayerList = new List<Player>();
    protected IDataAccess DataAccess { get; private set; }
    
    public LobbyState Status { get; protected set; }
    public int MaxPlayers { get; init; }
    public int RequiredPlayers { get; init; }
    public IReadOnlyList<Player> Players => PlayerList.AsReadOnly();
    public Player? Winner { get; protected set; }
    public string Code {get; private set; }

    protected BaseLobby(IDataAccess dataAccess, Player creator, int maxPlayers, int requiredPlayers)
    {
        DataAccess = dataAccess;
        MaxPlayers = maxPlayers;
        RequiredPlayers = requiredPlayers;
        Status = LobbyState.Initialising;
        Code = GenerateCode();
        Add(creator);
        creator.IsOwner = true;
        Status = LobbyState.InLobby;
    }

    public abstract void StartGame();

    public void Add(Player player)
    {
        if (PlayerList.Count <= 0 && Status != LobbyState.Initialising)
        {
            throw new LobbyShouldNotExistException();
        } 
        if (PlayerList.Count >= MaxPlayers)
        {
            throw new LobbyFullException(MaxPlayers);
        }
        PlayerList.Add(player);
    }

    public void RemoveById(string id)
    {
        var player = PlayerList.FirstOrDefault(p => p.Id == id);
        if (player == null)
        {
            return;
        }
        PlayerList.Remove(player);
        if (player.IsOwner && PlayerList.Count >= 1)
        {
            PlayerList[0].IsOwner = true;
        }
    }

    private string GenerateCode()
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] codeArr = new char[CodeLength];
        var r = new Random();
        for (int i = 0; i < CodeLength; i++)
        {
            codeArr[i] = chars[r.Next(chars.Length)];
        }
        
        return new string(codeArr);
    }
}