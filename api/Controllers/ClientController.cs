using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using api.DTos;
using Microsoft.AspNetCore.Http;

namespace api.Controllers
{
    [ApiController]
    [Route("api/client")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateClient(ClientCreateDTO client)
        {
            try
            {
                _clientService.CreateClient(client);
                return StatusCode(201, client);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
          
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Client>> GetClients()
        {
            try
            {
                var clients = _clientService.GetClients();
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateClient(string id, ClientUpdateDTO updatedClient)
        {
            
                if (_clientService.GetClientById(id) == null)
                {
                    return NotFound("Client Not Found");
                }
                _clientService.UpdateClient(id, updatedClient);

                return Ok(updatedClient);
            
        }
    }
}