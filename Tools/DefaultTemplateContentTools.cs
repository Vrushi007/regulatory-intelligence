using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using RimPoc.Data;
using RimPoc.Services;

namespace RimPoc.Tools;

public class DefaultTemplateContentTools
{
    private readonly DefaultTemplateContentService _service;

    public DefaultTemplateContentTools(DefaultTemplateContentService service)
    {
        _service = service;
    }

    [Description("Get default template contents by template ID")]
    public async Task<List<DefaultTemplateContent>> GetDefaultTemplateContentsByTemplateIdAsync(int templateId)
    {
        return await _service.GetByTemplateIdAsync(templateId);
    }
}
