using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RimPoc.Data;

public class ControlledVocabulary
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Value { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty; // Risk, MedicalSpecialty, ProductType, ProductSubtype, Functions, EnergySource, RadiationType

    public string? Country { get; set; } // Country codes (e.g., US, GB, FR, etc. or comma-separated values)

    public bool IsActive { get; set; } = true;

    public int? DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedAt { get; set; }

    // Self-referencing relationship for Product Type -> Product Subtypes
    public int? ParentId { get; set; }

    [JsonIgnore]
    public ControlledVocabulary? Parent { get; set; }

    [JsonIgnore]
    public ICollection<ControlledVocabulary> Children { get; set; } = new List<ControlledVocabulary>();

    // Navigation properties for entities that use this vocabulary
    [JsonIgnore]
    public ICollection<Application> ApplicationsAsRisk { get; set; } = new List<Application>();

    [JsonIgnore]
    public ICollection<Product> ProductsAsMedicalSpecialty { get; set; } = new List<Product>();

    [JsonIgnore]
    public ICollection<Product> ProductsAsType { get; set; } = new List<Product>();

    [JsonIgnore]
    public ICollection<Product> ProductsAsSubtype { get; set; } = new List<Product>();

    [JsonIgnore]
    public ICollection<Product> ProductsAsFunctions { get; set; } = new List<Product>();

    [JsonIgnore]
    public ICollection<Product> ProductsAsEnergySource { get; set; } = new List<Product>();

    [JsonIgnore]
    public ICollection<Product> ProductsAsRadiationType { get; set; } = new List<Product>();
}
