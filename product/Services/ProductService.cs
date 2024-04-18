using product.Exceptions;
using product.Db.Repositories.Interfaces;
using product.Models;
using product.Models.DTOs;
using product.Services.Interfaces;
using Serilog;
using System.Diagnostics;
using System.Reflection;
using product.Utils;


namespace product.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly Serilog.ILogger _logger;
        private readonly ContextFactory _ctxFactory;
        private readonly string _namespace = "Service";


        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _logger = Serilog.Log.ForContext<ProductService>();
            _ctxFactory = new ContextFactory(_logger);
        }

        public Product CreateProduct(ProductCreateDTO newProduct)
        {
            var methodName = $"{_namespace} {MethodBase.GetCurrentMethod()!.Name}";

            using (var ctx = _ctxFactory.Create(methodName))
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

                Product res = GetProductById(product.Id);
                return res;
            }
        }

        public List<Product> GetProducts()
        {
            var methodName = $"{_namespace} {MethodBase.GetCurrentMethod()!.Name}";

            using (var ctx = _ctxFactory.Create(methodName))
            {
                return _productRepository.GetProducts();
            }
        }
        public Product GetProductById(string id)
        {
            var methodName = $"{_namespace} {MethodBase.GetCurrentMethod()!.Name}";

            using (var ctx = _ctxFactory.Create(methodName))
            {
                return _productRepository.GetProductById(id);
            }
        }

        public void UpdateProduct(string id, ProductUpdateDTO updatedProductDto)
        {
            var methodName = $"{_namespace} {MethodBase.GetCurrentMethod()!.Name}";

            using (var ctx = _ctxFactory.Create(methodName))
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
        }
        public void DeleteProduct(string id)
        {
            var methodName = $"{_namespace} {MethodBase.GetCurrentMethod()!.Name}";

            using (var ctx = _ctxFactory.Create(methodName))
            {
                if (GetProductById(id) == null)
                {
                    throw new NotFoundException("Client Not Found");
                }

                _productRepository.DeleteProduct(id);
            }
        }
    }
}