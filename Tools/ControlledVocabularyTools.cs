using ModelContextProtocol.Server;
using RimPoc.Data;
using RimPoc.Services;
using System.ComponentModel;

namespace RimPoc.Tools;

[McpServerToolType]
public class ControlledVocabularyTools
{
    private readonly IControlledVocabularyService _vocabularyService;

    public ControlledVocabularyTools(IControlledVocabularyService vocabularyService)
    {
        _vocabularyService = vocabularyService;
    }

    [McpServerTool, Description("Get distinct categories of controlled vocabularies")]
    public async Task<List<string>> GetDistinctCategories()
    {
        var vocabularies = await _vocabularyService.GetAllVocabulariesAsync();
        return vocabularies
            .Select(v => v.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToList();
    }

    [McpServerTool, Description("Get all vocabularies(also called as Controlled Vocabularies)")]
    public async Task<List<ControlledVocabulary>> GetAllVocabularies()
    {
        return (await _vocabularyService.GetAllVocabulariesAsync()).ToList();
    }

    [McpServerTool, Description("Get all vocabularies(also called as Controlled Vocabularies) by category -> Get the distinct categories using GetDistinctCategories()")]
    public async Task<List<ControlledVocabulary>> GetVocabulariesByCategory(
        [Description("The category to filter vocabularies by. Use GetDistinctCategories() to get valid categories.")]
        string category)
    {
        return (await _vocabularyService.GetVocabulariesByCategoryAsync(category)).ToList();
    }
}
