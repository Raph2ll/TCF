using System;
using product.Data.Mappings.Interfaces;
using MySql.Data.MySqlClient;
 
namespace product.Data.Mappings
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
                        dest VARCHAR(250) NOT NULL,
                        quantity INT NOT NULL,
                        price DECIMAL(10, 2) NOT NULL,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                        deleted BOOL DEFAULT FALSE
                    );";
                cmd.ExecuteNonQuery();
            }
        }
    }
}