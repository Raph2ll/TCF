using System;
using client.Models;
using client.Data.Repositories.Interfaces;
using client.Models.DTOs;
using client.Exceptions;
using client.Services.Interfaces;
using Serilog;
using System;
using System.Diagnostics;
using System.Reflection;

namespace client.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly Serilog.ILogger _logger;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _logger = Serilog.Log.ForContext<ClientService>();
        }

        public void CreateClient(ClientCreateDTO createClientDto)
        {
            var stopwatch = Stopwatch.StartNew();
            string methodName = MethodBase.GetCurrentMethod().Name;
            _logger.Information($"{methodName} method started.");

            try
            {
                var client = new Client()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = createClientDto.Name,
                    Surname = createClientDto.Surname,
                    Email = createClientDto.Email,
                    BirthDate = createClientDto.BirthDate
                };

                _clientRepository.CreateClient(client);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in CreateClient method.");
                throw;
            }
            stopwatch.Stop();

            double elapsedMilliseconds = stopwatch.Elapsed.TotalMilliseconds;

            _logger.Information($"{methodName} method completed in {elapsedMilliseconds.ToString("0.####")}ms");
        }

        public List<Client> GetClients()
        {
            
            return _clientRepository.GetClients();
        }

        public Client GetClientById(string id)
        {
            return _clientRepository.GetClientById(id);
        }

        public void UpdateClient(string id, ClientUpdateDTO updatedClientDto)
        {
            var existingClient = GetClientById(id);

            if (existingClient == null)
            {
                throw new NotFoundException("Client Not Found");
            }

            string updatedName = existingClient.Name;
            if (updatedClientDto.Name != "")
            {
                updatedName = updatedClientDto.Name;
            }

            string updatedSurname = existingClient.Surname;
            if (updatedClientDto.Surname != "")
            {
                updatedSurname = updatedClientDto.Surname;
            }

            string updatedEmail = existingClient.Email;
            if (updatedClientDto.Email != "")
            {
                updatedEmail = updatedClientDto.Email;
            }

            DateTime updatedBirthDate = existingClient.BirthDate;
            if (updatedClientDto.BirthDate != default(DateTime))
            {
                updatedBirthDate = updatedClientDto.BirthDate;
            }

            var updatedClient = new Client
            {
                Id = existingClient.Id,
                Name = updatedName,
                Surname = updatedSurname,
                Email = updatedEmail,
                BirthDate = updatedBirthDate
            };

            _clientRepository.UpdateClient(updatedClient);
        }


        public void DeleteClient(string id)
        {
            if (GetClientById(id) == null)
            {
                throw new NotFoundException("Client Not Found");
            }

            _clientRepository.DeleteClient(id);
        }

    }
}