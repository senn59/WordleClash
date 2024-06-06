namespace WordleClash.Core.Interfaces;

public interface IGameLogRepository
{
   List<GameLog> GetFromUserId(int userId);

   void AddNewGamelog(GameLog log, string sessionId);
}