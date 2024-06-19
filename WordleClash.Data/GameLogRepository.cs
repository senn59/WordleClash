using MySql.Data.MySqlClient;
using WordleClash.Core;
using WordleClash.Core.Entities;
using WordleClash.Core.Enums;
using WordleClash.Core.Interfaces;

namespace WordleClash.Data;

public class GameLogRepository: IGameLogRepository
{
    private readonly string _connString;
    private const string GameLogTable = "game_log";
    private const int ResultsPerPage = 10;
    public GameLogRepository(string connString)
    {
        _connString = connString;
    }
    
    public List<GameLog> GetFromUserIdByPage(int userId, int pageSize, int page)
    {
        var logs = new List<GameLog>();
        try
        {
            using var conn = new MySqlConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT gl.*, w.entry FROM {GameLogTable} as gl" +
                              "INNER JOIN word AS w ON gl.word_id = w.id" +
                              "WHERE user_id=@userId " +
                              "ORDER BY id DESC" +
                              "LIMIT @pageSize OFFSET @page";
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
                    AttemptCount = reader.GetInt32("attempt_count"),
                    Date = reader.GetDateTime("timestamp"),
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
            cmd.CommandText = $"INSERT INTO {GameLogTable}" +
                              "(attempt_count, time, status, word_id, user_id)" +
                              "VALUES " +
                              "(@attemptCount, @time, @status, " +
                              "(SELECT id from word WHERE entry=@word)," +
                              "(SELECT id FROM user WHERE session_id=@sessionId))";
            cmd.Parameters.AddWithValue("@attemptCount", log.AttemptCount);
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
}