using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Text.Json.Serialization;
using client.Models.DataAnnotations;


namespace client.Models.DTOs
{
    public class ClientUpdateDTO
    {
        [MinLength(3), MaxLength(80)]
        public string? Name { get; set; }

        [MinLength(3), MaxLength(80)]

        public string? Surname { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [DataType(DataType.Date), CustomerDateOfBirthValidation]
        public DateTime BirthDate { get; set; }
    }
}