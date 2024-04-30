using System.Text.Json.Serialization;

namespace product.Models;

public class ProductResponseDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    [JsonPropertyName("BRL")]
    public decimal BRL { get; set; }
    [JsonPropertyName("EUR")]
    public decimal EUR { get; set; }
    [JsonPropertyName("USD")]
    public decimal USD { get; set; }
    [JsonPropertyName("GBP")]
    public decimal GBP { get; set; }
    [JsonPropertyName("CNY")]
    public decimal CNY { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Deleted { get; set; }
}