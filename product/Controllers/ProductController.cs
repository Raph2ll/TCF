using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using product.Models;
using product.Services.Interfaces;
using System.Linq;
using product.Models.DTOs;
using Microsoft.AspNetCore.Http;
using product.Exceptions;

namespace product.Controllers
{
    [ApiController]
    [Route("api/Product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService ProductService)
        {
            _productService = ProductService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateProduct(ProductCreateDTO Product)
        {
            try
            {
                _productService.CreateProduct(Product);
                return StatusCode(201, Product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Product>> GetProducts()
        {
            try
            {
                var Products = _productService.GetProducts();
                return Ok(Products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateProduct(string id, ProductUpdateDTO updatedProduct)
        {
            try
            {
                _productService.UpdateProduct(id, updatedProduct);

                return Ok(updatedProduct);
            }
            catch (NotFoundException ex)
            {
                return NotFound($"Product not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteProduct(string id)
        {
            try
            {
                _productService.DeleteProduct(id);

                return Ok("The Product has been deleted");
            }
            catch (NotFoundException ex)
            {
                return NotFound($"Product not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}