using System;
using api.Models;

namespace api.Services.Interfaces
{
    public interface IClientService
    {
        public void CreateClient(Client client);
        public List<Client> GetClients();
        public Client GetClientById(string id);
        public void UpdateClient(Client updatedClient);
        public void DeleteClient(string id);
        
    }
}