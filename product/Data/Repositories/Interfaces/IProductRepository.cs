using product.Models;

namespace product.Data.Repositories.Interfaces;

public interface IProductRepository
{
    public void CreateProduct(Product product);
    public List<Product> GetProducts();
    public Product GetProductById(string id);
    public void UpdateProduct(Product updatedProduct);
    public void DeleteProduct(string id);
}