using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Text.Json.Serialization;
using client.Models.DataAnnotations;

namespace client.Models.DTOs
{
    public class ClientCreateDTO
    {
        [Required, MinLength(3), MaxLength(80)]
        public string? Name { get; set; }

        [Required, MinLength(3), MaxLength(80)]

        public string? Surname { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, DataType(DataType.Date), CustomerDateOfBirthValidation]
        public DateTime BirthDate { get; set; }
    }
}
