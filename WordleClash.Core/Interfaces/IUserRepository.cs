namespace WordleClash.Core.Interfaces;

public interface IUserRepository
{
   void Create(string sessionId);
   User GetUserBySessionId(string sessionId);
}