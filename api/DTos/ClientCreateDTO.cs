using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace api.DTos;
using System.Text.Json.Serialization;

public class ClientCreateDTO
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public DateTime BirthDate { get; set; }
}