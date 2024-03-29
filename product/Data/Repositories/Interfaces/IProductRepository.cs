using product.Models;

namespace product.Data.Repositories.Interfaces;

public interface IProductRepository
{
    public void CreateProduct(Product product);
}