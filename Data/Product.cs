using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RimPoc.Data;

public class Product
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public string CodeName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }

    // Foreign key to ProductFamily
    public int ProductFamilyId { get; set; }

    // Foreign keys to ControlledVocabulary
    public int? MedicalSpecialtyId { get; set; }
    public int? TypeId { get; set; }
    public int? SubtypeId { get; set; }
    public int? FunctionsId { get; set; }
    public int? EnergySourceId { get; set; }
    public int? RadiationTypeId { get; set; }

    // Additional properties
    public bool RadiationEmitting { get; set; } = false;

    // Navigation property - many Products belong to one ProductFamily
    [JsonIgnore]
    public ProductFamily ProductFamily { get; set; } = null!;

    // Navigation properties to ControlledVocabulary
    [JsonIgnore]
    public ControlledVocabulary? MedicalSpecialty { get; set; }

    [JsonIgnore]
    public ControlledVocabulary? ProductType { get; set; }

    [JsonIgnore]
    public ControlledVocabulary? ProductSubtype { get; set; }

    [JsonIgnore]
    public ControlledVocabulary? Functions { get; set; }

    [JsonIgnore]
    public ControlledVocabulary? EnergySource { get; set; }

    [JsonIgnore]
    public ControlledVocabulary? RadiationType { get; set; }

    // ✅ New many-to-many relationship
    [JsonIgnore]
    public ICollection<Application> Applications { get; set; } = new List<Application>();

}