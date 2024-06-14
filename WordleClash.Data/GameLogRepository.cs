using MySql.Data.MySqlClient;
using WordleClash.Core;
using WordleClash.Core.Interfaces;

namespace WordleClash.Data;

public class GameLogRepository: IGameLogRepository
{
    private readonly string _connString;
    private const string GameLogTable = "game_log";
    public GameLogRepository(string connString)
    {
        _connString = connString;
    }
    
    public List<GameLog> GetFromUserId(int userId)
    {
        throw new NotImplementedException();
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