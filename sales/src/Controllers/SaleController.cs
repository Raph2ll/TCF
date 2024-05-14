using Microsoft.AspNetCore.Mvc;
using sales.src.Models;
using sales.src.Models.DTOs;
using sales.src.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sales.src.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale(SaleRequestDTO sale)
        {
            try
            {
                await _saleService.CreateSale(sale);
                return Ok();
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
