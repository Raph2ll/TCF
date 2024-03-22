using System;
using api.Models;
using api.Data.Repositories.Interfaces;
using api.Services.Interfaces;

namespace api.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientrepository _clientRepository;

        public ClientService(IClientrepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public void CreateClient(Client client)
        {
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

        public void UpdateClient(Client updatedClient)
        {
            _clientRepository.UpdateClient(updatedClient);
        }

        public void DeleteClient(string id)
        {
            _clientRepository.DeleteClient(id);
        }

    }
}