using WordleClash.Core.Entities;

namespace WordleClash.Core.Interfaces;

public interface IUserRepository
{
   CreateUserResult Create(string sessionId, string? username = null);
   User GetByName(string name);
   User GetBySessionId(string sessionId);
   void UpdateName(string sessionId, string name);
   void DeleteById(int userId);
}