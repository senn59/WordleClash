namespace WordleClash.Core.Interfaces;

public interface IGameLogDao
{
   List<GameLog> GetFromUserId(int userId);
}