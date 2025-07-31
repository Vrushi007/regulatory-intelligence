using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RimPoc.Data;

namespace RimPoc.Services
{
    public interface IPlanService
    {
        // Task<Plan> CreatePlanWithCombinedToCAsync(string name, string description, List<int> submissionIds, List<PlanDocument> combinedToc, string? createdBy = null);
        Task<Plan> CreatePlanWithToCAndMappingsAsync(
            string name,
            string description,
            List<int> submissionIds,
            List<(PlanDocument planDoc, List<int> submissionToCIds)> planDocToSubmissionToCMap,
            string? createdBy = null);
        // Task<PlanDocument> AddDocumentToPlanAsync(int planId, string name, DateTime? startDate, DateTime? endDate, int? estimatedDays);
        Task MapDocumentToSubmissionToCAsync(int planDocumentId, List<int> submissionToCIds);
        Task SyncPlanDocumentToSubmissionsAsync(int planDocumentId);
    }

    public class PlanService : IPlanService
    {
        private readonly ApplicationDbContext _dbContext;

        public PlanService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates a plan, its ToC (PlanDocuments), and all PlanDocumentSubmissionToCMap entries in a single atomic operation.
        /// </summary>
        /// <param name="name">Plan name</param>
        /// <param name="description">Plan description</param>
        /// <param name="submissionIds">List of submission IDs to map to the plan</param>
        /// <param name="planDocToSubmissionToCMap">A list of tuples, each containing a PlanDocument definition and a list of SubmissionToC IDs to map</param>
        /// <param name="createdBy">User creating the plan</param>
        /// <returns>The created Plan</returns>
        public async Task<Plan> CreatePlanWithToCAndMappingsAsync(
            string name,
            string description,
            List<int> submissionIds,
            List<(PlanDocument planDoc, List<int> submissionToCIds)> planDocToSubmissionToCMap,
            string? createdBy = null)
        {
            var plan = new Plan
            {
                Name = name,
                Description = description,
                CreatedBy = createdBy,
                CreatedDate = DateTime.UtcNow
            };
            _dbContext.Plans.Add(plan);
            await _dbContext.SaveChangesAsync();

            // Add PlanDocuments and collect their IDs
            var planDocIdMap = new Dictionary<PlanDocument, int>();
            foreach (var (planDoc, _) in planDocToSubmissionToCMap)
            {
                planDoc.PlanId = plan.Id;
                _dbContext.PlanDocuments.Add(planDoc);
            }
            await _dbContext.SaveChangesAsync();

            // Map PlanDocuments to their DB IDs
            foreach (var (planDoc, _) in planDocToSubmissionToCMap)
            {
                var dbDoc = _dbContext.PlanDocuments.FirstOrDefault(d =>
                    d.PlanId == plan.Id &&
                    d.Parent == planDoc.Parent &&
                    d.Section == planDoc.Section &&
                    d.LeafTitle == planDoc.LeafTitle &&
                    d.FileName == planDoc.FileName &&
                    d.Href == planDoc.Href);
                if (dbDoc != null)
                {
                    planDocIdMap[planDoc] = dbDoc.Id;
                }
            }

            // Create PlanDocumentSubmissionToCMap entries
            foreach (var (planDoc, submissionToCIds) in planDocToSubmissionToCMap)
            {
                if (!planDocIdMap.TryGetValue(planDoc, out var planDocId)) continue;
                foreach (var tocId in submissionToCIds)
                {
                    var map = new PlanDocumentSubmissionToCMap
                    {
                        PlanDocumentId = planDocId,
                        SubmissionToCId = tocId
                    };
                    _dbContext.PlanDocumentSubmissionToCMaps.Add(map);
                }
            }
            await _dbContext.SaveChangesAsync();

            // Map submissions to plan
            foreach (var submissionId in submissionIds)
            {
                var map = new PlanSubmissionMap
                {
                    PlanId = plan.Id,
                    SubmissionId = submissionId
                };
                _dbContext.PlanSubmissionMaps.Add(map);
            }
            await _dbContext.SaveChangesAsync();

            return plan;
        }

        // public async Task<PlanDocument> AddDocumentToPlanAsync(int planId, string name, DateTime? startDate, DateTime? endDate, int? estimatedDays)
        // {
        //     var doc = new PlanDocument
        //     {
        //         PlanId = planId,
        //         Section = name, // For backward compatibility, treat name as section
        //         StartDate = startDate,
        //         EndDate = endDate,
        //         EstimatedDays = estimatedDays
        //     };
        //     _dbContext.PlanDocuments.Add(doc);
        //     await _dbContext.SaveChangesAsync();
        //     return doc;
        // }

        public async Task MapDocumentToSubmissionToCAsync(int planDocumentId, List<int> submissionToCIds)
        {
            foreach (var tocId in submissionToCIds)
            {
                var toc = await _dbContext.SubmissionToCs.FindAsync(tocId);
                if (toc == null) continue;
                // Try to find an existing plan document with same hierarchy
                var existingDoc = _dbContext.PlanDocuments.FirstOrDefault(d =>
                    d.PlanId == planDocumentId &&
                    d.Parent == toc.Parent &&
                    d.Section == toc.Section &&
                    d.LeafTitle == toc.LeafTitle);
                int docId;
                if (existingDoc != null)
                {
                    docId = existingDoc.Id;
                }
                else
                {
                    var newDoc = new PlanDocument
                    {
                        PlanId = planDocumentId,
                        Parent = toc.Parent,
                        Section = toc.Section,
                        LeafTitle = toc.LeafTitle,
                        FileName = toc.FileName,
                        Href = toc.Href
                    };
                    _dbContext.PlanDocuments.Add(newDoc);
                    await _dbContext.SaveChangesAsync();
                    docId = newDoc.Id;
                }
                var map = new PlanDocumentSubmissionToCMap
                {
                    PlanDocumentId = docId,
                    SubmissionToCId = tocId
                };
                _dbContext.PlanDocumentSubmissionToCMaps.Add(map);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task SyncPlanDocumentToSubmissionsAsync(int planDocumentId)
        {
            var planDoc = await _dbContext.PlanDocuments.FindAsync(planDocumentId);
            if (planDoc == null) return;
            var mappings = _dbContext.PlanDocumentSubmissionToCMaps
                .Where(m => m.PlanDocumentId == planDocumentId)
                .Include(m => m.SubmissionToC);
            foreach (var map in mappings)
            {
                var toc = map.SubmissionToC;
                toc.StartDate = planDoc.StartDate;
                toc.EndDate = planDoc.EndDate;
                toc.EstimatedDays = planDoc.EstimatedDays;
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
