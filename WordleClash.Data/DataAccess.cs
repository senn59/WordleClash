using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using WordleClash.Core.DataAccess;

namespace WordleClash.Data;

public class DataAccess: IDataAccess
{
    private MySqlConnection _conn;

    public DataAccess(IConfiguration configuration)
    {
        _conn = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
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
}