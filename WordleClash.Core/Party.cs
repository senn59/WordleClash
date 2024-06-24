using WordleClash.Core.Entities;
using WordleClash.Core.Interfaces;

namespace WordleClash.Core;

public class Party: IMultiplayerGame
{
    public int MaxPlayers => 4;
    public int RequiredPlayers => 2;
    public int MaxTries => 6;
    public IReadOnlyList<Player> Players { get; } = new List<Player>();
    private IWordRepository _wordRepository;
    private string _word;

    public Party(IWordRepository wordRepository)
    {
        _wordRepository = wordRepository;
        _word = _wordRepository.GetRandom();
    }
    
    public void Start()
    {
        foreach (var player in Players)
        {
            player.SetGame(new Game(_wordRepository, MaxTries, _word));
        }

        throw new NotImplementedException();
    }

    public GuessResult HandleGuess(Player player, string guess)
    {
        throw new NotImplementedException();
    }

    public void SetPlayers(IReadOnlyList<Player> players)
    {
        throw new NotImplementedException();
    }

    public List<GameModel> GetGames()
    {
        throw new NotImplementedException();
    }

    public void Restart()
    {
        throw new NotImplementedException();
    }
}