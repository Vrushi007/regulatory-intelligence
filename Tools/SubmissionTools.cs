using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using RimPoc.Data;
using RimPoc.Services;

namespace RimPoc.Tools;

[McpServerToolType]
public class SubmissionTools
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SubmissionTools(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    // [McpServerTool, Description("Get all submissions")]
    // public async Task<List<Submission>> GetAllSubmissionsAsync()
    // {
    //     using var scope = _serviceScopeFactory.CreateScope();
    //     var submissionService = scope.ServiceProvider.GetRequiredService<ISubmissionService>();
    //     return (List<Submission>)await submissionService.GetAllActiveAsync();
    // }

    [McpServerTool, Description("Get all submissions for a specific application")]
    public async Task<List<Submission>> GetSubmissionsByApplicationIdAsync(
        [Description("The ID of the application")] int applicationId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var submissionService = scope.ServiceProvider.GetRequiredService<ISubmissionService>();
        return (List<Submission>)await submissionService.GetByApplicationIdAsync(applicationId);
    }

    public class CreateSubmissionResult
    {
        public Submission? CreatedSubmission { get; set; }
        public bool TocCreated { get; set; }
    }

    [McpServerTool, Description("Create a new submission for an application")]
    public async Task<CreateSubmissionResult> CreateSubmissionAsync(
        [Description("Required: The ID of the application this submission belongs to")] int applicationId,
        [Description("Required: The ID of the submission activity (from ControlledVocabulary with category 'SubmissionActivity')")] int submissionActivityId,
        [Description("Optional: Description of the submission")] string? description = null,
        [Description("Optional: Submission date (ISO format)")] string? submissionDate = null,
        [Description("Optional: Status of the submission")] string status = "Draft",
        [Description("Optional: Status date (ISO format)")] string? statusDate = null)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var submissionService = scope.ServiceProvider.GetRequiredService<ISubmissionService>();

        var submission = new Submission
        {
            ApplicationId = applicationId,
            SubmissionActivityId = submissionActivityId,
            Description = description,
            SubmissionNumber = submissionService.GenerateNextSequenceNumberAsync(applicationId).Result, // Generate sequence number
            SubmissionDate = string.IsNullOrEmpty(submissionDate) ? null : DateTime.Parse(submissionDate).ToUniversalTime(),
            Status = status,
            StatusDate = string.IsNullOrEmpty(statusDate) ? null : DateTime.Parse(statusDate).ToUniversalTime()
        };

        var createdSubmission = await submissionService.CreateAsync(submission);
        var tocCreated = false;
        if (createdSubmission != null)
        {
            tocCreated = await submissionService.PopulateSubmissionToCFromTemplateAsync(createdSubmission.Id);
        }

        return new CreateSubmissionResult
        {
            CreatedSubmission = createdSubmission,
            TocCreated = tocCreated
        };
    }

    [McpServerTool, Description("Update an existing submission")]
    public async Task<Submission> UpdateSubmissionAsync(
        [Description("The ID of the submission to update")] int id,
        [Description("Optional: New submission activity ID")] int? submissionActivityId = null,
        [Description("Optional: New description")] string? description = null,
        [Description("Optional: New external submission reference number")] string? submissionNumber = null,
        [Description("Optional: New submission date (ISO format)")] string? submissionDate = null,
        [Description("Optional: New status")] string? status = null,
        [Description("Optional: New status date (ISO format)")] string? statusDate = null,
        [Description("Optional: Whether the submission should be active")] bool? isActive = null)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var submissionService = scope.ServiceProvider.GetRequiredService<ISubmissionService>();

        // Get the existing submission first
        var existingSubmission = await submissionService.GetByIdAsync(id);
        if (existingSubmission == null)
            throw new ArgumentException($"Submission with ID {id} not found.");

        // Create updated submission with only changed values
        var updatedSubmission = new Submission
        {
            Id = id,
            ApplicationId = existingSubmission.ApplicationId, // Keep existing
            SequenceNumber = existingSubmission.SequenceNumber, // Keep existing
            SubmissionActivityId = submissionActivityId ?? existingSubmission.SubmissionActivityId,
            Description = description ?? existingSubmission.Description,
            SubmissionNumber = submissionNumber ?? existingSubmission.SubmissionNumber,
            SubmissionDate = string.IsNullOrEmpty(submissionDate) ? existingSubmission.SubmissionDate : DateTime.Parse(submissionDate).ToUniversalTime(),
            Status = status ?? existingSubmission.Status,
            StatusDate = string.IsNullOrEmpty(statusDate) ? existingSubmission.StatusDate : DateTime.Parse(statusDate).ToUniversalTime(),
            IsActive = isActive ?? existingSubmission.IsActive
        };

        return await submissionService.UpdateAsync(updatedSubmission);
    }

    [McpServerTool, Description("Delete a submission (soft delete - marks as inactive)")]
    public async Task<bool> DeleteSubmissionAsync(
        [Description("The ID of the submission to delete")] int id)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var submissionService = scope.ServiceProvider.GetRequiredService<ISubmissionService>();
        return await submissionService.DeleteAsync(id);
    }

    [McpServerTool, Description("Get the Table of Contents (ToC) for a specific submission")]
    public async Task<List<SubmissionToC>> GetSubmissionToCAsync(
        [Description("The ID of the submission to get ToC for")] int submissionId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var submissionService = scope.ServiceProvider.GetRequiredService<ISubmissionService>();
        var tocEntries = await submissionService.GetSubmissionToCAsync(submissionId);
        return tocEntries.ToList();
    }

    // [McpServerTool, Description("Update the Start Date, Estimated Days, and End Date for a specific submission ToC entry")]
    // public async Task<SubmissionToC> UpdateSubmissionToCAsync(
    //     [Description("The ID of the submission to update")] int submissionId,
    //     [Description("The ID of the submission ToC entry to update")] int submissionToCId,
    //     [Description("The new start date")] string? startDate = null,
    //     [Description("The new estimated days")] int? estimatedDays = null)
    // {
    //     using var scope = _serviceScopeFactory.CreateScope();
    //     var submissionService = scope.ServiceProvider.GetRequiredService<ISubmissionService>();
    //     var tocEntry = await submissionService.UpdateSubmissionToCDatesAsync(submissionId, submissionToCId, startDate, estimatedDays);
    //     if (tocEntry == null)
    //         throw new ArgumentException($"Submission ToC entry with ID {submissionToCId} not found.");
    //     return tocEntry;
    // }
}