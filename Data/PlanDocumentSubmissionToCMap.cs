namespace RimPoc.Data
{
    public class PlanDocumentSubmissionToCMap
    {
        public int Id { get; set; }
        public int PlanDocumentId { get; set; }
        public int SubmissionToCId { get; set; }
        public PlanDocument? PlanDocument { get; set; }
        public SubmissionToC? SubmissionToC { get; set; }
    }
}
