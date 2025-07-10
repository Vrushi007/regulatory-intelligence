using Microsoft.EntityFrameworkCore;
using RimPoc.Data;

namespace RimPoc.Services;

public class ControlledVocabularyService
{
    private readonly ApplicationDbContext _context;

    public ControlledVocabularyService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all vocabularies by category
    public async Task<List<ControlledVocabulary>> GetVocabulariesByCategoryAsync(string category)
    {
        return await _context.ControlledVocabularies
            .Where(cv => cv.Category == category && cv.IsActive)
            .OrderBy(cv => cv.DisplayOrder)
            .ThenBy(cv => cv.Value)
            .ToListAsync();
    }

    // Get vocabulary with its children (for ProductType -> ProductSubtypes)
    public async Task<ControlledVocabulary?> GetVocabularyWithChildrenAsync(int id)
    {
        return await _context.ControlledVocabularies
            .Include(cv => cv.Children.Where(c => c.IsActive))
            .FirstOrDefaultAsync(cv => cv.Id == id && cv.IsActive);
    }

    // Get all product types with their subtypes
    public async Task<List<ControlledVocabulary>> GetProductTypesWithSubtypesAsync()
    {
        return await _context.ControlledVocabularies
            .Where(cv => cv.Category == "ProductType" && cv.ParentId == null && cv.IsActive)
            .Include(cv => cv.Children.Where(c => c.IsActive))
            .OrderBy(cv => cv.DisplayOrder)
            .ThenBy(cv => cv.Value)
            .ToListAsync();
    }

    // Get subtypes for a specific product type
    public async Task<List<ControlledVocabulary>> GetSubtypesForProductTypeAsync(int productTypeId)
    {
        return await _context.ControlledVocabularies
            .Where(cv => cv.Category == "ProductSubtype" && cv.ParentId == productTypeId && cv.IsActive)
            .OrderBy(cv => cv.DisplayOrder)
            .ThenBy(cv => cv.Value)
            .ToListAsync();
    }

    // Add new vocabulary
    public async Task<ControlledVocabulary> AddVocabularyAsync(ControlledVocabulary vocabulary)
    {
        _context.ControlledVocabularies.Add(vocabulary);
        await _context.SaveChangesAsync();
        return vocabulary;
    }

    // Update vocabulary
    public async Task<ControlledVocabulary?> UpdateVocabularyAsync(int id, ControlledVocabulary vocabulary)
    {
        var existing = await _context.ControlledVocabularies.FindAsync(id);
        if (existing == null) return null;

        existing.Code = vocabulary.Code;
        existing.Value = vocabulary.Value;
        existing.Description = vocabulary.Description;
        existing.Category = vocabulary.Category;
        existing.IsActive = vocabulary.IsActive;
        existing.DisplayOrder = vocabulary.DisplayOrder;
        existing.ParentId = vocabulary.ParentId;
        existing.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    // Soft delete vocabulary
    public async Task<bool> DeleteVocabularyAsync(int id)
    {
        var vocabulary = await _context.ControlledVocabularies.FindAsync(id);
        if (vocabulary == null) return false;

        vocabulary.IsActive = false;
        vocabulary.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    // Validate that a subtype belongs to the correct parent type
    public async Task<bool> ValidateSubtypeParentAsync(int subtypeId, int expectedParentId)
    {
        var subtype = await _context.ControlledVocabularies
            .FirstOrDefaultAsync(cv => cv.Id == subtypeId && cv.Category == "ProductSubtype");

        return subtype?.ParentId == expectedParentId;
    }

    // Get all risk levels
    public async Task<List<ControlledVocabulary>> GetRiskLevelsAsync()
    {
        return await GetVocabulariesByCategoryAsync("Risk");
    }

    // Get all classifications
    public async Task<List<ControlledVocabulary>> GetClassificationsAsync()
    {
        return await GetVocabulariesByCategoryAsync("Classification");
    }
}
