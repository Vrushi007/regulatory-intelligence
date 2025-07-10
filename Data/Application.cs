using System.ComponentModel.DataAnnotations;

namespace RimPoc.Data;

public class Application
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string SerialNum { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;

    public int CountryId { get; set; }
    public Country Country { get; set; } = null!;

    [MaxLength(50)]
    public string AppNumber { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    public DateTime? StatusDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // ✅ New many-to-many relationship
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
