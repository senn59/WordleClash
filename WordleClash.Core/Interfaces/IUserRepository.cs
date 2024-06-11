namespace WordleClash.Core.Interfaces;

public interface IUserRepository
{
   CreateUserResult Create(string sessionId);
   User GetBySessionId(string sessionId);
   void DeleteBySessionId(string sessionId);
}