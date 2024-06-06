using MySql.Data.MySqlClient;
using WordleClash.Core;
using WordleClash.Core.Interfaces;

namespace WordleClash.Data;

public class UserRepository: IUserRepository
{
    private readonly string _connString;

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
            cmd.CommandText = @"INSERT INTO user (session_id) VALUES (@sessionId)";
            cmd.Parameters.AddWithValue("@sessionId", sessionId);
            cmd.ExecuteScalar();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public User GetUserBySessionId(string sessionId)
    {
        try
        {
            using var conn = new MySqlConnection(_connString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * from user WHERE session_id=@sessionId LIMIT 1";
            cmd.Parameters.AddWithValue("@sessionId", sessionId);
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
}