using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using product.Models;
using product.Services.Interfaces;
using System.Linq;
using product.Models.DTOs;
using Microsoft.AspNetCore.Http;
using product.Exceptions;
using System.ComponentModel.DataAnnotations;


namespace product.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService ProductService)
        {
            _productService = ProductService;
        }

        /// <summary>
        /// Create a product.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="Product">Product's data</param>
        /// <returns></returns>
        /// <response code="201">Success in creating the product</response>
        /// <response code="400">Malformed request</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateProduct(ProductCreateDTO Product)
        {
            try
            {
                _productService.CreateProduct(Product);
                return StatusCode(201, Product);
            }
            catch (ValidationException ex)
            {
                return StatusCode(400, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        /// <summary>
        /// Get a products.
        /// </summary>
        /// <remarks>
        /// </remarks> 
        /// <returns></returns>
        /// <response code="201">Success in creating the product</response>
        /// <response code="500">Internal server error</response>
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

        /// <summary>
        /// Edit product by id.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">Product id</param>
        /// <returns></returns>
        /// <response code="200">Return new product</response>
        /// <response code="404">Product not found</response>
        /// <response code="500">Internal server error</response>
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

        /// <summary>
        /// Soft delete product by id.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">Product id</param>
        /// <returns></returns>
        /// <response code="200">The product has been deleted</response>
        /// <response code="404">Product not found</response>
        /// <response code="500">Internal server error</response>
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