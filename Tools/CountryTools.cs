using System;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using RimPoc.Data;
using RimPoc.Services;

namespace RimPoc.Tools;

[McpServerToolType]
public class CountryTools
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CountryTools(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    [McpServerTool, Description("Get all countries")]
    public async Task<List<Country>> GetAllCountriesAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();
        return (List<Country>)await countryService.GetAllCountriesAsync();
    }

    [McpServerTool, Description("Get a country by its ID")]
    public async Task<Country?> GetCountryByIdAsync(
        [Description("The ID of the country to retrieve")] int id)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();
        return await countryService.GetCountryByIdAsync(id);
    }

    [McpServerTool, Description("Get a country by its code")]
    public async Task<Country?> GetCountryByCodeAsync(
        [Description("The 3-letter code of the country (e.g., USA, GBR, IND)")] string code)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();
        return await countryService.GetCountryByCodeAsync(code);
    }
}