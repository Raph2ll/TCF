using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using client.Models;
using client.Services.Interfaces;
using System.Linq;
using client.Models.DTOs;
using Microsoft.AspNetCore.Http;
using client.Exceptions;
using System.ComponentModel.DataAnnotations;


namespace api.Controllers
{
    [ApiController]
    [Route("api")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        /// <summary>
        /// Create a client.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="client">Client's data</param>
        /// <returns></returns>
        /// <response code="201">Success in creating the client</response>
        /// <response code="400">Malformed request</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(Client), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateClient(ClientCreateDTO client)
        {
            try
            {
                var res = _clientService.CreateClient(client);
                return StatusCode(201, res);
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
        /// Get clients.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        /// <response code="201">Success in creating the client</response>
        /// <response code="500">Internal server error</response>
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

        /// <summary>
        /// Get client by id.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        /// <response code="201">Success in get the client</response>
        /// <response code="404">Client not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Client>> GetClientById(string id)
        {
            try
            {
                var client = _clientService.GetClientById(id);
                return Ok(client);
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
        
        /// <summary>
        /// Edit client by id.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">Client id</param>
        /// <returns></returns>
        /// <response code="200">Return new client</response>
        /// <response code="404">Client not found</response>
        /// <response code="500">Internal server error</response>
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
                return NotFound($"{ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Soft delete client by id.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">Client id</param>
        /// <returns></returns>
        /// <response code="200">The client has been deleted</response>
        /// <response code="404">Client not found</response>
        /// <response code="500">Internal server error</response>
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