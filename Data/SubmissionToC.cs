using System.ComponentModel.DataAnnotations;

namespace RimPoc.Data;

public class SubmissionToC
{
    public int Id { get; set; }

    [Required]
    public string Parent { get; set; } = string.Empty;      // Parent section name

    [Required]
    public string Section { get; set; } = string.Empty;     // Section or folder name

    public string LeafTitle { get; set; } = string.Empty;   // Document title (if applicable)
    public string FileName { get; set; } = string.Empty;    // File name (if applicable)
    public string Href { get; set; } = string.Empty;        // Path to file or folder

    public int SubmissionId { get; set; }                    // Reference to Submissions
    public Submission? Submission { get; set; }             // Navigation property

    public DateTime? StartDate { get; set; }                // Start date of the submission item
    public int? EstimatedDays { get; set; }                 // Estimated days for completion
    public DateTime? EndDate { get; set; }                  // End date of the submission item
}
