using MySqlConnector;


namespace EspnBackend.Database
{
    public class DatabaseSeeder
    {
        private readonly string _connectionString;

        public DatabaseSeeder(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Seed()
        {

        }

    }
}
