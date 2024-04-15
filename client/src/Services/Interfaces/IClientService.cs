using System;
using client.Models;
using client.Models.DTOs;

namespace client.Services.Interfaces
{
    public interface IClientService
    {
        public Client CreateClient(ClientCreateDTO clientCreateDto);
        public List<Client> GetClients();
        public Client GetClientById(string id);
        public void UpdateClient(string id, ClientUpdateDTO updatedClient);
        public void DeleteClient(string id);
        
    }
}