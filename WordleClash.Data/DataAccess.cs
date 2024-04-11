using MySql.Data.MySqlClient;
using WordleClash.Core.DataAccess;

namespace WordleClash.Data;

public class DataAccess: IDataAccess
{
    private MySqlConnection _conn;

    public DataAccess(string connString)
    {
        _conn = new MySqlConnection(connString);
    }

    public List<string> GetWords()
    {
        var words = new List<string>();
        try
        {
            _conn.Open();

            var cmd = new MySqlCommand();
            cmd.Connection = _conn;
            cmd.CommandText = @"SELECT word FROM words";

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
        finally
        {
            _conn.Close();
        }

        return words;
    }

    public string GetRandomWord()
    {
        try
        {
            _conn.Open();

            var cmd = new MySqlCommand();
            cmd.Connection = _conn;
            cmd.CommandText = @"SELECT word FROM words ORDER BY RAND() LIMIT 1";

            var res = cmd.ExecuteScalar();
            return res.ToString() ?? throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            //TODO: throw custom exception with e as inner exception
            Console.WriteLine(e.ToString());
        }
        finally
        {
            _conn.Close();
        }

        throw new Exception("Could not find a word");
    }

    public string? GetWord(string word)
    {
        try
        {
            _conn.Open();

            var cmd = new MySqlCommand();
            cmd.Connection = _conn;
            cmd.CommandText = @"SELECT word FROM words WHERE UPPER(word) = UPPER(@word)";
            cmd.Parameters.AddWithValue("@word", word);

            var res = cmd.ExecuteScalar();
            return res?.ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        finally
        {
            _conn.Close();
        }

        return null;
    }
}