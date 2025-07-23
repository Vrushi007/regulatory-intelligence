using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RimPoc.Data;

public class Submission
{
    public int Id { get; set; }

    // Foreign key to Application
    public int ApplicationId { get; set; }

    [Required]
    [MaxLength(10)]
    public string SequenceNumber { get; set; } = string.Empty; // Format: 0000, 0001, 0002, etc.

    // Foreign key to ControlledVocabulary for Submission Activity
    public int SubmissionActivityId { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(100)]
    public string? SubmissionNumber { get; set; } // External submission reference number

    public DateTime? SubmissionDate { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = string.Empty; // Draft, Submitted, Approved, Rejected, etc.

    public DateTime? StatusDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true;

    // Navigation properties
    [JsonIgnore]
    public Application Application { get; set; } = null!;

    [JsonIgnore]
    public ControlledVocabulary SubmissionActivity { get; set; } = null!;
} 