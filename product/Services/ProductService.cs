using product.Exceptions;
using product.Data.Repositories.Interfaces;
using product.Models;
using product.Models.DTOs;
using product.Services.Interfaces;

namespace product.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void CreateProduct(ProductCreateDTO newProduct)
        {
            var product = new Product()
            {
                Id = Guid.NewGuid().ToString(),
                Name = newProduct.Name,
                Description = newProduct.Description,
                Quantity = newProduct.Quantity,
                Price = newProduct.Price
            };

            _productRepository.CreateProduct(product);
        }

        public List<Product> GetProducts()
        {
            return _productRepository.GetProducts();
        }

        public Product GetProductById(string id)
        {
            return _productRepository.GetProductById(id);
        }

        public void UpdateProduct(string id, ProductUpdateDTO updatedProductDto)
        {
            var existingProduct = GetProductById(id);

            if (existingProduct == null)
            {
                throw new NotFoundException("Product Not Found");
            }

            string updatedName = existingProduct.Name;
            if (!string.IsNullOrEmpty(updatedProductDto.Name))
            {
                updatedName = updatedProductDto.Name;
            }

            string updatedDescription = existingProduct.Description;
            if (!string.IsNullOrEmpty(updatedProductDto.Description))
            {
                updatedDescription = updatedProductDto.Description;
            }

            int updatedQuantity = existingProduct.Quantity;
            if (updatedProductDto.Quantity > 0)
            {
                updatedQuantity = updatedProductDto.Quantity;
            }

            decimal updatedPrice = existingProduct.Price;
            if (updatedProductDto.Price > 0)
            {
                updatedPrice = updatedProductDto.Price;
            }

            var updatedProduct = new Product
            {
                Id = existingProduct.Id,
                Name = updatedName,
                Description = updatedDescription,
                Quantity = updatedQuantity,
                Price = updatedPrice
            };

            _productRepository.UpdateProduct(updatedProduct);
        }
        public void DeleteProduct(string id)
        {
            if (GetProductById(id) == null)
            {
                throw new NotFoundException("Client Not Found");
            }

            _productRepository.DeleteProduct(id);
        }
    }
}