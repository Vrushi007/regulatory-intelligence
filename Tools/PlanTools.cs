using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using RimPoc.Data;
using RimPoc.Services;

namespace RimPoc.Tools;

[McpServerToolType]
public class PlanTools
{
    public class PlanDocumentWithSubmissionToCIds : PlanDocument
    {
        public List<int>? SubmissionToCIds { get; set; }
    }
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PlanTools(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    // [McpServerTool, Description("Create a new plan from multiple submissions and a combined ToC structure")]
    // public async Task<Plan> CreatePlanWithCombinedToCAsync(
    //     [Description("The name of the plan")] string name,
    //     [Description("The description of the plan")] string description,
    //     [Description("List of submission IDs to compare")] List<int> submissionIds,
    //     [Description("Combined ToC structure as a list of plan documents")] List<PlanDocument> combinedToc,
    //     [Description("The user creating the plan (optional)")] string? createdBy = null)
    // {
    //     using var scope = _serviceScopeFactory.CreateScope();
    //     var planService = scope.ServiceProvider.GetRequiredService<PlanService>();
    //     return await planService.CreatePlanWithCombinedToCAsync(name, description, submissionIds, combinedToc, createdBy);
    // }

    [McpServerTool, Description("Create a new plan, its ToC, and all mappings in a single atomic operation")]
    public async Task<Plan> CreatePlanWithToCAndMappingsAsync(
        [Description("The name of the plan")] string name,
        [Description("The description of the plan")] string description,
        [Description("List of submission IDs to map")] List<int> submissionIds,
        [Description("List of plan documents (ToC), this includes the plan document details and it's corresponding linked SubmissionToC IDs")] List<PlanDocumentWithSubmissionToCIds> planDocuments,
        [Description("The user creating the plan (optional)")] string? createdBy = null)
    {
        var tupleList = new List<(PlanDocument, List<int>)>();
        for (int i = 0; i < planDocuments.Count; i++)
        {
            tupleList.Add((planDocuments[i], planDocuments[i].SubmissionToCIds ?? new List<int>()));
        }
        using var scope = _serviceScopeFactory.CreateScope();
        var planService = scope.ServiceProvider.GetRequiredService<PlanService>();
        return await planService.CreatePlanWithToCAndMappingsAsync(name, description, submissionIds, tupleList, createdBy);
    }

    // [McpServerTool, Description("Add a document to a plan")]
    // public async Task<PlanDocument> AddDocumentToPlanAsync(
    //     [Description("The ID of the plan")] int planId,
    //     [Description("Parent section name")] string parent,
    //     [Description("Section or folder name")] string section,
    //     [Description("Document title")] string leafTitle,
    //     [Description("File name")] string fileName,
    //     [Description("Href/path")] string href,
    //     [Description("Start date")] string? startDate = null,
    //     [Description("End date")] string? endDate = null,
    //     [Description("Estimated days")] int? estimatedDays = null)
    // {
    //     using var scope = _serviceScopeFactory.CreateScope();
    //     var planService = scope.ServiceProvider.GetRequiredService<PlanService>();
    //     var doc = new PlanDocument
    //     {
    //         PlanId = planId,
    //         Parent = parent,
    //         Section = section,
    //         LeafTitle = leafTitle,
    //         FileName = fileName,
    //         Href = href,
    //         StartDate = string.IsNullOrEmpty(startDate) ? null : DateTime.Parse(startDate),
    //         EndDate = string.IsNullOrEmpty(endDate) ? null : DateTime.Parse(endDate),
    //         EstimatedDays = estimatedDays
    //     };
    //     scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().PlanDocuments.Add(doc);
    //     await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().SaveChangesAsync();
    //     return doc;
    // }

    // [McpServerTool, Description("Map a plan document to SubmissionToC entries")]
    // public async Task<bool> MapDocumentToSubmissionToCAsync(
    //     [Description("The ID of the plan document")] int planDocumentId,
    //     [Description("List of SubmissionToC IDs")] List<int> submissionToCIds)
    // {
    //     using var scope = _serviceScopeFactory.CreateScope();
    //     var planService = scope.ServiceProvider.GetRequiredService<PlanService>();
    //     await planService.MapDocumentToSubmissionToCAsync(planDocumentId, submissionToCIds);
    //     return true;
    // }

    [McpServerTool, Description("Sync a plan document's dates to all mapped SubmissionToC entries")]
    public async Task<bool> SyncPlanDocumentToSubmissionsAsync(
        [Description("The ID of the plan document")] int planDocumentId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var planService = scope.ServiceProvider.GetRequiredService<PlanService>();
        await planService.SyncPlanDocumentToSubmissionsAsync(planDocumentId);
        return true;
    }
}
