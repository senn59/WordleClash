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

    public string GetRow(int id)
    {
        try
        {
            _conn.Open();

            var cmd = new MySqlCommand();
            cmd.Connection = _conn;
            cmd.CommandText = @"SELECT name FROM testing WHERE id = @id;";
            cmd.Parameters.AddWithValue("@id", id);

            var res = cmd.ExecuteScalar();
            if (res != null)
            {
                Console.WriteLine(res);
                return res.ToString();
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

        return "";
    }
}