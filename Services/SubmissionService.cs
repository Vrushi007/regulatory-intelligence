using Microsoft.EntityFrameworkCore;
using RimPoc.Data;

namespace RimPoc.Services;

public interface ISubmissionService
{
    Task<Submission?> GetByIdAsync(int id);
    Task<IEnumerable<Submission>> GetAllAsync();
    Task<IEnumerable<Submission>> GetAllActiveAsync();
    Task<IEnumerable<Submission>> GetByApplicationIdAsync(int applicationId);
    Task<IEnumerable<Submission>> GetByStatusAsync(string status);
    Task<IEnumerable<Submission>> GetBySubmissionActivityIdAsync(int submissionActivityId);
    Task<Submission?> GetByApplicationAndSequenceAsync(int applicationId, string sequenceNumber);
    Task<Submission> CreateAsync(Submission submission);
    Task<Submission> UpdateAsync(Submission submission);
    Task<bool> DeleteAsync(int id);
    Task<string> GenerateNextSequenceNumberAsync(int applicationId);
}

public class SubmissionService : ISubmissionService
{
    private readonly ApplicationDbContext _context;

    public SubmissionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Submission?> GetByIdAsync(int id)
    {
        return await _context.Submissions
            .Include(s => s.Application)
                .ThenInclude(a => a.Country)
            .Include(s => s.SubmissionActivity)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Submission>> GetAllAsync()
    {
        return await _context.Submissions
            .Include(s => s.Application)
                .ThenInclude(a => a.Country)
            .Include(s => s.SubmissionActivity)
            .OrderBy(s => s.Application.Name)
            .ThenBy(s => s.SequenceNumber)
            .ToListAsync();
    }

    public async Task<IEnumerable<Submission>> GetAllActiveAsync()
    {
        return await _context.Submissions
            .Where(s => s.IsActive)
            .Include(s => s.Application)
                .ThenInclude(a => a.Country)
            .Include(s => s.SubmissionActivity)
            .OrderBy(s => s.Application.Name)
            .ThenBy(s => s.SequenceNumber)
            .ToListAsync();
    }

    public async Task<IEnumerable<Submission>> GetByApplicationIdAsync(int applicationId)
    {
        return await _context.Submissions
            .Where(s => s.ApplicationId == applicationId)
            .Include(s => s.Application)
            .Include(s => s.SubmissionActivity)
            .OrderBy(s => s.SequenceNumber)
            .ToListAsync();
    }

    public async Task<IEnumerable<Submission>> GetByStatusAsync(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return new List<Submission>();

        return await _context.Submissions
            .Where(s => s.Status == status && s.IsActive)
            .Include(s => s.Application)
                .ThenInclude(a => a.Country)
            .Include(s => s.SubmissionActivity)
            .OrderBy(s => s.Application.Name)
            .ThenBy(s => s.SequenceNumber)
            .ToListAsync();
    }

    public async Task<IEnumerable<Submission>> GetBySubmissionActivityIdAsync(int submissionActivityId)
    {
        return await _context.Submissions
            .Where(s => s.SubmissionActivityId == submissionActivityId && s.IsActive)
            .Include(s => s.Application)
                .ThenInclude(a => a.Country)
            .Include(s => s.SubmissionActivity)
            .OrderBy(s => s.Application.Name)
            .ThenBy(s => s.SequenceNumber)
            .ToListAsync();
    }

    public async Task<Submission?> GetByApplicationAndSequenceAsync(int applicationId, string sequenceNumber)
    {
        return await _context.Submissions
            .Include(s => s.Application)
            .Include(s => s.SubmissionActivity)
            .FirstOrDefaultAsync(s => s.ApplicationId == applicationId && s.SequenceNumber == sequenceNumber);
    }

    public async Task<string> GenerateNextSequenceNumberAsync(int applicationId)
    {
        // Get the latest sequence number for this application
        var latestSubmission = await _context.Submissions
            .Where(s => s.ApplicationId == applicationId)
            .OrderByDescending(s => s.SequenceNumber)
            .FirstOrDefaultAsync();

        if (latestSubmission == null)
        {
            // First submission for this application, start with 0000
            return "0000";
        }

        // Parse the current sequence number and increment it
        if (int.TryParse(latestSubmission.SequenceNumber, out int currentNumber))
        {
            int nextNumber = currentNumber + 1;
            return nextNumber.ToString("D4"); // Format as 4-digit number with leading zeros
        }

        // If parsing fails, return 0000 as fallback
        return "0000";
    }

    public async Task<Submission> CreateAsync(Submission submission)
    {
        if (submission == null)
            throw new ArgumentNullException(nameof(submission));

        // Validate that the application exists
        var applicationExists = await _context.Applications
            .AnyAsync(a => a.Id == submission.ApplicationId);
        if (!applicationExists)
            throw new ArgumentException($"Application with ID {submission.ApplicationId} does not exist.");

        // Validate that the submission activity exists and is of correct category
        var submissionActivity = await _context.ControlledVocabularies
            .FirstOrDefaultAsync(cv => cv.Id == submission.SubmissionActivityId && cv.Category == "SubmissionActivity");
        if (submissionActivity == null)
            throw new ArgumentException($"Submission Activity with ID {submission.SubmissionActivityId} does not exist or is not of category 'SubmissionActivity'.");

        // Generate sequence number if not provided
        if (string.IsNullOrWhiteSpace(submission.SequenceNumber))
        {
            submission.SequenceNumber = await GenerateNextSequenceNumberAsync(submission.ApplicationId);
        }
        else
        {
            // Validate that the sequence number is not already used for this application
            var existingSubmission = await _context.Submissions
                .FirstOrDefaultAsync(s => s.ApplicationId == submission.ApplicationId && s.SequenceNumber == submission.SequenceNumber);
            if (existingSubmission != null)
                throw new ArgumentException($"Sequence number {submission.SequenceNumber} already exists for application {submission.ApplicationId}.");
        }

        submission.CreatedAt = DateTime.UtcNow;
        submission.UpdatedAt = DateTime.UtcNow;

        _context.Submissions.Add(submission);
        await _context.SaveChangesAsync();
        
        // Return the created submission with related data
        return await GetByIdAsync(submission.Id) ?? submission;
    }

    public async Task<Submission> UpdateAsync(Submission submission)
    {
        if (submission == null)
            throw new ArgumentNullException(nameof(submission));

        var existingSubmission = await _context.Submissions
            .FirstOrDefaultAsync(s => s.Id == submission.Id);
        if (existingSubmission == null)
            throw new ArgumentException($"Submission with ID {submission.Id} does not exist.");

        // Update properties
        existingSubmission.Description = submission.Description;
        existingSubmission.SubmissionNumber = submission.SubmissionNumber;
        existingSubmission.SubmissionDate = submission.SubmissionDate;
        existingSubmission.Status = submission.Status;
        existingSubmission.StatusDate = submission.StatusDate;
        existingSubmission.IsActive = submission.IsActive;
        existingSubmission.UpdatedAt = DateTime.UtcNow;

        // Allow updating submission activity if provided
        if (submission.SubmissionActivityId != existingSubmission.SubmissionActivityId)
        {
            var submissionActivity = await _context.ControlledVocabularies
                .FirstOrDefaultAsync(cv => cv.Id == submission.SubmissionActivityId && cv.Category == "SubmissionActivity");
            if (submissionActivity == null)
                throw new ArgumentException($"Submission Activity with ID {submission.SubmissionActivityId} does not exist or is not of category 'SubmissionActivity'.");
            
            existingSubmission.SubmissionActivityId = submission.SubmissionActivityId;
        }

        await _context.SaveChangesAsync();
        
        // Return the updated submission with related data
        return await GetByIdAsync(submission.Id) ?? existingSubmission;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var submission = await _context.Submissions
            .FirstOrDefaultAsync(s => s.Id == id);

        if (submission == null)
            return false;

        // Soft delete - just mark as inactive
        submission.IsActive = false;
        submission.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
} 