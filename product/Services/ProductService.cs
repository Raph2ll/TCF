using product.Exceptions;
using product.Db.Repositories.Interfaces;
using product.Models;
using product.Models.DTOs;
using product.Services.Interfaces;
using Serilog;
using System.Diagnostics;
using System.Reflection;
using product.Utils;
using product.Services.Refit;
using Newtonsoft.Json;
using System.Globalization;

namespace product.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IQuotation _quotation;
        private readonly Serilog.ILogger _logger;
        private readonly ContextFactory _ctxFactory;
        private readonly string _namespace = "Service";


        public ProductService(IQuotation quotation, IProductRepository productRepository)
        {
            _quotation = quotation;
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

public async Task<List<ProductResponseDTO>> GetProducts()
{
    var methodName = $"{_namespace} {MethodBase.GetCurrentMethod()!.Name}";

    using (var ctx = _ctxFactory.Create(methodName))
    {
        var products = new List<Product>();
        products = _productRepository.GetProducts();

        var currencyInfo = await _quotation.GetCurrencyInfo();
        JsonConvert.SerializeObject(currencyInfo);

        var newProducts = new List<ProductResponseDTO>();

        foreach (var product in products)
        {
            var newProduct = new ProductResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                BRL = product.Price,
                EUR = Convert.ToDecimal(product.Price / currencyInfo.EUR, CultureInfo.InvariantCulture),
                USD = Convert.ToDecimal(product.Price / currencyInfo.USD, CultureInfo.InvariantCulture),
                GBP = Convert.ToDecimal(product.Price / currencyInfo.GBP, CultureInfo.InvariantCulture),
                CNY = Convert.ToDecimal(product.Price / currencyInfo.CNY, CultureInfo.InvariantCulture),
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,   
                Deleted = product.Deleted
            };

            newProducts.Add(newProduct);
        }
        return newProducts;
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