using System;
using api.Models;
using api.Models.DTOs;

namespace api.Services.Interfaces
{
    public interface IClientService
    {
        public void CreateClient(ClientCreateDTO clientCreateDto);
        public List<Client> GetClients();
        public Client GetClientById(string id);
        public void UpdateClient(string id, ClientUpdateDTO updatedClient);
        public void DeleteClient(string id);
        
    }
}