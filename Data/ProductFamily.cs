using System.ComponentModel.DataAnnotations;

namespace RimPoc.Data;

public class ProductFamily
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedAt { get; set; }

    // Navigation property - one ProductFamily can have many Products
    public ICollection<Product> Products { get; set; } = new List<Product>();
}