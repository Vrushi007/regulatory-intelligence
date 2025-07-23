using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RimPoc.Data;

public class DefaultTemplates
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    // Foreign key to ControlledVocabulary for SubmissionType (category = "SubmissionActivity")
    public int SubmissionTypeId { get; set; }

    // Country code as string instead of foreign key
    [Required]
    public string Country { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    // Audit fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }

    // Navigation property
    [JsonIgnore]
    public ControlledVocabulary SubmissionType { get; set; } = null!;
}