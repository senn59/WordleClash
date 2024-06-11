using MySql.Data.MySqlClient;
using WordleClash.Core;
using WordleClash.Core.Interfaces;
using Exception = System.Exception;

namespace WordleClash.Data;

public class UserRepository: IUserRepository
{
    private readonly string _connString;
    private const string UserTable = "user";

    public UserRepository(string connString)
    {
        _connString = connString;
    }
    
    public CreateUserResult Create(string sessionId)
    {
        var userName = GenerateName(sessionId);
        try
        {
            using var conn = new MySqlConnection(_connString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"INSERT INTO {UserTable} (session_id, name) VALUES (@sessionId, @name)";
            cmd.Parameters.AddWithValue("@sessionId", sessionId);
            cmd.Parameters.AddWithValue("@name", userName);
            conn.Open();
            cmd.ExecuteScalar();
            return new CreateUserResult
            {
                SessionId = sessionId,
                UserName = userName
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        throw new Exception("Failed to create user");
    }

    public User GetByName(string name)
    {
        try
        {
            using var conn = new MySqlConnection(_connString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * from {UserTable} WHERE name=@name LIMIT 1";
            cmd.Parameters.AddWithValue("@name", name);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = rdr.GetInt32("id");
                var sessionId = rdr.GetString("session_id");
                var creationDate = rdr.GetDateTime("created_at");
                return new User
                {
                    Id = id,
                    Name = name,
                    SessionId = sessionId,
                    CreatedAt = creationDate
                };
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        throw new Exception("User not found");
    }

    public void DeleteBySessionId(string sessionId)
    {
        throw new NotImplementedException();
    }

    private string GenerateName(string id)
    {
        var shortenedId = id.Substring(Math.Max(0, id.Length - 10));
        return $"user-{shortenedId}";
    }
}