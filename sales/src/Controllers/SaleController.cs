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

        [HttpPost("items/{id}")]
        public async Task<IActionResult> AddItemsToSale(string id, List<SaleItemRequestDTO> saleItems)
        {
            try
            {
                await _saleService.AddItemsToSale(id, saleItems);
                return StatusCode(200, $"Ok, these items have been added for sale");
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
                return StatusCode(500, ex.Message);
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

        [HttpPut("confirm/{id}")]
        public async Task<IActionResult> ConfirmSale(string id)
        {
            try
            {
                await _saleService.ConfirmSale(id);
                return Ok($"Sale with ID '{id}' confirmed successfully.");
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{saleId}/items")]
        public async Task<IActionResult> UpdateSaleItems(string saleId, [FromBody] List<SaleItemRequestDTO> itemUpdates)
        {
            try
            {
                var saleItemUpdateRequests = itemUpdates.Select(dto => new SaleItemRequestDTO
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                }).ToList();

                await _saleService.UpdateItemsFromSale(saleId, saleItemUpdateRequests);
                return Ok("Sale items updated successfully.");
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("remove-items/{saleId}")]
        public async Task<IActionResult> RemoveItemsFromSale(string saleId, [FromBody] List<string> itemIds)
        {
            try
            {
                await _saleService.RemoveItemsFromSale(saleId, itemIds);
                return Ok("Items removed successfully.");
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
                return StatusCode(500, ex.Message);
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