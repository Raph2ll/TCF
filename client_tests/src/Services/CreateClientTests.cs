using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using System.Threading.Tasks;
using client.Services;
using Xunit;
using client.Models.DTOs;
using client.Db.Repositories.Interfaces;
using Moq;
using client.Models;
using ZstdSharp.Unsafe;
using System.Diagnostics;
using Microsoft.VisualBasic;
using client.Db.Repositories;

namespace client_tests.src.Services
{
    public class CreateClientTests
    {
        [Fact]
        public void CreateClient_IsCalled_CallsClientServiceCreateMethod()
        {
            // Arrange
            var addClientCreateDTO = new Fixture().Create<ClientCreateDTO>();

            var repositoryMock = new Mock<IClientRepository>();
            var clientService = new ClientService(repositoryMock.Object);

            // Act
            var result = clientService.CreateClient(addClientCreateDTO);

            // Assert
            Assert.NotNull(result); // Verifica se result não é null
            Assert.Equal(addClientCreateDTO.Name, result.Name);

            repositoryMock.Verify(mock => mock.CreateClient(It.IsAny<Client>()), Times.Once);

        }
    }
}