using System;
using System.Collections.Generic;
using System.Linq;
using client.Db.Repositories.Interfaces;
using client.Models;

namespace client.src.Data.Repositories
{
    public class FakeRepository : IClientRepository
    {
        private List<Client> _clients = new List<Client>();

        public void CreateClient(Client client)
        {
            client.Id = Guid.NewGuid().ToString();

            _clients.Add(client);

        }

        public List<Client> GetClients()
        {
            return _clients;
        }

        public Client GetClientById(string id)
        {
            return _clients.FirstOrDefault(c => c.Id == id);
        }

        public void UpdateClient(Client updatedClient)
        {
            var existingClient = _clients.FirstOrDefault(c => c.Id == updatedClient.Id);
            if (existingClient != null)
            {
                existingClient.Name = updatedClient.Name;
                existingClient.Surname = updatedClient.Surname;
                existingClient.Email = updatedClient.Email;
                existingClient.BirthDate = updatedClient.BirthDate;
            }
        }

        public void DeleteClient(string id)
        {
            _clients.RemoveAll(c => c.Id == id);
        }
    }
}