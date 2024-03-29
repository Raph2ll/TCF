using product.Models;
using product.Data.Repositories.Interfaces;
using MySql.Data.MySqlClient;

namespace product.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _connection;
        private string _tableName = "product";
        
        public ProductRepository(DataContext connection)
        {
            _connection = connection;
        }

        public void CreateProduct(Product product)
        {
            using (var dbConnection = _connection.GetConnection())
            {
                dbConnection.Open();
                using (var cmd = new MySqlCommand(@$"INSERT INTO {_tableName} (id, name, dest, quantity, price) 
                        VALUES (@Id, @Name, @Dest, @Quantity, @Price)",
                           dbConnection))
                {
                    cmd.Parameters.AddWithValue("@Id", product.Id);
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@Dest", product.Description);
                    cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                    cmd.Parameters.AddWithValue("@Price", product.Price);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Product> GetProducts()
        {
            var products = new List<Product>();

            using (var dbConnection = _connection.GetConnection())
            {
                dbConnection.Open();
                using (var command = new MySqlCommand($@"SELECT id, name, dest, quantity, price, created_at, updated_at, deleted FROM {_tableName}",
                           dbConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = new Product
                            {
                                Id = reader["id"].ToString(),
                                Name = reader["name"].ToString(),
                                Description = reader["dest"].ToString(),
                                Quantity = Convert.ToInt32(reader["quantity"]),
                                Price = Convert.ToDecimal(reader["price"]),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                                Deleted = Convert.ToBoolean(reader["deleted"])
                            };

                            products.Add(product);
                        }
                    }
                }
            }

            return products;
        }

        public Product GetProductById(string id)
        {
            using (var dbConnection = _connection.GetConnection())
            {
                dbConnection.Open();
                using (var command = new MySqlCommand($"SELECT id, name, dest, quantity, price, created_at, updated_at, deleted FROM {_tableName} WHERE id = @Id",
                           dbConnection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Product
                            {
                                Id = reader["id"].ToString(),
                                Name = reader["name"].ToString(),
                                Description = reader["dest"].ToString(),
                                Quantity = Convert.ToInt32(reader["quantity"]),
                                Price = Convert.ToDecimal(reader["price"]),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                                Deleted = Convert.ToBoolean(reader["deleted"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void UpdateProduct(Product updatedProduct)
        {
            using (var dbConnection = _connection.GetConnection())
            {
                dbConnection.Open();
                using (var cmd = new MySqlCommand($"UPDATE {_tableName} SET name = @Name, dest = @Dest, quantity = @Quantity, price = @Price WHERE id = @Id AND deleted = false",
                    dbConnection))
                {
                    cmd.Parameters.AddWithValue("@Id", updatedProduct.Id);
                    cmd.Parameters.AddWithValue("@Name", updatedProduct.Name);
                    cmd.Parameters.AddWithValue("@Dest", updatedProduct.Description);
                    cmd.Parameters.AddWithValue("@Quantity", updatedProduct.Quantity);
                    cmd.Parameters.AddWithValue("@Price", updatedProduct.Price);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteProduct(string id)
        {
            using (var dbConnection = _connection.GetConnection())
            {
                dbConnection.Open();
                using (var command = new MySqlCommand($"UPDATE {_tableName} SET deleted = true WHERE id = @Id AND deleted = false",
                    dbConnection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
