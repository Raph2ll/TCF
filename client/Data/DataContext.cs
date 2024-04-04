using client.Models;
using client.Data.Mappings;
using client.Data.Mappings.Interfaces;

using MySql.Data.MySqlClient;

namespace client.Data
{
    public class DataContext
    {
        private readonly string _connectionString;
        private readonly IEnumerable<IEntityMap> _entityMaps;
        private string DatabaseName = "client";

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
                    cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS {DatabaseName}";
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
