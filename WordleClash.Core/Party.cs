using WordleClash.Core.Entities;
using WordleClash.Core.Enums;
using WordleClash.Core.Exceptions;
using WordleClash.Core.Interfaces;

namespace WordleClash.Core;

public class Party: IMultiplayerGame
{
    public int MaxPlayers => 4;
    public int RequiredPlayers => 2;
    public int MaxTries => 6;
    public IReadOnlyList<Player> Players { get; private set; } = new List<Player>();
    private readonly IWordRepository _wordRepository;
    private string _word;

    public Party(IWordRepository wordRepository)
    {
        _wordRepository = wordRepository;
    }
    
    public void Start()
    {
        if (Players.Any(p => p.Game == null))
        {
            SetPlayerGames();
        }
        foreach (var player in Players)
        {
            player.Game!.Start();
        }
    }

    public GuessResult HandleGuess(Player player, string guess)
    {
        return player.Game!.TakeGuess(guess);
    }

    public void SetPlayers(IReadOnlyList<Player> players)
    {
        if (IsInProgress())
        {
            ValidatePlayers();
        }

        Players = players;
    }

    public List<GameModel> GetGames()
    {
        return Players.Select(p => GameModel.FromGame(p.Game!)).ToList();
    }

    public void Restart()
    {
        SetPlayerGames();
    }

    private bool IsInProgress()
    {
        return Players.Any(p => p.Game?.Status == GameStatus.InProgress);
    }
    
    private void ValidatePlayers()
    {
        if (Players.Count < RequiredPlayers)
        {
            throw new TooFewPlayersException();
        }

        if (Players.Count > MaxPlayers)
        {
            throw new TooManyPlayersException();
        }
    }

    private void SetPlayerGames()
    {
        _word = _wordRepository.GetRandom();
        foreach (var player in Players)
        {
            player.SetGame(new Game(_wordRepository, MaxTries, _word));
        }
    }
}