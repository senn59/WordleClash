using MySql.Data.MySqlClient;
using WordleClash.Core.Exceptions;
using WordleClash.Core.Interfaces;
using Exception = System.Exception;

namespace WordleClash.Data;

public class WordRepository: IWordRepository
{
    private readonly string _connString;
    private const string WordTable = "word";
        
    public WordRepository(string connString)
    {
        _connString = connString;
    }

    public List<string> GetAll()
    {
        var words = new List<string>();
        try
        {
            using var conn = new MySqlConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT entry FROM {WordTable}";
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                words.Add(rdr[0].ToString() ?? throw new InvalidOperationException());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        return words;
    }

    public string GetRandom()
    {
        try
        {
            
            using var conn = new MySqlConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT entry FROM {WordTable} ORDER BY RAND() LIMIT 1";
            var res = cmd.ExecuteScalar();
            return res?.ToString() ?? throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        throw new CouldNotFindWordException();
    }

    public string? Get(string word)
    {
        try
        {
            using var conn = new MySqlConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT entry FROM {WordTable} WHERE UPPER(entry) = UPPER(@word)";
            cmd.Parameters.AddWithValue("@word", word);
            var res = cmd.ExecuteScalar();
            return res?.ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        return null;
    }

    public int? GetId(string word)
    {
        try
        {
            using var conn = new MySqlConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT id FROM {WordTable} WHERE UPPER(entry) = UPPER(@word)";
            cmd.Parameters.AddWithValue("@word", word);
            var res = cmd.ExecuteScalar();
            return int.Parse(res!.ToString()!);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        return null;
    }
}