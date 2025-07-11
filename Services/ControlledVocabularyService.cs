using Microsoft.EntityFrameworkCore;
using RimPoc.Data;

namespace RimPoc.Services;

public interface IControlledVocabularyService
{
    Task<IEnumerable<ControlledVocabulary>> GetAllVocabulariesAsync();
    Task<IEnumerable<ControlledVocabulary>> GetVocabulariesByCategoryAsync(string category);
}

public class ControlledVocabularyService : IControlledVocabularyService
{
    private readonly ApplicationDbContext _context;

    public ControlledVocabularyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ControlledVocabulary>> GetAllVocabulariesAsync()
    {
        return await _context.ControlledVocabularies
            .Where(cv => cv.IsActive)
            .Include(cv => cv.Parent)
            .Include(cv => cv.Children.Where(c => c.IsActive))
            .OrderBy(cv => cv.Category)
            .ThenBy(cv => cv.DisplayOrder)
            .ThenBy(cv => cv.Value)
            .ToListAsync();
    }


    public async Task<IEnumerable<ControlledVocabulary>> GetVocabulariesByCategoryAsync(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return new List<ControlledVocabulary>();

        return await _context.ControlledVocabularies
            .Where(cv => cv.Category == category && cv.IsActive)
            .Include(cv => cv.Parent)
            .Include(cv => cv.Children.Where(c => c.IsActive))
            .OrderBy(cv => cv.DisplayOrder)
            .ThenBy(cv => cv.Value)
            .ToListAsync();
    }

    // public async Task<ControlledVocabulary> CreateVocabularyAsync(ControlledVocabulary vocabulary)
    // {
    //     if (vocabulary == null)
    //         throw new ArgumentNullException(nameof(vocabulary));

    //     // Validate required fields
    //     if (string.IsNullOrWhiteSpace(vocabulary.Code))
    //         throw new ArgumentException("Code is required");

    //     if (string.IsNullOrWhiteSpace(vocabulary.Value))
    //         throw new ArgumentException("Value is required");

    //     if (string.IsNullOrWhiteSpace(vocabulary.Category))
    //         throw new ArgumentException("Category is required");

    //     // Check if Code already exists
    //     var existingVocabulary = await _context.ControlledVocabularies
    //         .FirstOrDefaultAsync(cv => cv.Code == vocabulary.Code);
    //     if (existingVocabulary != null)
    //         throw new InvalidOperationException($"Vocabulary with Code '{vocabulary.Code}' already exists");

    //     // Validate parent if specified
    //     if (vocabulary.ParentId.HasValue)
    //     {
    //         var parent = await _context.ControlledVocabularies.FindAsync(vocabulary.ParentId.Value);
    //         if (parent == null)
    //             throw new ArgumentException($"Parent vocabulary with ID {vocabulary.ParentId} does not exist");
    //     }

    //     // Set audit fields
    //     vocabulary.CreatedAt = DateTime.UtcNow;
    //     vocabulary.IsActive = true;

    //     _context.ControlledVocabularies.Add(vocabulary);
    //     await _context.SaveChangesAsync();

    //     return await GetVocabularyByIdAsync(vocabulary.Id) ?? vocabulary;
    // }

    // public async Task<ControlledVocabulary?> UpdateVocabularyAsync(int id, ControlledVocabulary vocabulary)
    // {
    //     if (vocabulary == null)
    //         throw new ArgumentNullException(nameof(vocabulary));

    //     var existingVocabulary = await _context.ControlledVocabularies.FindAsync(id);
    //     if (existingVocabulary == null)
    //         return null;

    //     // Validate required fields
    //     if (string.IsNullOrWhiteSpace(vocabulary.Code))
    //         throw new ArgumentException("Code is required");

    //     if (string.IsNullOrWhiteSpace(vocabulary.Value))
    //         throw new ArgumentException("Value is required");

    //     if (string.IsNullOrWhiteSpace(vocabulary.Category))
    //         throw new ArgumentException("Category is required");

    //     // Check if Code already exists for another vocabulary
    //     var duplicateCode = await _context.ControlledVocabularies
    //         .FirstOrDefaultAsync(cv => cv.Code == vocabulary.Code && cv.Id != id);
    //     if (duplicateCode != null)
    //         throw new InvalidOperationException($"Vocabulary with Code '{vocabulary.Code}' already exists");

    //     // Validate parent if specified
    //     if (vocabulary.ParentId.HasValue)
    //     {
    //         var parent = await _context.ControlledVocabularies.FindAsync(vocabulary.ParentId.Value);
    //         if (parent == null)
    //             throw new ArgumentException($"Parent vocabulary with ID {vocabulary.ParentId} does not exist");
    //     }

    //     // Update properties
    //     existingVocabulary.Code = vocabulary.Code;
    //     existingVocabulary.Value = vocabulary.Value;
    //     existingVocabulary.Description = vocabulary.Description;
    //     existingVocabulary.Category = vocabulary.Category;
    //     existingVocabulary.IsActive = vocabulary.IsActive;
    //     existingVocabulary.DisplayOrder = vocabulary.DisplayOrder;
    //     existingVocabulary.ParentId = vocabulary.ParentId;
    //     existingVocabulary.ModifiedAt = DateTime.UtcNow;

    //     await _context.SaveChangesAsync();

    //     return await GetVocabularyByIdAsync(id);
    // }

    // public async Task<bool> DeleteVocabularyAsync(int id)
    // {
    //     var vocabulary = await _context.ControlledVocabularies
    //         .Include(cv => cv.Children)
    //         .FirstOrDefaultAsync(cv => cv.Id == id);

    //     if (vocabulary == null)
    //         return false;

    //     // Check if there are active children
    //     var hasActiveChildren = vocabulary.Children.Any(c => c.IsActive);
    //     if (hasActiveChildren)
    //     {
    //         throw new InvalidOperationException("Cannot delete vocabulary that has active children associated with it.");
    //     }

    //     // Soft delete by setting IsActive to false
    //     vocabulary.IsActive = false;
    //     vocabulary.ModifiedAt = DateTime.UtcNow;
    //     await _context.SaveChangesAsync();
    //     return true;
    // }


}
