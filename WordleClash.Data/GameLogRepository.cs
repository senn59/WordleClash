using MySql.Data.MySqlClient;
using WordleClash.Core;
using WordleClash.Core.Entities;
using WordleClash.Core.Enums;
using WordleClash.Core.Interfaces;

namespace WordleClash.Data;

public class GameLogRepository: IGameLogRepository
{
    private readonly string _connString;
    public GameLogRepository(string connString)
    {
        _connString = connString;
    }
    
    public List<GameLog> GetByUserIdAndPage(int userId, int pageSize, int page)
    {
        var logs = new List<GameLog>();
        try
        {
            using var conn = new MySqlConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT gl.*, w.entry FROM game_log as gl " +
                              "INNER JOIN word AS w ON gl.word_id = w.id " +
                              "WHERE user_id=@userId AND gl.deleted_at IS NULL " +
                              "ORDER BY id DESC " +
                              "LIMIT @pageSize OFFSET @page";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@pageSize", pageSize);
            cmd.Parameters.AddWithValue("@page", pageSize * (page - 1));
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                TimeSpan? timeVal = null;
                if (!reader.IsDBNull(reader.GetOrdinal("time")))
                {
                    timeVal = reader.GetTimeSpan("time");
                }
                logs.Add(new GameLog
                {
                    Tries = reader.GetInt32("tries"),
                    Date = reader.GetDateTime("created_at"),
                    Status = (GameStatus)reader.GetByte("status"),
                    Time = timeVal,
                    Word = reader.GetString("entry")
                });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        return logs;
    }

    public void AddToUser(GameLog log, string sessionId)
    {
        try
        {
            using var conn = new MySqlConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO game_log" +
                              "(tries, time, status, word_id, user_id)" +
                              "VALUES " +
                              "(@tries, @time, @status, " +
                              "(SELECT id from word WHERE entry=@word)," +
                              "(SELECT id FROM user WHERE session_id=@sessionId))";
            cmd.Parameters.AddWithValue("@tries", log.Tries);
            cmd.Parameters.AddWithValue("@time", log.Time);
            cmd.Parameters.AddWithValue("@status", log.Status);
            cmd.Parameters.AddWithValue("@word", log.Word);
            cmd.Parameters.AddWithValue("@sessionId", sessionId);
            cmd.ExecuteScalar();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public void DeleteByUserId(int userId)
    {
        try
        {
            using var conn = new MySqlConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE game_log SET deleted_at=CURRENT_TIMESTAMP where user_id=@id";
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteScalar();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}