using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RimPoc.Data;

namespace RimPoc.Services;

public class DefaultTemplateContentService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DefaultTemplateContentService> _logger;

    public DefaultTemplateContentService(ApplicationDbContext context, ILogger<DefaultTemplateContentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<DefaultTemplateContent>> GetByTemplateIdAsync(int templateId)
    {
        try
        {
            return await _context.DefaultTemplateContents
                .Include(tc => tc.Template)
                .Where(tc => tc.TemplateId == templateId)
                .OrderBy(tc => tc.Parent)
                .ThenBy(tc => tc.Section)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting default template contents for template ID {TemplateId}", templateId);
            throw;
        }
    }
}
