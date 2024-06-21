using WordleClash.Core.Entities;

namespace WordleClash.Core.Interfaces;

public interface IGameLogRepository
{
   List<GameLog> GetByUserIdAndPage(int userId, int pageSize, int page);
   void AddToUser(GameLog log, string sessionId);
   void DeleteByUserId(int userId);
}