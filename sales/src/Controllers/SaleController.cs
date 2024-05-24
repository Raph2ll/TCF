using Microsoft.AspNetCore.Mvc;
using sales.src.Models;
using sales.src.Models.DTOs;
using sales.src.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using sales.src.Exceptions;
using MongoDB.Bson;

namespace sales.src.Controllers
{
    [ApiController]
    [Route("api")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> CreateSale(string id)
        {
            try
            {
                await _saleService.CreateSale(id);
                return StatusCode(201, $"Ok a sale for the client: {id} was created");
            }
            catch (NotFoundException ex)
            {
                return NotFound($"{ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpPost("item/{id}")]
        public async Task<IActionResult> AddItemsToSale(string id, List<SaleItemRequestDTO> saleItems)
        {
            try
            {
                await _saleService.AddItemsToSale(id, saleItems);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaleById(string id)
        {
            try
            {
                var sale = await _saleService.GetSaleById(id);
                if (sale == null)
                    return NotFound();

                return Ok(sale);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSales()
        {
            try
            {
                var sales = await _saleService.GetAllSales();
                return Ok(sales);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSale(string id, Sale updatedSale)
        {
            try
            {
                await _saleService.UpdateSale(id, updatedSale);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(string id)
        {
            try
            {
                await _saleService.DeleteSale(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
