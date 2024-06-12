namespace WordleClash.Core.Interfaces;

public interface IUserRepository
{
   CreateUserResult Create(string sessionId);
   User GetByName(string name);
   User GetFromSessionId(string sessionId);
   void DeleteBySessionId(string sessionId);
}