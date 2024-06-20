using WordleClash.Core.Entities;

namespace WordleClash.Core.Interfaces;

public interface IUserRepository
{
   CreateUserResult Create(string sessionId, string? username = null);
   User GetByName(string name);
   User GetFromSessionId(string sessionId);
   void ChangeName(string sessionId, string name);
   void DeleteById(int userId);
}