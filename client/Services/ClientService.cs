using System;
using api.Models;
using api.Data.Repositories.Interfaces;
using api.Services.Interfaces;
using api.DTos;
using api.Models;

namespace api.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        
        public void CreateClient(ClientCreateDTO createClientDto)
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
        
        public List<Client> GetClients()
        {
            return _clientRepository.GetClients();
        }

        public Client GetClientById(string id)
        {
            return _clientRepository.GetClientById(id);
        }

        public void UpdateClient(string id,ClientUpdateDTO updatedClient)
        {
            var client = new Client()
            {
                Id = id,
                Name = updatedClient.Name,
                Surname = updatedClient.Surname,
                Email = updatedClient.Email,
                BirthDate = updatedClient.BirthDate
            };
            
            _clientRepository.UpdateClient(client);
        }

        public void DeleteClient(string id)
        {
            _clientRepository.DeleteClient(id);
        }

    }
}