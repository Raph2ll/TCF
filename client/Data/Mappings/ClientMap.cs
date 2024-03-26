using System;
using api.Data.Mappings.Interfaces;
using MySql.Data.MySqlClient;
 
namespace api.Data.Mappings
{
    public class ClientMap : IEntityMap
    {
        private string DatabaseName = "plataforma";

        public void Configure(MySqlConnection connection)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = $"USE {DatabaseName}";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS client (
                        id VARCHAR(40) PRIMARY KEY,
                        name VARCHAR(80) NOT NULL,
                        surname VARCHAR(80) NOT NULL,
                        email VARCHAR(80) NOT NULL,
                        birthdate DATE NOT NULL,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                        deleted BOOL DEFAULT FALSE
                    );";
                cmd.ExecuteNonQuery();
            }
        }
    }
}