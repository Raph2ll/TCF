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


        [HttpGet]
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

    }
}