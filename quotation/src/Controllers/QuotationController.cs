using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using quotation.src.Models;
using quotation.src.Services.Interfaces;

namespace quotation.src.Controllers
{
    [Route("api")]
    public class QuotationController : Controller
    {
        private readonly IQuotationService _quotationService;

        public QuotationController(IQuotationService quotationService)
        {
            _quotationService = quotationService;
        }

        /// <summary>
        /// Get All.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Return all quotation</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(Quotation), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Quotation>> GetAll()
        {
            try
            {
                var res = await _quotationService.GetCurrencyInfo();
                return StatusCode(201, res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get quotation by name.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">Quotation by name</param>
        /// <returns></returns>
        /// <response code="200">Return a single quotation</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetByName(string id)
        {
            try
            {
                var res = await _quotationService.GetCurrencyByName(id);
                return StatusCode(201, res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
} 