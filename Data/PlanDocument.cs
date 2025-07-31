using System;
using System.Collections.Generic;

namespace RimPoc.Data
{
    using System.Text.Json.Serialization;
    public class PlanDocument
    {
        public int Id { get; set; }
        public int PlanId { get; set; }
        public string Parent { get; set; } = string.Empty;      // Parent section name
        public string Section { get; set; } = string.Empty;     // Section or folder name
        public string LeafTitle { get; set; } = string.Empty;   // Document title (if applicable)
        public string FileName { get; set; } = string.Empty;    // File name (if applicable)
        public string Href { get; set; } = string.Empty;        // Path to file or folder
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? EstimatedDays { get; set; }
        [JsonIgnore]
        public Plan? Plan { get; set; }
        public ICollection<PlanDocumentSubmissionToCMap> SubmissionMappings { get; set; } = new List<PlanDocumentSubmissionToCMap>();
    }
}
