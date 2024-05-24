using System;
using System.Collections.Generic;
using Refit;
using System.Text.Json.Serialization;
using MongoDB.Bson;

namespace sales.src.Services.Refit
{
    public interface IClient
    {
        [Get("/api/{id}")]
        Task<ApiResponse<Client>> GetClientById([AliasAs("id")] ObjectId id);
    }

    public class Client
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}