using Microsoft.Extensions.AI;
using RimPoc.Data;
using RimPoc.Services;
using System.ComponentModel;

namespace RimPoc.Tools;

public class ControlledVocabularyTools
{
    private readonly ControlledVocabularyService _vocabularyService;

    public ControlledVocabularyTools(ControlledVocabularyService vocabularyService)
    {
        _vocabularyService = vocabularyService;
    }

    [Description("Get all vocabularies by category (Risk, Classification, ProductType, ProductSubtype)")]
    public async Task<List<ControlledVocabulary>> GetVocabulariesByCategory(
        [Description("The category to filter by (Risk, Classification, ProductType, ProductSubtype)")]
        string category)
    {
        return await _vocabularyService.GetVocabulariesByCategoryAsync(category);
    }

    [Description("Get all product types with their associated subtypes")]
    public async Task<List<ControlledVocabulary>> GetProductTypesWithSubtypes()
    {
        return await _vocabularyService.GetProductTypesWithSubtypesAsync();
    }

    [Description("Get subtypes for a specific product type")]
    public async Task<List<ControlledVocabulary>> GetSubtypesForProductType(
        [Description("The ID of the product type")]
        int productTypeId)
    {
        return await _vocabularyService.GetSubtypesForProductTypeAsync(productTypeId);
    }

    [Description("Add a new controlled vocabulary entry")]
    public async Task<ControlledVocabulary> AddVocabulary(
        [Description("The unique code for the vocabulary")]
        string code,
        [Description("The display value")]
        string value,
        [Description("The category (Risk, Classification, ProductType, ProductSubtype)")]
        string category,
        [Description("Optional description")]
        string? description = null,
        [Description("Optional display order")]
        int? displayOrder = null,
        [Description("Optional parent ID for subtypes")]
        int? parentId = null)
    {
        var vocabulary = new ControlledVocabulary
        {
            Code = code,
            Value = value,
            Category = category,
            Description = description,
            DisplayOrder = displayOrder,
            ParentId = parentId,
            IsActive = true
        };

        return await _vocabularyService.AddVocabularyAsync(vocabulary);
    }

    [Description("Update an existing controlled vocabulary entry")]
    public async Task<ControlledVocabulary?> UpdateVocabulary(
        [Description("The ID of the vocabulary to update")]
        int id,
        [Description("The unique code")]
        string code,
        [Description("The display value")]
        string value,
        [Description("The category")]
        string category,
        [Description("Optional description")]
        string? description = null,
        [Description("Optional display order")]
        int? displayOrder = null,
        [Description("Whether the vocabulary is active")]
        bool isActive = true,
        [Description("Optional parent ID for subtypes")]
        int? parentId = null)
    {
        var vocabulary = new ControlledVocabulary
        {
            Code = code,
            Value = value,
            Category = category,
            Description = description,
            DisplayOrder = displayOrder,
            ParentId = parentId,
            IsActive = isActive
        };

        return await _vocabularyService.UpdateVocabularyAsync(id, vocabulary);
    }

    [Description("Soft delete a controlled vocabulary entry")]
    public async Task<bool> DeleteVocabulary(
        [Description("The ID of the vocabulary to delete")]
        int id)
    {
        return await _vocabularyService.DeleteVocabularyAsync(id);
    }

    [Description("Get all risk levels")]
    public async Task<List<ControlledVocabulary>> GetRiskLevels()
    {
        return await _vocabularyService.GetRiskLevelsAsync();
    }

    [Description("Get all classifications")]
    public async Task<List<ControlledVocabulary>> GetClassifications()
    {
        return await _vocabularyService.GetClassificationsAsync();
    }

    [Description("Validate that a product subtype belongs to the correct product type")]
    public async Task<bool> ValidateSubtypeParent(
        [Description("The subtype ID")]
        int subtypeId,
        [Description("The expected parent type ID")]
        int expectedParentId)
    {
        return await _vocabularyService.ValidateSubtypeParentAsync(subtypeId, expectedParentId);
    }
}
