namespace product.Models.DTOs;

public class ProductCreateDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}