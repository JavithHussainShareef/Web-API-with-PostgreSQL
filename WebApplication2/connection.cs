using Npgsql;

namespace WebApplication2
{
    public class connection
    {
        static void Main(string[] args)
        {
            TestConnection();
            Console.ReadKey();
        }
        private static void TestConnection()
        {
            using(NpgsqlConnection con = GetConnection())
            {
                con.Open();
                if(con.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connected");
                }
            }
        }
        private static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=localhost;Database=API;Port=5432;User Id=postgres;Password=javith");
        }
    }
}
