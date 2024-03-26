using System;
using api.Data.Mappings.Interfaces;
using MySql.Data.MySqlClient;
 
namespace api.Data.Mappings
{
    public class ProductMap : IEntityMap
    {
        private string DatabaseName = "products";

        public void Configure(MySqlConnection connection)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = $"USE {DatabaseName}";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS product (
                        id VARCHAR(40) PRIMARY KEY,
                        name VARCHAR(80) NOT NULL,
                        dest VARCHAR(80) NOT NULL,
                        quantity VARCHAR(80) NOT NULL,
                        Price DECIMAL(10, 2),
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                        deleted BOOL DEFAULT FALSE
                    );";
                cmd.ExecuteNonQuery();
            }
        }
    }
}