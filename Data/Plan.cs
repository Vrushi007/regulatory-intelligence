using System;
using System.Collections.Generic;

namespace RimPoc.Data
{
    using System.Text.Json.Serialization;
    public class Plan
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public ICollection<PlanDocument> Documents { get; set; } = new List<PlanDocument>();
    }
}
