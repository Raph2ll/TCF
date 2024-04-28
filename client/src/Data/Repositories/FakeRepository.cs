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
            // Atribuir um novo ID ao cliente
            client.Id = Guid.NewGuid().ToString();

            // Adicionar o cliente à lista
            _clients.Add(client);

        }

        public List<Client> GetClients()
        {
            // Retornar todos os clientes na lista
            return _clients;
        }

        public Client GetClientById(string id)
        {
            // Encontrar e retornar o cliente pelo ID
            return _clients.FirstOrDefault(c => c.Id == id);
        }

        public void UpdateClient(Client updatedClient)
        {
            // Encontrar o cliente na lista pelo ID
            var existingClient = _clients.FirstOrDefault(c => c.Id == updatedClient.Id);
            if (existingClient != null)
            {
                // Atualizar os dados do cliente existente com os dados atualizados
                existingClient.Name = updatedClient.Name;
                existingClient.Surname = updatedClient.Surname;
                existingClient.Email = updatedClient.Email;
                existingClient.BirthDate = updatedClient.BirthDate;
            }
        }

        public void DeleteClient(string id)
        {
            // Remover o cliente da lista pelo ID
            _clients.RemoveAll(c => c.Id == id);
        }
    }
}