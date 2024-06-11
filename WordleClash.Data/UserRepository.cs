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
    
    public void Create(string sessionId)
    {
        try
        {
            using var conn = new MySqlConnection(_connString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"INSERT INTO {UserTable} (session_id) VALUES (@sessionId)";
            cmd.Parameters.AddWithValue("@sessionId", sessionId);
            conn.Open();
            cmd.ExecuteScalar();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public User GetBySessionId(string sessionId)
    {
        try
        {
            using var conn = new MySqlConnection(_connString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * from {UserTable} WHERE session_id=@sessionId LIMIT 1";
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
            Console.WriteLine(e.ToString());
        }
        throw new Exception("User not found");
    }

    public void DeleteBySessionId(string sessionId)
    {
        throw new NotImplementedException();
    }
}