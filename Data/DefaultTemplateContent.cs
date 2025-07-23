using System.ComponentModel.DataAnnotations;

namespace RimPoc.Data;

public class DefaultTemplateContent
{
    public int Id { get; set; }

    [Required]
    public string Parent { get; set; } = string.Empty;      // Parent section name

    [Required]
    public string Section { get; set; } = string.Empty;     // Section or folder name

    public string LeafTitle { get; set; } = string.Empty;   // Document title (if applicable)
    public string FileName { get; set; } = string.Empty;    // File name (if applicable)
    public string Href { get; set; } = string.Empty;        // Path to file or folder

    public int TemplateId { get; set; }                      // Reference to DefaultTemplates
    public DefaultTemplates? Template { get; set; }         // Navigation property
}
