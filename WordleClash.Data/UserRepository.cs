using MySql.Data.MySqlClient;
using WordleClash.Core;
using WordleClash.Core.Exceptions;
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
    
    public CreateUserResult Create(string sessionId, string username)
    {
        try
        {
            using var conn = new MySqlConnection(_connString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"INSERT INTO {UserTable} (session_id, name) VALUES (@sessionId, @name)";
            cmd.Parameters.AddWithValue("@sessionId", sessionId);
            cmd.Parameters.AddWithValue("@name", username);
            conn.Open();
            cmd.ExecuteScalar();
            return new CreateUserResult
            {
                SessionId = sessionId,
                Username = username
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
        const string nameColumn = "name";
        try
        {
            using var conn = new MySqlConnection(_connString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * from {UserTable} WHERE {nameColumn}=@name LIMIT 1";
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
        throw new UserNotFoundException(nameColumn, name);
    }
    
    public User GetFromSessionId(string sessionId)
    {
        const string sessionColumn = "session_id";
        try
        {
            using var conn = new MySqlConnection(_connString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * from {UserTable} WHERE {sessionColumn}=@sessionId LIMIT 1";
            cmd.Parameters.AddWithValue("@sessionId", sessionId);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = rdr.GetInt32("id");
                var name = rdr.GetString("name");
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
            throw new UserRetrievalException(e);
        }
        throw new UserNotFoundException(sessionColumn, sessionId);
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