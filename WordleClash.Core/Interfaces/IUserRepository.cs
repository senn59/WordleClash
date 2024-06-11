namespace WordleClash.Core.Interfaces;

public interface IUserRepository
{
   CreateUserResult Create(string sessionId);
   User GetByName(string name);
   void DeleteBySessionId(string sessionId);
}