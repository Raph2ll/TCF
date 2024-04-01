using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Services.Interfaces;
using System.Linq;
using api.Models.DTOs;
using Microsoft.AspNetCore.Http;
using client.Exceptions;

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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateClient(string id, ClientUpdateDTO updatedClient)
        {
            try
            {
                _clientService.UpdateClient(id, updatedClient);

                return Ok(updatedClient);
            }
            catch (NotFoundException ex)
            {
                return NotFound($"Client not found: {ex.Message}");
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
        public ActionResult DeleteClient(string id)
        {
            try
            {
                _clientService.DeleteClient(id);

                return Ok("The client has been deleted");
            }
            catch (NotFoundException ex)
            {
                return NotFound($"Client not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}