using api.Models;
using api.Mappings;
using api.Mappings.Interfaces;

using MySql.Data.MySqlClient;

namespace api.Data
{
    public class DataContext
    {
        private readonly string _connectionString;
        private readonly IEnumerable<IEntityMap> _entityMaps;
        string databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");

        public DataContext(string connectionString, IEnumerable<IEntityMap> entityMaps)
        {
            _connectionString = connectionString;
            _entityMaps = entityMaps;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public void OnModelCreating()
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS {databaseName}";
                    cmd.ExecuteNonQuery();
                }

                foreach (var entityMap in _entityMaps)
                {
                    entityMap.Configure(connection);
                }
            }
        }

    }
}
