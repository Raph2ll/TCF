using product.Models;
using Serilog;
using System;
using System.Diagnostics;
using System.Reflection;
using product.Db.Repositories.Interfaces;
using MySql.Data.MySqlClient;
using product.Utils;

namespace product.Db.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbContext _dbContext;

        private string _tableName = "products";
        private readonly Serilog.ILogger _logger;
        private readonly ContextFactory _ctxFactory;
        private readonly string _namespace = "Repository";

        public ProductRepository(DbContext connection)
        {
            _dbContext = connection;
            _logger = Serilog.Log.ForContext<ProductRepository>();
            _ctxFactory = new ContextFactory(_logger);
        }

        public void CreateProduct(Product product)
        {
            var methodName = $"{_namespace} {MethodBase.GetCurrentMethod()!.Name}";

            using (var ctx = _ctxFactory.Create(methodName))
            {
                using (var cmd = new MySqlCommand(@$"INSERT INTO {_tableName} (id, name, dest, quantity, price) 
                        VALUES (@Id, @Name, @Dest, @Quantity, @Price)",
                           _dbContext.Connection))
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
            var methodName = $"{_namespace} {MethodBase.GetCurrentMethod()!.Name}";

            using (var ctx = _ctxFactory.Create(methodName))
            {
                using (var command = new MySqlCommand($@"SELECT id, name, dest, quantity, price, created_at, updated_at, deleted FROM {_tableName}",
                           _dbContext.Connection))
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

                return products;
            }
        }

        public Product GetProductById(string id)
        {
            var methodName = $"{_namespace} {MethodBase.GetCurrentMethod()!.Name}";

            using (var ctx = _ctxFactory.Create(methodName))
            {
                using (var command = new MySqlCommand($"SELECT id, name, dest, quantity, price, created_at, updated_at, deleted FROM {_tableName} WHERE id = @Id",
                           _dbContext.Connection))
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

                return null;
            }
        }

        public void UpdateProduct(Product updatedProduct)
        {
            var methodName = $"{_namespace} {MethodBase.GetCurrentMethod()!.Name}";

            using (var ctx = _ctxFactory.Create(methodName))
            {
                using (var cmd = new MySqlCommand($"UPDATE {_tableName} SET name = @Name, dest = @Dest, quantity = @Quantity, price = @Price WHERE id = @Id AND deleted = false",
                    _dbContext.Connection))
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
            var methodName = $"{_namespace} {MethodBase.GetCurrentMethod()!.Name}";

            using (var ctx = _ctxFactory.Create(methodName))
            {
                using (var command = new MySqlCommand($"UPDATE {_tableName} SET deleted = true WHERE id = @Id AND deleted = false",
                    _dbContext.Connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}