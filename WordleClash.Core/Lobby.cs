using WordleClash.Core.Exceptions;

namespace WordleClash.Core;

public class Lobby
{
    private const int CodeLength = 6;
    private readonly List<Player> _players = [];
    
    public int MaxPlayers { get; private init; }

    public string Code {get; private init; }
    public IReadOnlyList<Player> Players => _players.AsReadOnly();

    public Lobby(string creator, int maxPlayers)
    {
        MaxPlayers = maxPlayers;
        Code = GenerateCode();
        
        Add(creator);
        _players[0].SetOwner();
    }

    public Player Add(string name)
    {
        if (_players.Count >= MaxPlayers)
        {
            throw new LobbyFullException(MaxPlayers);
        }

        var player = new Player { Name = name };
        _players.Add(player);
        return player;
    }

    public void RemoveById(string id)
    {
        var player = _players.FirstOrDefault(p => p.Id == id);
        if (player == null)
        {
            return;
        }
        _players.Remove(player);
        if (player.IsOwner && _players.Count >= 1)
        {
            _players[0].SetOwner();
        }
    }

    private string GenerateCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var codeArr = new char[CodeLength];
        var r = new Random();
        for (var i = 0; i < CodeLength; i++)
        {
            codeArr[i] = chars[r.Next(chars.Length)];
        }
        
        return new string(codeArr);
    }
}