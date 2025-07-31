using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RimPoc.Data;

namespace RimPoc.Data
{
    public class PlanSubmissionMap
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Plan")]
        public int PlanId { get; set; }
        public Plan Plan { get; set; }

        [ForeignKey("Submission")]
        public int SubmissionId { get; set; }
        public Submission Submission { get; set; }
    }
}
