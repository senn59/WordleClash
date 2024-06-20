using MySql.Data.MySqlClient;
using WordleClash.Core;
using WordleClash.Core.Entities;
using WordleClash.Core.Exceptions;
using WordleClash.Core.Interfaces;
using Exception = System.Exception;

namespace WordleClash.Data;

public class UserRepository: IUserRepository
{
    private readonly string _connString;
    private const int DuplicateEntryCode = 1062;

    public UserRepository(string connString)
    {
        _connString = connString;
    }
    
    public CreateUserResult Create(string sessionId, string? username = null)
    {
        try
        {
            using var conn = new MySqlConnection(_connString);
            conn.Open();
            using var transaction = conn.BeginTransaction();
            using var cmd = conn.CreateCommand();
            cmd.Transaction = transaction;
            if (username == null)
            {
                cmd.CommandText = "SELECT GROUP_CONCAT(entry ORDER BY RAND() SEPARATOR '') AS usrname " +
                                  "FROM (SELECT entry FROM word ORDER BY RAND() limit 2) AS rndwords";
                username = cmd.ExecuteScalar()?.ToString() ?? "default";
            }
            
            cmd.CommandText = "INSERT INTO user (session_id, name) VALUES (@sessionId, @name);";
            cmd.Parameters.AddWithValue("@sessionId", sessionId);
            cmd.Parameters.AddWithValue("@name", username);
            cmd.ExecuteScalar();
            transaction.Commit();
            
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
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * from user WHERE {nameColumn}=@name LIMIT 1";
            cmd.Parameters.AddWithValue("@name", name);
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                return new User
                {
                    Id = rdr.GetInt32("id"),
                    Name = rdr.GetString("name"),
                    SessionId = rdr.GetString("session_id"),
                    CreatedAt = rdr.GetDateTime("created_at")
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
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * from user WHERE {sessionColumn}=@sessionId LIMIT 1";
            cmd.Parameters.AddWithValue("@sessionId", sessionId);
            using var rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                if (!rdr.IsDBNull(rdr.GetOrdinal("deleted_at")))
                {
                    throw new UserDeletedException();
                }

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
        catch (UserDeletedException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new UserRetrievalException(e);
        }
        throw new UserNotFoundException(sessionColumn, sessionId);
    }

    public void ChangeName(string sessionId, string name)
    {
        try
        {
            using var conn = new MySqlConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE user SET name=@name WHERE session_id=@sessionId";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@sessionId", sessionId);
            cmd.ExecuteScalar();
        }
        catch (MySqlException e)
        {
            if (e.Number == DuplicateEntryCode)
            {
                throw new UsernameTakenException(name);
            }

            throw;
        }
    }

    public void DeleteById(int userId)
    {
        try
        {
            using var conn = new MySqlConnection(_connString);
            conn.Open();
            using var transaction = conn.BeginTransaction();
            using var cmd = conn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = "UPDATE user SET deleted_at=CURRENT_TIMESTAMP WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteScalar();
            cmd.CommandText = "UPDATE game_log SET deleted_at=CURRENT_TIMESTAMP where id=@id";
            cmd.ExecuteScalar();
            transaction.Commit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}