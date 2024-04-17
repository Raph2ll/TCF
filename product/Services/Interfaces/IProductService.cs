using product.Exceptions;
using product.Db.Repositories.Interfaces;
using product.Models;
using product.Models.DTOs;
namespace product.Services.Interfaces;

public interface IProductService
{
    public void CreateProduct(ProductCreateDTO newProduct);
    public List<Product> GetProducts();
    public Product GetProductById(string id);
    public void UpdateProduct(string id, ProductUpdateDTO updatedProductDto);
    public void DeleteProduct(string id);
}