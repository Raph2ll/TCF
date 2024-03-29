using System;
using api.Models;
using api.Data.Repositories.Interfaces;
using api.Services.Interfaces;
using api.DTos;
using client.Exceptions;

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