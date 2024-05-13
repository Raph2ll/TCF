using System;
using client.Models;
using client.Db.Repositories.Interfaces;
using client.Models.DTOs;
using client.Exceptions;
using client.Services.Interfaces;
using Serilog;
using System;
using System.Diagnostics;
using System.Reflection;
using client.Utils;

namespace client.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly Serilog.ILogger _logger;
        private readonly ContextFactory _ctxFactory;
        private readonly string _namespace = "Service";

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _logger = Serilog.Log.ForContext<ClientService>();
            _ctxFactory = new ContextFactory(_logger);
        }

        public Client CreateClient(ClientCreateDTO createClientDto)
        {
            var methodName = $"{_namespace} {MethodBase.GetCurrentMethod()!.Name}";

            using (var ctx = _ctxFactory.Create(methodName))
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

                Client res = GetClientById(client.Id);
                return res;
            }
        }

        public List<Client> GetClients()
        {
            var methodName = $"{_namespace} {MethodBase.GetCurrentMethod()!.Name}";

            using (var ctx = _ctxFactory.Create(methodName))
            {
                return _clientRepository.GetClients();
            }
        }

        public Client GetClientById(string id)
        {
            var methodName = $"{_namespace} {MethodBase.GetCurrentMethod()!.Name}";

            using (var ctx = _ctxFactory.Create(methodName))
            {
                var existingClient = GetClientById(id);
                
                if (existingClient == null)
                {
                    throw new NotFoundException("Client Not Found");
                }
                return existingClient;
            }
        }

        public void UpdateClient(string id, ClientUpdateDTO updatedClientDto)
        {
            var methodName = $"{_namespace} {MethodBase.GetCurrentMethod()!.Name}";

            using (var ctx = _ctxFactory.Create(methodName))
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
        }

        public void DeleteClient(string id)
        {
            var methodName = $"{_namespace} {MethodBase.GetCurrentMethod()!.Name}";

            using (var ctx = _ctxFactory.Create(methodName))
            {
                if (GetClientById(id) == null)
                {
                    throw new NotFoundException("Client Not Found");
                }

                _clientRepository.DeleteClient(id);
            }
        }
    }
}