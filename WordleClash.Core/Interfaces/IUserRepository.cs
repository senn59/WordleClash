namespace WordleClash.Core.Interfaces;

public interface IUserRepository
{
   void Create(string sessionId);
   User GetBySessionId(string sessionId);
   void DeleteBySessionId(string sessionId);
}